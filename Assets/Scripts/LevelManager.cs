using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject boomPanel;
    [SerializeField] GameObject winPanel;

    [SerializeField] Coin coin;

    [SerializeField] int coinsOnLevel = 5;

    [SerializeField] TextMeshProUGUI coinsText;

    int coins = 0;
    static int totalCoins = 0;

    [SerializeField] int victoryCoins = 100;

    [SerializeField] RocketMovement rocket;
    [SerializeField] Transform target;
    [SerializeField] Transform homePlanet;

    [SerializeField] Sprite[] planets;

    [SerializeField] string nextScene;

    [SerializeField] SwitchBackground background;


    [SerializeField] Animator transition;

    [SerializeField] float transitionTime = 1f;

    bool _restarting = false;
    bool _isWin = false;

    bool isTakingOff = true;

    string sceneName;

    static int sceneNom = 1;


    private void Start()
    {
        boomPanel.SetActive(false);
        winPanel.SetActive(false);

        sceneName = SceneManager.GetActiveScene().name;

        coins = totalCoins;
        coinsText.text = coins.ToString();

        SpriteRenderer homeImage = homePlanet.GetComponent<SpriteRenderer>();
        homeImage.sprite = planets[PlanetIndex(sceneNom-1)];

        SpriteRenderer targetImage = target.GetComponent<SpriteRenderer>();
        targetImage.sprite = planets[PlanetIndex(sceneNom)];

      
        background.ChangeBackground(sceneNom-1);
 
     
        float x; float y; // spawn coins on random positions
        float z = 0;

        for (int i = 0; i < coinsOnLevel; i++) 
        {
            bool coinSpawned = false;

            while (!coinSpawned)
            {
                x = Random.Range(-9, 9);
                y = Random.Range(-4, 4);

                Vector3 pos = new Vector3(x, y, z);
                if ((pos-target.transform.position).magnitude < 2f) // prevent coin from spawn on the planets
                {
                    continue;
                }
                else if ((homePlanet != null) && ((pos-homePlanet.transform.position).magnitude < 2f))
                {
                    continue;
                }
                else               
                {
                    Coin newCoin = Instantiate(coin) as Coin; // spawn coin

                    newCoin.levelManager = this;

                    newCoin.transform.position = pos;

                    coinSpawned = true;
                }
            }

        }

        StartCoroutine(FinishingTakeOff());

    }


    private int PlanetIndex(int nom)
    {
        int temp;

        if ((nom < planets.Length) && (nom > 0))
        {
            temp = nom--;
        }
        else
        {
            temp = 0;
        }

        return temp;
       
    }


    public void FinishTakeOff() // Prevent home planet from triggering "landing" event
    {
        isTakingOff = false;
    }



    IEnumerator FinishingTakeOff() // Backup option in case starting point can not be landed
    {
        yield return new WaitForSeconds(3f);

        isTakingOff = false;

    }




    public void AddCoins(int quantity)
    {
        coins += quantity;

        coinsText.text = coins.ToString();

    }



    public void HitHappened()
    {

        if (!_isWin)
        {
            if (!_restarting)
            {
                _restarting = true;
                StartCoroutine(Restart());
            }
        }
        
    }



    public void TargetReached(Transform targetPos)
    {
        if (!_restarting && !isTakingOff)
        {
            _isWin = true;

            AddCoins(victoryCoins);

            totalCoins = coins;

            rocket.WinRound(targetPos);

            StartCoroutine(Winning());
         
            if (sceneNom < planets.Length) // changing levels
            {
                sceneNom++;
            }
            else
            {
                sceneNom = 1;
            }

        }

    }



    IEnumerator Restart()
    {
        boomPanel.SetActive(true);

        rocket.CutEngine();
        rocket.GetComponent<Spaceship>().Explode();

        yield return new WaitForSeconds(1f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
        
    }



    IEnumerator Winning()
    {
        winPanel.SetActive(true);

        yield return new WaitForSeconds(4f);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(nextScene); 
    }

}
