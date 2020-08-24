using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingletonClass<GameManager>{

    public Action MouseControl = delegate {};
    public Action EndGame = delegate {};
    public   bool stopMouse {set; get;} = true;
    private void Update() {
    //    if(Input.GetMouseButtonDown(0)) {
     //       MouseControl();
     //   }
    }

    private void Start() {
        Button.Instance.StartGame += StartGame;
        Button.Instance.RestartGame += Loader;
    }

    private void StartGame() {
         stopMouse = true;
    }

    public void GameOver() {
        EndGame();
    stopMouse = false;
        
    }

    private void Loader() {
        
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
