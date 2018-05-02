using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardController : MonoBehaviour {
    List<Vector3> directions;
    public bool canGoDown;
    public bool canGoUp;
    public bool canGoNorth;
    public bool canGoSouth;
    public bool canGoEast;
    public bool canGoWest;
    public bool canMove;
    public bool followingLara = false;
    public bool isMooving = false;
    public float attackSeconds = 0.2f;
    float t;
    Vector3 targetPosition;
    // Use this for initialization
    void Start () {
        directions = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove && followingLara)
        {
            CheckForKill();
            if (!isMooving)
            {
                CalculatePosition();
                t = 0;
                isMooving = true;
            }
            else
            {
                MoveToTargetPosition();
            }
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

    void CheckForKill()
    {

        RaycastHit hit;
        if (followingLara)
        {
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
       
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3.0f))
        {
            if (hit.collider.tag == "Lara")
            {
                directions.Add(transform.forward);
                directions.Add(transform.forward);
                followingLara = true;
            }
        }
    }

    void EnableMovement(bool canMove)
    {
        this.canMove = canMove;
    }

    void ParseLaraDirections(Vector3 direction)
    {
        if(followingLara)
            directions.Add(direction);
    }

    void MoveToTargetPosition()
    {
        if (targetPosition == transform.position)
        {
            Debug.Log("final Postition");
            canMove = false;
            isMooving = false;
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            Debug.Log(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        }
        else
        {
            t += Time.deltaTime / attackSeconds;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        }
    }

    void CalculatePosition()
    {
        if (directions.Count > 0)
        {
            Vector3 movment = directions[0];
            directions.RemoveAt(0);
            targetPosition = transform.position += movment;
            
        }
        else targetPosition = transform.position;
        
    }
}
