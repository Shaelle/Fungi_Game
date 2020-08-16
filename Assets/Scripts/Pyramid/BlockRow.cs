using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRow : MonoBehaviour
{

    [SerializeField] GameObject[] blocks;
    [SerializeField] int number = 1;

    // Start is called before the first frame update
    void Start()
    {

        GenerateRow();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GenerateRow()
    {

        if (blocks.Length > 0)
        {
            Vector3 pos = transform.position;
            for (int i = 0; i <= number; i++)
            {
                GameObject block;

                block = Instantiate(blocks[Random.Range(0, blocks.Length - 1)]) as GameObject;

                block.transform.position = pos;
                block.transform.Rotate(0, 90, 0);

                pos.x += block.GetComponent<Renderer>().bounds.size.x;
            }
        }
    }
}
