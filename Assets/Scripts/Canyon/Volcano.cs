using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{

    InfiniteFly fly;

    GameObject erruption;

    float speedBoost = 0;

    private void Awake()
    {
        fly = GetComponent<InfiniteFly>();

        erruption = transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        speedBoost = fly.speed * 5;

        StartCoroutine(BoostSpeed());
    }


    IEnumerator BoostSpeed()
    {
        float normalSpeed = fly.speed;

        while (speedBoost > normalSpeed)
        {
            fly.speed = speedBoost;

            yield return new WaitForSeconds(0.3f);

            speedBoost--;
        }

        fly.speed = normalSpeed;
    }

    public void Errupt()
    {
        erruption.SetActive(true);
    }


}
