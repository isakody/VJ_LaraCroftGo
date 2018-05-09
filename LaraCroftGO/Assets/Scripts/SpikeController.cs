using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {
    public Vector3 moveDirection;
    public float distanceToMove;
    bool activated = false;
    bool isMooving = false;
    Vector3 targetPosition;
    public float attackSeconds;
    float t = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime*40);
        if (isMooving) MoveToTargetPosition();
	}
    void activation(bool activated)
    {
        if(this.activated != activated && !isMooving)
        {
            this.activated = activated;
            calculatePosition();
        }
    }

    void calculatePosition()
    {
        if (activated)
        {
            targetPosition = transform.position;
            targetPosition += moveDirection * distanceToMove;
            isMooving = true;
            t = 0;
        }
        else
        {
            targetPosition = transform.position;
            targetPosition += moveDirection * distanceToMove *- 1;
            isMooving = true;
            t = 0;
        }
    }

    void MoveToTargetPosition()
    {
        if (targetPosition == transform.position)
        {
            isMooving = false;
        }
        else
        {
            t += Time.deltaTime / attackSeconds;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        }
    }

    void OnCollisionEnter(Collision objectColiding)
    {
        Debug.Log("destroying");
        Destroy(objectColiding.gameObject);
    }
}
