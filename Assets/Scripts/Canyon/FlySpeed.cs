using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpeed : MonoBehaviour
{
    public InfiniteFly player;
    public InfiniteFly volcano;

    public float normalSpeed = 6;
    public float maxSpeed = 20;

    public float speedBoostRate = 5;
    public float acceleration = 2;



    float speedBoost = 0;

    // Start is called before the first frame update
    void Start()
    {
        speedBoost = normalSpeed * speedBoostRate;

        player.speed = normalSpeed;
        volcano.speed = normalSpeed;

        StartCoroutine(BoostSpeed());

    }


    public void ResetSpeed()
    {
        player.speed = normalSpeed;
        volcano.speed = normalSpeed;
    }


    IEnumerator BoostSpeed()
    {

        while (speedBoost > normalSpeed)
        {
            player.speed = speedBoost;
            volcano.speed = speedBoost;

            yield return new WaitForSeconds(0.3f);

            speedBoost--;
        }

        player.speed = normalSpeed;
        volcano.speed = normalSpeed;

        StartCoroutine(Accelerate());
    }


    IEnumerator Accelerate()
    {
        float currSpeed = normalSpeed;

        while (currSpeed <= maxSpeed)
        {
            player.speed = currSpeed;
            volcano.speed = currSpeed;

            yield return new WaitForSeconds(1f);

            currSpeed += acceleration;
        }
    }


}
