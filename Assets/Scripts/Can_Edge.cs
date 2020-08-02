using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can_Edge : MonoBehaviour
{

    [SerializeField] Can_Manager manager;
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Can_Obstacle obstacle = other.gameObject.GetComponent<Can_Obstacle>();

        if  (obstacle != null)
        {
            obstacle.Kill();

            manager.HitEdge();

        }

    }


    private void Update()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 0.1f);
        
    }


}
