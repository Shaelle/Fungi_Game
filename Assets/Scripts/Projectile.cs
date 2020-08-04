using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 50f;

    public Can_Manager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        Can_Obstacle obstacle = other.GetComponent<Can_Obstacle>();
        if (obstacle != null)
        {
            levelManager.HitTarget();
            obstacle.Kill();
        }
        else Debug.Log("hit someting");

        Destroy(gameObject);
    }
}
