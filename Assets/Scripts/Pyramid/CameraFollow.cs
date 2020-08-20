
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 10f;

    public Vector3 offset;

    Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position,desiredPosition, ref velocity, smoothTime*Time.deltaTime);

        transform.position = smoothPosition;

    }
}
