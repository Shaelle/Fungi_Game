using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BaseLevelManager : MonoBehaviour
{

    [SerializeField] GameObject boomPanel;
    [SerializeField] GameObject winPanel;


    [SerializeField] string nextScene;

    [SerializeField] Animator transition;

    [SerializeField] float transitionTime = 1f;

    bool _restarting = false;
    bool _isWin = false;

    string sceneName;




    private void Start()
    {
        boomPanel.SetActive(false);
        winPanel.SetActive(false);

        sceneName = SceneManager.GetActiveScene().name;

       if (string.IsNullOrEmpty(nextScene))
        {
            nextScene = sceneName;
        }
         
    }



    public void Loose()
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



    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }



    public void Win()
    {
        if (!_restarting)
        {
            _isWin = true;

            StartCoroutine(Winning());
        }

    }



    IEnumerator Restart()
    {
        boomPanel.SetActive(true);

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
