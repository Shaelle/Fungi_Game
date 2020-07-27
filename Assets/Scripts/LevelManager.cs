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

    [SerializeField] string nextScene;

    bool _restarting = false;
    bool _isWin = false;

    string sceneName;



    private void Start()
    {
        boomPanel.SetActive(false);
        winPanel.SetActive(false);

        sceneName = SceneManager.GetActiveScene().name;

        coins = totalCoins;
        coinsText.text = coins.ToString();

        float x; float y;
        float z = 0;

        for (int i = 0; i < coinsOnLevel; i++) // spawn coins on random positions
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



    public void TargetReached()
    {
        if (!_restarting)
        {
            _isWin = true;

            AddCoins(victoryCoins);

            totalCoins = coins;

            rocket.WinRound(target);

            StartCoroutine(Winning());
        }
    }



    IEnumerator Restart()
    {
        boomPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneName);
        
    }



    IEnumerator Winning()
    {
        winPanel.SetActive(true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(nextScene); 
    }

}
