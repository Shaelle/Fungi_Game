﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] LevelManager sceneManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
         sceneManager.HitHappened();
    }

}
