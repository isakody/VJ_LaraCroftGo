using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour {
    public bool canGoDown;
    public bool canGoUp;
    public bool canGoNorth;
    public bool canGoSouth;
    public bool canGoEast;
    public bool canGoWest;
    public bool canMove;
    bool isMooving = false;
    public float attackSeconds = 0.2f;
    float t;
    Vector3 targetPosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove)
        {
            CheckForKill();
            if (!isMooving)
            {
                CalculatePosition();
                isMooving = true;
                t = 0;
            }
            else MoveToTargetPosition();
        }
	}

    void OnTriggerEnter(Collider objectColiding)
    {
        canGoDown = objectColiding.gameObject.GetComponent<FloorTile>().canGoDown;
        canGoUp = objectColiding.gameObject.GetComponent<FloorTile>().canGoUp;
        canGoNorth = objectColiding.gameObject.GetComponent<FloorTile>().canGoNorth;
        canGoSouth = objectColiding.gameObject.GetComponent<FloorTile>().canGoSouth;
        canGoEast = objectColiding.gameObject.GetComponent<FloorTile>().canGoEast;
        canGoWest = objectColiding.gameObject.GetComponent<FloorTile>().canGoWest;
    }

    void CalculatePosition()
    {
        if(transform.forward == Vector3.forward)
        {
            if (canGoNorth)
            {
                targetPosition = transform.position + Vector3.forward;
            }
            else
            {
                transform.Rotate(0, 180, 0);
                CalculatePosition();

            }

        }
        else if (transform.forward == Vector3.back)
        {
            if (canGoSouth)
            {
                targetPosition = transform.position + Vector3.back;
            }
            else
            {
                transform.Rotate(0, 180, 0);
                CalculatePosition();

            }

        }
        else if (transform.forward == Vector3.right)
        {
            if (canGoEast)
            {
                targetPosition = transform.position + Vector3.right;
            }
            else
            {
                transform.Rotate(0, 180, 0);
                CalculatePosition(); ;

            }

        }
        else if (transform.forward == Vector3.left)
        {
            if (canGoWest)
            {
                targetPosition = transform.position + Vector3.left;
            }
            else
            {
                transform.Rotate(0, 180, 0);
                CalculatePosition();

            }

        }

    }

    void MoveToTargetPosition()
    {
        if (targetPosition == transform.position)
        {
            Debug.Log("final Postition");
            canMove = false;
            isMooving = false;

        }
        else
        {
            t += Time.deltaTime / attackSeconds;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        }
    }

    void CheckForKill()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.tag == "Lara")
            {
                targetPosition = transform.position;
                targetPosition += transform.forward;
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void EnableMovement(bool canMove)
    {
        this.canMove = canMove;
    }
}
