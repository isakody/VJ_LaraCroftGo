using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CharacterController controller;
    public float verticalVelocity;
    private float gravity = Physics.gravity.y;
    public float jumpForce = 1.0f;
    public float movmentSpeed = 5.0f;
    Vector2 input;
    Animator anim;
    float angle;
    Quaternion targetRotation;
    Transform cam;
    public float turnSpeed = 10.0f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        GetInput();
        CalculateDirection();
       
        Rotate();
        Move();
    }

    private void Move()
    {
        if (controller.isGrounded)
        {
            Vector3 moveVector = new Vector3(input.x * movmentSpeed, verticalVelocity, input.y * movmentSpeed);
            controller.Move(moveVector * Time.deltaTime);
            verticalVelocity = gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                Debug.Log("jump");
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown("space"))
            anim.SetTrigger("space");
        else anim.ResetTrigger("space");
    }

    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        anim.SetFloat("BlendX", input.x);
        anim.SetFloat("BlendY", input.y);

    }
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }



}
