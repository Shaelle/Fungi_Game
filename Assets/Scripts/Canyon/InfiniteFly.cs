using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFly : MonoBehaviour
{

    public float speed = 3f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);       
    }




}
