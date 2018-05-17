using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : MonoBehaviour {
    bool canMove = false;
    public Vector3 axisOfRotation;
    int actualPosition = 2;
    bool goingForward = true;
    public float movementTime = 0.2f;
    

	// Use this for initialization
	void Start () {
        
        
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();

    }
    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
    }

    private void Move()
    {
        if (canMove)
        {
            if (goingForward)
            {
                if (actualPosition == 4)
                {
                    StartCoroutine(RotateMe(-axisOfRotation * 90, movementTime));
                    actualPosition = 3;
                    goingForward = false;
                }
                else
                {
                    StartCoroutine(RotateMe(axisOfRotation * 90, movementTime));
                    actualPosition++;
                }
            }
            else
            {
                if (actualPosition == 0)
                {
                    StartCoroutine(RotateMe(axisOfRotation * 90, movementTime));
                    actualPosition = 1;
                    goingForward = true;

                }
                else
                {
                    StartCoroutine(RotateMe(-axisOfRotation * 90, movementTime));
                    actualPosition--;

                }
            }
            canMove = false;
        }
    }

    void EnableMovement(bool canMove)
    {
        this.canMove = canMove;
    }

    void OnCollisionEnter(Collision objectColiding)
    {
        if (objectColiding.gameObject.tag == "Lara" || objectColiding.gameObject.tag == "Enemie")
            Destroy(objectColiding.gameObject);
    }

}
