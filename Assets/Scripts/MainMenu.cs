using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadAsteroids()
    {
        SceneManager.LoadScene("Space1");
    }

    public void LoadCanyon()
    {
        SceneManager.LoadScene("Canyon");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
