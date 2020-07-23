using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public GameObject boomPanel;
    bool _restarting = false;

    private void Start()
    {
        boomPanel.SetActive(false);
        _restarting = false;
    }

    public void HitHappened()
    {
        if (!_restarting) { StartCoroutine(Restart()); }
        
        //Debug.Log("Hit something");
    }


    IEnumerator Restart()
    {
        boomPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(0);
        
    }

}
