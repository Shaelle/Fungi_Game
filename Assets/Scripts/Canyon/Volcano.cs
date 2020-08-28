using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{

    GameObject erruption;

    float speedBoost = 0;

    private void Awake()
    {

        erruption = transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }



    public void Errupt()
    {
        erruption.SetActive(true);
    }


}
