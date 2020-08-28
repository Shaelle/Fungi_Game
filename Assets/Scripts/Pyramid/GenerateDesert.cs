using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDesert : MonoBehaviour
{
    public GameObject plane;


    int halfTilesX = 60;
    int halfTilesZ = 25;
    int planeSize = 10;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;


        for (int x = -halfTilesX; x < halfTilesX; x++)
        {
            for (int z = -halfTilesZ; z < halfTilesZ; z++)
            {
                Vector3 pos = new Vector3(
                    (x * planeSize + startPos.x),
                    0,
                    (z * planeSize + startPos.z));

                GameObject t = (GameObject)Instantiate(plane, pos, Quaternion.identity);
                t.transform.parent = this.transform;

                string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                t.name = tileName;


            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
