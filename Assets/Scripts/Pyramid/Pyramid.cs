using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{

    [SerializeField] StoneStep[] blocks;
    [SerializeField] GameObject[] decorativeBlocks;

    [SerializeField] int rowBlocks = 1;
    [SerializeField] int rows = 1;

    [SerializeField] int allSizeBlockLimit = 5; // needed for prevent visual artifacts nearby summit. TODO: find a better name.
    [SerializeField] int interactiveBlocksRadius = 10;


    [SerializeField] ClimberMovement player;


    Animator animator;

    [SerializeField] BaseLevelManager levelManager;


    [SerializeField] float rowTurnX = 0.12f;
    [SerializeField] float rowTurnZ = 0.3f;

    [SerializeField] float RowPosX = 0.18f;
    [SerializeField] float RowPosY = 0.1f;
    [SerializeField] float RowPozZ = 0.16f;

    [SerializeField] int holesMinDistance = 10;

    [SerializeField] [Range(1, 100)] int holeChance = 60;
    [SerializeField] [Range(1, 100)] int lowerHoleChance = 50;


    PlayerInput controls;
   

    Vector3 top;

    List<int> rowsWithHoles;
    List<int> deeperHoles;

    List<StoneStep> holes;

    int currObstacles = 0;

    const float holeRaduis = 1.5f;

    bool isPassingObstacle = false;
    bool isWinning = false;


    private void Awake()
    {
        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => RaiseSteps();

        animator = player.transform.GetChild(0).GetComponent<Animator>();

    }


    // Start is called before the first frame update
    void Start()
    {
         GeneratePyramid();


        player.transform.position = new Vector3(top.x, player.transform.position.y, player.transform.position.z); // TODO: bug — sometimes do nothing.


        rowsWithHoles = new List<int>();
        deeperHoles = new List<int>();


        holes = new List<StoneStep>();

        int lastHole = 0;
        for (int i = 0; i < rows; i++)
        {
            bool isHole = Random.Range(1, 100) <= holeChance;
            bool lowerHole = Random.Range(1, 100) <= lowerHoleChance;

            if (isHole && i > lastHole + holesMinDistance)
            {
                rowsWithHoles.Add(i);
                lastHole = i;

                if (lowerHole)
                {
                    deeperHoles.Add(i);
                }
            }
        }

        foreach (StoneStep block in FindObjectsOfType<StoneStep>())
        {
            
            if ((block.transform.position.x > top.x - holeRaduis) && (block.transform.position.x < top.x + holeRaduis))
            {
                if (!block.IsWinBlock)
                {
                    if (rowsWithHoles.Contains(block.rowNumber))
                    {
                        holes.Add(block);


                        if (deeperHoles.Contains(block.rowNumber))
                        {
                            block.InitPosition(1);
                        }
                        else
                        {
                            block.InitPosition(2);
                        }
                    }
                }
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public void LockSteps(int rowNumber)
    {

        foreach (StoneStep block in holes)
        {
            if (block.rowNumber == rowNumber)
            {
                block.Lock();
            }
        }

    }


    void RaiseSteps()
    {

        foreach (StoneStep block in holes)
        {
            if (block.rowNumber == rowsWithHoles[currObstacles])
            {
                block.RaiseSteps();
            }

        }

    }


    public void PassObstacle()
    {
        if (!isPassingObstacle)
        {
            StartCoroutine(PassingTimer());
            currObstacles++;
        }
    }



    public void HitObstacle()
    {
        animator.SetTrigger("Hit");

        levelManager.Loose();
    }


    public void DeepFall()
    {
        animator.SetTrigger("Fall");

        levelManager.Loose();
    }

    public void Fall()
    {
        animator.SetTrigger("Stopped");

        levelManager.Loose();
    }




    IEnumerator PassingTimer()
    {
        isPassingObstacle = true;

        yield return new WaitForSeconds(0.1f);

        isPassingObstacle = false;
    }


    public void Win()
    {
        if (!isWinning)
        {
            isWinning = true;
            StartCoroutine(Winning());
        }

    }



    IEnumerator Winning()
    {
        yield return new WaitForSeconds(0.2f);

        player.isMoving = false;

        animator.SetTrigger("Win");

        yield return new WaitForSeconds(2f);

        levelManager.Win();

    }





    public void GeneratePyramid()
    {
        Vector3 rowPos = transform.position;

        int currNumber = rowBlocks;

        Renderer rend = blocks[0].transform.GetChild(0).gameObject.GetComponent<Renderer>();

        for (int i = 0; i < rows; i++) // frontal rows
        {    
            
            Vector3 temp =  GenerateRow(rowPos, currNumber, i, 1);


            temp.x += rend.bounds.size.x - rowTurnX;
            temp.z += rend.bounds.size.z - rowTurnZ;

            temp = GenerateRow(temp, currNumber, i, 2);

            temp.x -= rend.bounds.size.x - rowTurnX;
            temp.z += rend.bounds.size.z - rowTurnZ;

            temp = GenerateRow(temp, currNumber, i, 3);

            temp.x -= rend.bounds.size.x - rowTurnX;
            temp.z -= rend.bounds.size.z - rowTurnZ;

            GenerateRow(temp, currNumber, i, 4);


            rowPos.z += rend.bounds.size.z / 2 - RowPozZ;
            rowPos.y += rend.bounds.size.y - RowPosY;

            rowPos.x += rend.bounds.size.x + RowPosX;// / 2;
            
            currNumber--;
        }

    }




    private Vector3 GenerateRow(Vector3 startPos, int blockNumber, int rowNumber, int rotation)
    {

        Vector3 finalPoint = startPos;
      
        Vector3 pos = startPos;



        int minInteractive = Mathf.RoundToInt(blockNumber / 2) - interactiveBlocksRadius;

        int maxInteractive = Mathf.RoundToInt(blockNumber / 2) + interactiveBlocksRadius;

        void PlaceBlock(GameObject block, Renderer rend)
        {

            block.transform.position = pos;

            switch (rotation)
            {
                case 1:

                    block.transform.Rotate(0, 90, 0);
                    pos.x += rend.bounds.size.x;
                    break;

                case 2:

                    block.transform.Rotate(0, 0, 0);
                    pos.z += rend.bounds.size.z;
                    break;

                case 3:

                    block.transform.Rotate(0, 270, 0);
                    pos.x -= rend.bounds.size.x;
                    break;

                case 4:

                    block.transform.Rotate(0, 180, 0);
                    pos.z -= rend.bounds.size.z;
                    break;

                default:
                    break;
            }

            finalPoint = block.transform.position;

            top = block.transform.position;
        }


        for (int i = 0; i < blockNumber; i++)
        {
            bool canInteract = (i >= minInteractive) && (i <= maxInteractive);

            if ((canInteract && rotation == 1) || blockNumber <= allSizeBlockLimit)
            {
                StoneStep block;

                if (blocks.Length > 0)
                {
                    block = Instantiate(blocks[Random.Range(0, blocks.Length - 1)]) as StoneStep;

                    block.pyramid = this;

                    block.rowNumber = rowNumber + 1;


                    PlaceBlock(block.gameObject, block.transform.GetChild(0).gameObject.GetComponent<Renderer>());

                    if (blockNumber == 1)
                    {
                        block.WinBlock();
                    }
                }
            }
            else
            {
                GameObject block;

                if (decorativeBlocks.Length > 0)
                {
                    block = Instantiate(decorativeBlocks[Random.Range(0, blocks.Length - 1)]) as GameObject;

                    PlaceBlock(block, block.GetComponent<Renderer>());

                }

            }


        }


        return finalPoint;
    }



    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }
}
