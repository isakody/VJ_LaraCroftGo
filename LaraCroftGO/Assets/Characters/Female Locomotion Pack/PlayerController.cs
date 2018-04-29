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
    bool climb;
    bool attachedToWall = false;
    public float distanceTowall = 0.02f;
    RaycastHit hit;
    bool isLerping = false;
    Vector3 startPos, targetPos;

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
        CheckClimb();
        if (climb)
        {
            updateClimb();
        }
        else
        {
            Rotate();
            Move();
        }
        
    }
    private void CheckClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 0.5f;
        Vector3 dir = transform.forward;
        
        if (Physics.Raycast(origin, dir, out hit ,1))
        {
            climb = true;
        }
        else
        {
            climb = false;
        }

    }

    private void Move()
    {
        Vector3 moveVector = new Vector3(input.x * movmentSpeed, verticalVelocity, input.y * movmentSpeed);
        controller.Move(moveVector * Time.deltaTime);
        if (controller.isGrounded)
        {
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
        if (!climb)
        {
            anim.SetFloat("BlendX", input.x);
            anim.SetFloat("BlendY", input.y);
        }
        

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

    void updateClimb()
    {
        if (attachedToWall)
        {
            moveUp();
        }

        else
        {
            moveToWall();
        }
    }

    void moveToWall()
    {
        
        if (!isLerping)
        {
            anim.SetTrigger("climbIdle");
            targetPos = hit.point + (hit.normal * distanceTowall);
            isLerping = true;
        }
        else
        {
            Vector3 offset = targetPos - transform.position;
            if (offset.magnitude <= 0.1f)
            {
                isLerping = false;
                attachedToWall = true;
                return;
            }

            else
            {
                offset = offset.normalized * 5.0f;
                controller.Move(offset * Time.deltaTime);
            }
        }
      
       
    }

    void moveUp()
    {
        anim.SetTrigger("climbUp");
        anim.ResetTrigger("climbIdle");
        controller.Move(new Vector3(0, 0.2f, 0));
    }
}
