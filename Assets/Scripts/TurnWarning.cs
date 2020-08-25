using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnWarning : MonoBehaviour
{

    Image image;
    Image panel;

    Animator animator;

    private void Awake()
    {

        panel = transform.GetChild(0).GetComponent<Image>();
        image = transform.GetChild(1).GetComponent<Image>();
        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        panel.enabled = false;
        image.enabled = false;
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.deviceOrientation != DeviceOrientation.LandscapeLeft && Input.deviceOrientation != DeviceOrientation.Unknown)
        {
            panel.enabled = true;
            image.enabled = true;
            animator.enabled = true;
        }
        else
        {
            panel.enabled = false;
            image.enabled = false;
            animator.enabled = false;
        }

    }


}
