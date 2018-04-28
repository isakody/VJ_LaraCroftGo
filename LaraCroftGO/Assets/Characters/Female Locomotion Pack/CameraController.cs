using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5;
    public float turnSpeed = 5;
    public float smoothSpeed = 0.5f;

    Quaternion targetRotation;
    Vector3 targetPos;

    
    bool smoothRotating = false;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        MoveWithTarget();
        LookAtTarget();
        if (Input.GetKeyDown(KeyCode.G) && !smoothRotating)
        {
            StartCoroutine("RotateArowndTarget", 45);
        }
        if (Input.GetKeyDown(KeyCode.H) && !smoothRotating)
        {
            StartCoroutine("RotateArowndTarget", -45);
        }
    }

    void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    IEnumerator RotateArowndTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        float distance = Vector3.Distance(offsetPos, targetOffsetPos);
        smoothRotating = true;
        while(distance > 0.02f)
        {

            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            distance = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }
        smoothRotating = false;
        offsetPos = targetOffsetPos;
    }
}
