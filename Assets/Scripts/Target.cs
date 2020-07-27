using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelManager.TargetReached();
    }


}
