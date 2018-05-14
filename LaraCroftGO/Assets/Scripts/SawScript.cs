using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour {
    public Vector3 direction;
    public float moveSpeed;
    bool isMoving = false;
    bool canMove = false;
    public int distanceToMove = 0;
    Vector3 finalPosition;
    Vector3 targetPosition;
    public Vector3 axisOfRotation;
    public float rotationSpeed = 500;
    float t = 0;

	// Use this for initialization
	void Start () {
        
        finalPosition = transform.position + direction * distanceToMove;
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axisOfRotation * Time.deltaTime * rotationSpeed);
        if (canMove)
            Move();		
	}

    void Move()
    {
        if (!isMoving)
        {
            t = 0;
            isMoving = true;
            calculateTargetPosition();
        }
        else
        {
            if (transform.position != targetPosition)
            {
                t += Time.deltaTime / moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            }
            else
            {
                canMove = false;
                isMoving = false;
            }
        }
    }

    void calculateTargetPosition()
    {
        if(transform.position == finalPosition)
        {
            finalPosition = transform.position - direction * distanceToMove;
            direction = -direction;
        }
        Vector3 vDirecction = finalPosition - transform.position;
        targetPosition = transform.position + Vector3.Normalize(vDirecction);
    }

    void EnableMovement(bool canMove)
    {
        if(!this.canMove) this.canMove = canMove;
    }
    void OnCollisionEnter(Collision objectColiding)
    {
        if( objectColiding.gameObject.tag == "Lara" || objectColiding.gameObject.tag == "Enemie")
            Destroy(objectColiding.gameObject);
    }
}
