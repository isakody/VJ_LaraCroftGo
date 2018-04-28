using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float velocity = 5;
    public float turnSpeed = 10;
    Vector2 input;
    float angle;
    Quaternion targetRotation;
    Transform cam;
    Animator anim;
    public float height = 0.55f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;
    public bool debug;
    float groundAngle;
    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;
    void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        ClaculateGroundAngle();
        CheckGround();
        AplyGravity();
        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;
        Rotate();
        Move();
        if (Input.GetKeyDown("space"))
            anim.SetTrigger("space");
        else anim.ResetTrigger("space");
    }

    private void AplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position,-Vector3.up,out hitInfo, height + heightPadding, ground))
        {
            
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void ClaculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
            return;
        }
        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }
        forward = Vector3.Cross(hitInfo.normal, -transform.right);
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
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed * Time.deltaTime);
    }

    void Move()
    {
        if (groundAngle >= maxGroundAngle) return;
        transform.position += forward * velocity * Time.deltaTime;
    }


}
