using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTrigger : MonoBehaviour
{

    [SerializeField] StoneStep stoneStep;

    private void OnTriggerEnter(Collider other)
    {
        stoneStep.Trigger();
    }


}
