using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRotationj : MonoBehaviour
{

    public Transform asteroid;
    public float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        asteroid.Rotate(0, 0, speed * Time.deltaTime);
    }
}
