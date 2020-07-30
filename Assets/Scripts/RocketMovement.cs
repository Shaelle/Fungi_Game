using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketMovement : MonoBehaviour
{

    [SerializeField] GameObject rocket;
    GameObject innerRocket;
    GameObject exhaust;


    [SerializeField] GameObject destinationPoint;
    [SerializeField] float speed = 3f;
    [SerializeField] float turnSpeed = 3f;

    [SerializeField] float startPause = 1f;
    bool isStarting = true;


    bool constantMovement = true;
    bool inFlight = true;
    bool waypointReached = false;

    PlayerInput controls;

    CharacterController controller;

    private Vector2 pointerPosition;

    float moveThreshold = 0.1f;
    const float landingRadius = 1f;

    const float doubleTapThreshhold = 0.3f;


    bool isDoubleTap = false;

    bool isBooosted = false;

    [SerializeField] float boostTime = 1f;
    [SerializeField] float boostMagnitude = 2f;

    bool isWin = false;


    [SerializeField] GameObject moveButton;
    Image moveImage;



    private void Awake()
    {

        controls = new PlayerInput();
        controls.Main.Click.performed += ctx => MoveToClick();
        controls.Main.PointerPosition.performed += ctx => UpdateCursorPos(ctx.ReadValue<Vector2>());

        moveImage = moveButton.GetComponent<Image>();

        constantMovement = PlayerPrefs.GetInt("ConstMove", 1) == 1 ? true : false;
        SetColour(constantMovement, moveImage);


        controller = GetComponent<CharacterController>();
        if (controller == null) { Debug.Log("Character controller not found!"); }

        innerRocket = rocket.transform.GetChild(0).gameObject;
        exhaust = rocket.transform.GetChild(1).gameObject;

        StartCoroutine(StartPause());
       
    }


    IEnumerator StartPause()
    {
        yield return new WaitForSeconds(startPause);

        isStarting = false;
    }


    // Update is called once per frame
    void Update()
    {


        float speedBust = 1;

        if (isBooosted) { speedBust = boostMagnitude; }

        Vector3 direction = new Vector3(destinationPoint.transform.position.x - rocket.transform.position.x, destinationPoint.transform.position.y - rocket.transform.position.y, 0f);

        if (!isStarting)
        {
            if (!constantMovement) { waypointReached = false; }
            //waypointReached = false;

            RotateShip();

            if ((direction.magnitude > moveThreshold) && !inFlight)
            {
                direction.Normalize();

                controller.Move(direction * (speed + speedBust) * Time.deltaTime);
            }

            if (inFlight)
            {
                Vector3 moveDirection = transform.TransformDirection(Vector3.right) * speed;

                controller.Move(moveDirection * speedBust * Time.deltaTime);
            }
        }

    }

    public void CutEngine()
    {
        exhaust.SetActive(false);
    }


    public void WinRound(Transform target)
    {
        isWin = true;

        speed = speed / 2;

        moveThreshold = landingRadius;

        destinationPoint.transform.position = target.position;

        //constantMovement = false;
        inFlight = false;

        exhaust.SetActive(false);

        Animator animation;
        animation = innerRocket.GetComponent<Animator>();

        if (animation != null)
        {
            animation.Play("RocketTurn");
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



    void UpdateCursorPos(Vector2 cursorPos)
    {
        pointerPosition = cursorPos;
    }



    void MoveToClick()
    {

        SetDestination();

        if (inFlight)
         {
        waypointReached = false;
        }

        if (!isDoubleTap) { StartCoroutine(DoubleTab()); }
        else { StartCoroutine(BoostSpeed()); }

    }


    void SetDestination()
    {
        if (!isWin)
        {
            Vector3 destination;

            destination = new Vector3(pointerPosition.x, pointerPosition.y, 0);
            destination = Camera.main.ScreenToWorldPoint(destination);
            destination.z = 0;

            destinationPoint.transform.position = destination;
        }
    }



    void RotateShip()
    {

        float speedBust = 1;

        if (isBooosted) { speedBust = boostMagnitude; }

        if (!waypointReached)
        {
            Vector3 vectorToTarget = destinationPoint.transform.position - rocket.transform.position;
            waypointReached = vectorToTarget.magnitude < 0.5f;

            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            rocket.transform.rotation = Quaternion.Slerp(rocket.transform.rotation, q, Time.deltaTime * speed * speedBust);
        }
    }



    IEnumerator DoubleTab()
    {
        isDoubleTap = true;

        yield return new WaitForSeconds(doubleTapThreshhold);

        isDoubleTap = false;
    }

    IEnumerator BoostSpeed()
    {
        isBooosted = true;

        yield return new WaitForSeconds(boostTime);

        isBooosted = false;
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
