using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketMovement : MonoBehaviour
{

    public GameObject rocket;
    public GameObject destinationPoint;
    public float speed = 3f;
    public float turnSpeed = 3f;
    public bool constantMovement = true;
    public bool holdPosition = false;


    public GameObject moveButton;
    Image moveImage;

    public GameObject holdButton;
    Image holdImage;




    bool waypointReached = false;

    PlayerInput controls;

    CharacterController controller;

    private Vector2 pointerPosition;

    const float moveThreshold = 0.1f;

    private void Awake()
    {
        holdImage = holdButton.GetComponent<Image>();
        moveImage = moveButton.GetComponent<Image>();

        constantMovement = PlayerPrefs.GetInt("ConstMove", 1) == 1 ? true : false;
        SetColour(constantMovement, moveImage);

        holdPosition = PlayerPrefs.GetInt("HoldPos", 0) == 1 ? true : false;
        SetColour(holdPosition, holdImage);

        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => MoveToClick();
        controls.Main.PointerPosition.performed += ctx => UpdateCursorPos(ctx.ReadValue<Vector2>());


        controller = GetComponent<CharacterController>();
        if (controller == null) { Debug.Log("Character controller not found!"); }

    }


    // Update is called once per frame
    void Update()
    {

        Vector3 direction = new Vector3(destinationPoint.transform.position.x - rocket.transform.position.x, destinationPoint.transform.position.y - rocket.transform.position.y, 0f);

        if (!constantMovement || holdPosition) { waypointReached = false; }

        RotateShip();

        if ((direction.magnitude > moveThreshold) && !constantMovement)
        {

           // RotateShip();

            float speedBust = 1;// direction.magnitude;
            direction.Normalize();

            controller.Move(direction * (speed + speedBust) * Time.deltaTime);
          
        }

        if (constantMovement)
        {
            Vector3 moveDirection = transform.TransformDirection(Vector3.right) * speed;
            controller.Move(moveDirection * Time.deltaTime);
        }

    }


    void SetColour(bool condition, Image pic)
    {
        if (condition) { pic.color = Color.green; }
        else { pic.color = Color.white; }
    }


    public void ToggleMovement()
    {
        constantMovement = !constantMovement;

        PlayerPrefs.SetInt("ConstMove", constantMovement ? 1 : 0);

        SetColour(constantMovement, moveImage);

    }



    public void ToggleHold()
    {
        holdPosition = !holdPosition;

        PlayerPrefs.SetInt("HoldPos", holdPosition ? 1 : 0);

        SetColour(holdPosition, holdImage);
    }


    void UpdateCursorPos(Vector2 cursorPos)
    {
        pointerPosition = cursorPos;


        //if (constantMovement) { SetDestination(); }
    }

    void MoveToClick()
    {
        
        SetDestination();

        if (constantMovement)
        {
            waypointReached = false;
        }

    }


    void SetDestination()
    {
        Vector3 destination;

        destination = new Vector3(pointerPosition.x, pointerPosition.y, 0);
        destination = Camera.main.ScreenToWorldPoint(destination);
        destination.z = 0;

        destinationPoint.transform.position = destination;
    }

    void RotateShip()
    {
        if (!waypointReached)
        {
            Vector3 vectorToTarget = destinationPoint.transform.position - rocket.transform.position;
            waypointReached = vectorToTarget.magnitude < 0.5f;


            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            rocket.transform.rotation = Quaternion.Slerp(rocket.transform.rotation, q, Time.deltaTime * speed);
        }


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
