using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{

    [SerializeField] StoneStep[] blocks;
    [SerializeField] int rowBlocks = 1;
    [SerializeField] int rows = 1;


    [SerializeField] ClimberMovement player;


    [SerializeField] float rowTurnX = 0.12f;
    [SerializeField] float rowTurnZ = 0.3f;

    [SerializeField] float RowPosX = 0.18f;
    [SerializeField] float RowPosY = 0.1f;
    [SerializeField] float RowPozZ = 0.16f;

    [SerializeField] int holesDistance = 10;

    [SerializeField] [Range(1, 100)] int holeChance = 60; 
   

    Vector3 top;


    // Start is called before the first frame update
    void Start()
    {
         GeneratePyramid();

        player.transform.position = new Vector3(top.x, player.transform.position.y, player.transform.position.z);


        foreach (StoneStep block in FindObjectsOfType<StoneStep>())
        {
            
            if ((block.transform.position.x > top.x - 1f) && (block.transform.position.x < top.x + 1f))
            {

                bool isHole = Random.Range(1, 100) <= holeChance;

                bool correctRow = block.rowNumber % holesDistance == 0;


                if (isHole && correctRow)
                {
                    block.gameObject.SetActive(false);
                }
            }
        }

         
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }



    public void GeneratePyramid()
    {
        Vector3 rowPos = transform.position;

        int currNumber = rowBlocks;

        Renderer rend = blocks[0].GetComponent<Renderer>();

        for (int i = 0; i < rows; i++) // frontal rows
        {    
            
            Vector3 temp =  GenerateRow(rowPos, currNumber, i, false);

            temp.x += rend.bounds.size.x - rowTurnX;
            temp.z += rend.bounds.size.z - rowTurnZ;

            GenerateRow(temp, currNumber, i, true);

            rowPos.z += rend.bounds.size.z/2 - RowPozZ;
            rowPos.y += rend.bounds.size.y - RowPosY;

            rowPos.x += rend.bounds.size.x + RowPosX;// / 2;

            currNumber--;
        }

    }




    private Vector3 GenerateRow(Vector3 startPos, int blockNumber, int rowNumber, bool rotate)
    {

        Vector3 finalPoint = startPos;

        if (blocks.Length > 0)
        {
            Vector3 pos = startPos;
            for (int i = 0; i < blockNumber; i++)
            {
                StoneStep block;

                block = Instantiate(blocks[Random.Range(0, blocks.Length - 1)]) as StoneStep;

                block.transform.position = pos;

                block.rowNumber = rowNumber + 1;

                Renderer rend = block.GetComponent<Renderer>();


                if (!rotate)
                {
                    block.transform.Rotate(0, 90, 0);

                    pos.x += rend.bounds.size.x;
                }
                else
                {
                    pos.z += rend.bounds.size.z;
                }

                finalPoint = block.transform.position;

                top = block.transform.position;

                //bool isHole = Random.Range(1, 5) >= 4;

                //if (isHole && !rotate && i == Mathf.Round(blockNumber / 2)) { block.SetActive(false); }
            }

        }

        return finalPoint;
    }
}
