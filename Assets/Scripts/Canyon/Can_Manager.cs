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


    [SerializeField] GameObject boomPanel;
    [SerializeField] GameObject winPanel;

    [SerializeField] Animator transition;

    [SerializeField] float transitionTime = 1f;

    string sceneName;


    [SerializeField] Volcano volcano;


    [SerializeField] GenerateInfinite land;


    [SerializeField] GameObject powerPanel;
    Image powerImage;
    Animator powerAnimator;

    int power = 0;

    public bool isCharged = false;

    public bool restarting = false;



    bool obstacleCreated;



    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
        powerText.text = power.ToString()+"%";

        powerImage = powerPanel.GetComponent<Image>();
        powerAnimator = powerPanel.GetComponent<Animator>();

        powerAnimator.enabled = false;

        sceneName = SceneManager.GetActiveScene().name;

        //powerImage.color = Color.white;

        StartCoroutine(ShortInterval());

    }


    IEnumerator ShortInterval()
    {
        float normalInterval = obstaclesInterval;


        obstaclesInterval = obstaclesInterval / 2;

        yield return new WaitForSeconds(1.5f);
    
        obstaclesInterval = normalInterval;
    }



    IEnumerator Restart()
    {

        restarting = true; 

        boomPanel.SetActive(true);


        yield return new WaitForSeconds(1f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

    }


    public void Win()
    {
        StartCoroutine(Winning());
    }



    IEnumerator Winning()
    {

        restarting = true;

        power = 0;
        powerText.text = power.ToString() + "%";

        yield return new WaitForSeconds(4f);

        volcano.Errupt();

        yield return new WaitForSeconds(2f);
     
        winPanel.SetActive(true);

        yield return new WaitForSeconds(4f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

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
        if (!isCharged && !restarting)
        {
            health--;
            healthText.text = health.ToString();

            if (health <= 0)
            {
                StartCoroutine(Restart());               
            }

        }
    }



    public void HitTarget()
    {
        if (!isCharged && !restarting)
        {
            power += chargePerHit;

            powerText.text = power.ToString() + "%";

            if (power >= 100)
            {
                isCharged = true;

                power = 100;
                powerText.text = power.ToString() + "%";

                powerAnimator.enabled = true;
                powerImage.color = Color.red;

                player.transform.GetChild(0).LookAt(volcano.transform);


            }
        }
    }


    public void Shoot()
    {
        power--;

        if (power < 0) power = 0;

        powerText.text = power.ToString() + "%";
    }


    IEnumerator CreateObstacle()
    {
        obstacleCreated = true;

        GenerateObstacle(new Vector3(Random.Range(-10, 10), 20, player.transform.position.z + spawnDistance));


        yield return new WaitForSeconds(obstaclesInterval);


        obstacleCreated = false;
    }


    private void GenerateObstacle(Vector3 spawn)
    {

        const float rise = 3f;

        if (obstaclePrefabs.Length > 0)
        {
            int nom = Random.Range(0, obstaclePrefabs.Length - 1);

            Can_Obstacle obstacle = Instantiate(obstaclePrefabs[nom]) as Can_Obstacle;
            obstacle.transform.position = spawn;

            RaycastHit hit;
            if (Physics.Raycast(obstacle.transform.position, -Vector3.up, out hit))
            {
                obstacle.transform.position = hit.point;
                obstacle.transform.Translate(0, 2 + rise, 0);                
            }

            if (obstacle.transform.position.y <= 2 + rise)
            {
                //Debug.Log("Obstacle too low: y = " + obstacle.transform.position.y + ". Removing it.");
                obstacle.Kill();
            }


            float x = obstacle.transform.position.x;

            if (x < -7) { obstacle.transform.Rotate(0, 0, -60); }
            else if (x < -4) { obstacle.transform.Rotate(0, 0, -30); }
            else if (x > 4) { obstacle.transform.Rotate(0, 0, 30); }
            else if (x > 7) { obstacle.transform.Rotate(0, 0, 60); }

            

                                                             
        }
    }

     

}
