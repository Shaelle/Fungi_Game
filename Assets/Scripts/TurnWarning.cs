using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnWarning : MonoBehaviour
{

     Image warningImage;

    private void Awake()
    {
        warningImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        warningImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.deviceOrientation != DeviceOrientation.LandscapeLeft && Input.deviceOrientation != DeviceOrientation.Unknown)
        {
            warningImage.enabled = true;
            Debug.Log("Enabled");
        }
        else
        {
            warningImage.enabled = false;
        }

    }
}
