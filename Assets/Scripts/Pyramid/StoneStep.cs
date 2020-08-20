using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStep : MonoBehaviour
{

    public int rowNumber = 0;

    public float speed = 5f;

    bool _isWinBlock = false;
    public bool IsWinBlock
    {
        get
        {
            return _isWinBlock;
        }
    }

    public Pyramid pyramid;

    int position = 3;

    GameObject blocks;
    float height;

    GameObject passTrigger;
    [SerializeField] GameObject light;


    bool isRaising = false;

    bool isTriggered = false;

    Vector3 newPos;


    private void Awake()
    {
        blocks = transform.GetChild(0).gameObject;

        Renderer rend = blocks.GetComponent<Renderer>();
        height = rend.bounds.size.y;

        passTrigger = transform.GetChild(1).gameObject;
        passTrigger.SetActive(false);

        light.SetActive(false);

    }



    // Update is called once per frame
    void Update()
    {
        if (isRaising)
        {
            if (blocks.transform.position.y < newPos.y)
            {
                blocks.transform.Translate(0,speed * Time.deltaTime,0);
            }
            else
            {
                isRaising = false;
            }
        }
    }


    public void Lock()
    {
        isTriggered = true;
    }


    public void Trigger()
    {

        isTriggered = true;
        pyramid.LockSteps(rowNumber);

        switch (position)
        {
            case 1:

                pyramid.DeepFall();

                break;

            case 2:

                pyramid.Fall();

                break;

            case 3:

                if (!_isWinBlock)
                {
                    pyramid.PassObstacle();
                }
                else
                {
                    pyramid.Win();
                }

                break;

            case 4:

                pyramid.HitObstacle();

                break;

            default:
                break;
        }
    }


    public void  WinBlock()
    {
        passTrigger.SetActive(true);
        _isWinBlock = true;
    }


    public void InitPosition(int nom)
    {

        position = nom;

        light.SetActive(true);
        passTrigger.SetActive(true);

        switch (nom)
        {
            case 1:

                blocks.transform.position = new Vector3(blocks.transform.position.x, blocks.transform.position.y - height * 2, blocks.transform.position.z);
                break;

            case 2:
                blocks.transform.position = new Vector3(blocks.transform.position.x, blocks.transform.position.y - height, blocks.transform.position.z);
                break;


            default:
                Debug.Log("Position № " + nom.ToString() + " will not be initialized.");
                break;
        }
    }

    public void RaiseSteps()
    {

        if (!isRaising && position < 4 && !isTriggered)
        {

            isRaising = true;

            
            newPos = new Vector3(blocks.transform.position.x, blocks.transform.position.y + height, blocks.transform.position.z);
         
            position++;

            switch (position)
            {
                case 3:

                    light.SetActive(false);
                    break;

                case 4:
                    //bumpTrigger.SetActive(true);
                    break;

                default:
                    break;
            }

        }
    }




}
