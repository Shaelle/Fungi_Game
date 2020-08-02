using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    public Animator rocket;
    public Animator explosion;

    public void Explode()
    {
        rocket.Play("RocketDissapear");
        explosion.Play("Explosion");       
    }


}
