using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackground : MonoBehaviour
{

    [SerializeField] Sprite[] images;



    public void ChangeBackground(int nom)
    {

        SpriteRenderer background = GetComponent<SpriteRenderer>();

        if ((background != null) && (images.Length > 0))
        {

            int temp;

            if ((nom < images.Length) && (nom > 0)) { temp = nom--; }

            else { temp = 0; }

            background.sprite = images[temp];
        }

    }
}
