using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Can_Manager : MonoBehaviour
{

    public float obstaclesInterval = 0.5f;
    public float spawnDistance = 30f;

    public int health = 5;

    public GameObject player;

    [SerializeField] Can_Obstacle[] obstaclePrefabs;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI powerText;

    [SerializeField] int chargePerHit = 5;


    [SerializeField] GameObject volcano;


    [SerializeField] GameObject powerPanel;
    Image powerImage;
    Animator powerAnimator;

    int power = 0;

    public bool isCharged = false;



    bool obstacleCreated;



    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
        powerText.text = power.ToString()+"%";

        powerImage = powerPanel.GetComponent<Image>();
        powerAnimator = powerPanel.GetComponent<Animator>();

        powerAnimator.enabled = false;

        //powerImage.color = Color.white;
    }



    // Update is called once per frame
    void Update()
    {

        if (!obstacleCreated) { StartCoroutine(CreateObstacle()); }
        
    }



    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }




    public void HitEdge()
    {
        if (!isCharged)
        {
            health--;
            healthText.text = health.ToString();

        }
    }



    public void HitTarget()
    {
        if (!isCharged)
        {
            power += chargePerHit;

            powerText.text = power.ToString() + "%";

            if (power >= 100)
            {
                isCharged = true;

                powerAnimator.enabled = true;
                powerImage.color = Color.red;

                player.transform.GetChild(0).LookAt(volcano.transform);


            }
        }
    }


    IEnumerator CreateObstacle()
    {
        obstacleCreated = true;

        Vector3 spawn = new Vector3(Random.Range(-10, 10), 20, player.transform.position.z + spawnDistance);


        if (obstaclePrefabs.Length > 0)
        {
            int nom = Random.Range(0, obstaclePrefabs.Length - 1);

            Can_Obstacle obstacle = Instantiate(obstaclePrefabs[nom]) as Can_Obstacle;
            obstacle.transform.position = spawn;

            RaycastHit hit;
            if (Physics.Raycast(obstacle.transform.position, -Vector3.up, out hit))
            {
                obstacle.transform.position = hit.point;
                obstacle.transform.Translate(0, 2, 0);
            }
        }

        yield return new WaitForSeconds(obstaclesInterval);


        obstacleCreated = false;
    }

     

}
