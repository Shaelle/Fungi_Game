using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Can_Manager : MonoBehaviour
{

    public float obstaclesInterval = 0.5f;
    public float spawnDistance = 30f;

    public int health = 5;

   int power = 0;

    public GameObject player;

    [SerializeField] Can_Obstacle obstaclePrefab;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI powerText;

    bool obstacleCreated;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
        powerText.text = power.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (!obstacleCreated) { StartCoroutine(CreateObstacle()); }
        
    }

    public void HitEdge()
    {
        health--;
        healthText.text = health.ToString();
    }


    IEnumerator CreateObstacle()
    {
        obstacleCreated = true;

        Vector3 spawn = new Vector3(Random.Range(-7, 7), Random.Range(1, 15), player.transform.position.z + spawnDistance);

        Can_Obstacle obstacle = Instantiate(obstaclePrefab) as Can_Obstacle;
        obstacle.transform.position = spawn;     

        yield return new WaitForSeconds(obstaclesInterval);


        obstacleCreated = false;
    }

     

}
