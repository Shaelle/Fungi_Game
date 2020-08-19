using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimberMovement : MonoBehaviour
{
    // TODO: Require component
    CharacterController controller;
    [SerializeField] float speed = 5f;

    [SerializeField] Transform groundCheck = null;
    [SerializeField] float gravity = -9.8f;

    [SerializeField] float groundDistance = 0.1f;
    [SerializeField] LayerMask groundMask = 0;

    Vector3 velocity;
    bool isGrounded;

    public bool isMoving = true;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isMoving)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded) // gravitation
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                controller.Move(Vector3.forward * speed * Time.deltaTime);
            }

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
        }


        
        
    }
}
