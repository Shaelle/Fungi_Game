using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitinScreen : MonoBehaviour
{

    private Vector2 screenBoundaries;

    private float objectWidth;
    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void LateUpdate()
    {

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, screenBoundaries.x * -1 + objectWidth, screenBoundaries.x - objectWidth);
        pos.y = Mathf.Clamp(pos.y, screenBoundaries.y * -1 + objectHeight, screenBoundaries.y - objectHeight);

        transform.position = pos;
    }

}
