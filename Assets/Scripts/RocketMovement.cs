using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{

    public GameObject rocket;
    public GameObject destinationPoint;
    public float speed = 3f;
    public float turnSpeed = 3f;

    PlayerInput controls;

    CharacterController controller;

    private Vector2 pointerPosition;

    const float moveThreshold = 0.1f;

    private void Awake()
    {
        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => MoveToClick();
        controls.Main.PointerPosition.performed += ctx => pointerPosition = ctx.ReadValue<Vector2>();


        controller = GetComponent<CharacterController>();
        if (controller == null) { Debug.Log("Character controller not found!"); }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = new Vector3(destinationPoint.transform.position.x - rocket.transform.position.x, destinationPoint.transform.position.y - rocket.transform.position.y, 0f);


        if (direction.magnitude > moveThreshold)
        {

            Vector3 vectorToTarget = destinationPoint.transform.position - rocket.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            rocket.transform.rotation = Quaternion.Slerp(rocket.transform.rotation, q, Time.deltaTime * speed);

            //var q = Quaternion.LookRotation(destinationPoint.transform.position - rocket.transform.position);
            //rocket.transform.rotation = Quaternion.RotateTowards(rocket.transform.rotation, q, turnSpeed * Time.deltaTime);

            float speedBust = 1;// direction.magnitude;
            direction.Normalize();


            controller.Move(direction * (speed + speedBust) * Time.deltaTime);
        }

    }

    void MoveToClick()
    {

        

        Vector3 destination;

        destination = new Vector3 (pointerPosition.x, pointerPosition.y, 0);
        destination = Camera.main.ScreenToWorldPoint(destination);
        destination.z = 0;

        destinationPoint.transform.position = destination;

        //rocket.transform.LookAt(destinationPoint.transform);

        //rocket.transform.position = destination;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
