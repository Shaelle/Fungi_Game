using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] bool destination = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelManager.TargetReached(transform, destination);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        levelManager.FinishTakeOff();
    }


}
