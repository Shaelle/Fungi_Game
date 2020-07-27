using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRotationj : MonoBehaviour
{

    [SerializeField] Transform asteroid;
    [SerializeField] float speed = 3f;


    // Update is called once per frame
    void Update()
    {
        asteroid.Rotate(0, 0, speed * Time.deltaTime);
    }
}
