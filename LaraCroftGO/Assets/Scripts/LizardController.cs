using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardController : MonoBehaviour {
    public List<string> directions;
    public bool canGoDown;
    public bool canGoUp;
    public bool canGoNorth;
    public bool canGoSouth;
    public bool canGoEast;
    public bool canGoWest;
    public bool canMove;
    public bool canGoNorthUp;
    public bool canGoWestUp;
    public bool canGoEastDown;
    public bool canGoSouthDown;
    public bool followingLara = false;
    public bool hasDetectedLara = false;
    public bool isMooving = false;
    public float attackSeconds = 0.2f;
    bool isClimbingX;
    bool isClimbingZ;
    public float boxSize = 0.25f;
    float t;
    Animator anim;
    Vector3 targetPosition;
    // Use this for initialization
    void Start () {
        directions = new List<string>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(transform.position+Vector3.up*0.5f, transform.forward * 3, Color.red);
        if (canMove)
        {
            CheckForKill();
            if (hasDetectedLara) followingLara = true;
            if (followingLara)
            {
                if (!isMooving)
                {
                    CalculatePosition();
                    t = 0;
                    isMooving = true;
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                }
                else
                {
                    anim.SetTrigger("isRunning");
                    MoveToTargetPosition();
                }
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
        canGoNorthUp = objectColiding.gameObject.GetComponent<FloorTile>().canGoNorthUp;
        canGoWestUp = objectColiding.gameObject.GetComponent<FloorTile>().canGoWestUp;
        canGoSouthDown = objectColiding.gameObject.GetComponent<FloorTile>().canGoSouthDown;
        canGoEastDown = objectColiding.gameObject.GetComponent<FloorTile>().canGoEastDown;
    }

    void CheckForKill()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.tag == "Lara")
            {
                targetPosition = transform.position;
                targetPosition += transform.forward;
                Destroy(hit.collider.gameObject);
            }
        }
        
        if (!hasDetectedLara)
        {

            if (Physics.Raycast(transform.position+Vector3.up*0.5f, transform.forward, out hit, 3.0f))
            {
                if (hit.collider.tag == "Lara")
                {
                    if (isClimbingX && hit.collider.GetComponent<LaraController>().isClimbingX ||
                        isClimbingZ && hit.collider.GetComponent<LaraController>().isClimbingZ) return;
                    directions.Add("Forward");
                    directions.Add("Forward");
                    directions.Add("Forward");
                    hasDetectedLara = true;
                    canMove = false;
                }
            }
        }
    }

    void EnableMovement(bool canMove)
    {
        this.canMove = canMove;
    }

    void ParseLaraDirections(string direction)
    {
        if(followingLara)
            directions.Add(direction);
    }

    void MoveToTargetPosition()
    {
        if (Mathf.Abs(Vector3.Distance(targetPosition, transform.position)) <= 0.01f)
        {
            Debug.Log("final Postition");
            canMove = false;
            isMooving = false;
            anim.ResetTrigger("isRunning");
            Debug.Log(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        }
        else
        {
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            t += Time.deltaTime / attackSeconds;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        }
    }

    void CalculatePosition()
    {
        if (directions.Count > 0)
        {
            if (directions[0] == "Forward")
            {
                targetPosition = transform.position + transform.forward;
            }
            else if (directions[0] == "North" && canGoNorth)
            {
                targetPosition = transform.position + Vector3.forward;
            }
            else if (directions[0] == "South" && canGoSouth)
            {
                targetPosition = transform.position + Vector3.back;
            }
            else if (directions[0] == "East" && canGoEast)
            {
                targetPosition = transform.position + Vector3.right;
            }
            else if (directions[0] == "West" && canGoWest)
            {
                targetPosition = transform.position + Vector3.left;
            }
            else if (directions[0] == "NorthUp" && canGoNorthUp)
            {
                if (isClimbingZ)
                {
                    //adRotation and movment depending on the character box
                    targetPosition = transform.position;
                    targetPosition.y += 1;
                    targetPosition.z += 0.5f + (boxSize / 2);
                    isClimbingZ = false;

                }
                else
                {
                    //adRotation and movment depending on the character box
                    targetPosition = transform.position;
                    targetPosition.z += 0.5f - (boxSize / 2);
                    isClimbingZ = true;
                }
            }
            else if (directions[0] == "EastDown" && canGoEastDown)
            {
                if (isClimbingX)
                {
                    targetPosition = transform.position;
                    
                    targetPosition.x += 0.5f - (boxSize / 2);
                    isClimbingX = false;
                }
                else
                {
                    targetPosition = transform.position;
                    targetPosition.y -= 1;
                    targetPosition.z += 0.5f + (boxSize / 2);
                    isClimbingX = true;
                }

            }
            else if (directions[0] == "WestUp" && canGoWestUp)
            {
                if (isClimbingX)
                {
                    targetPosition = transform.position;
                    targetPosition.y += 1;
                    targetPosition.x -= 0.5f + (boxSize / 2);
                    isClimbingX = false;
                }
                else
                {
                    targetPosition = transform.position;
                    targetPosition.x -= 0.5f - (boxSize / 2);
                    isClimbingX = true;
                }
            }
            else if (directions[0] == "SouthDown" && canGoSouthDown)
            {
                if (isClimbingZ)
                {
                    targetPosition = transform.position;
                    targetPosition.z -= 0.5f - (boxSize / 2);
                    isClimbingZ = false;
                }
                else
                {
                    targetPosition = transform.position;
                    targetPosition.y -= 1;
                    targetPosition.z -= 0.5f + (boxSize / 2);
                    isClimbingZ = true;
                }

            }
            else
            {
                followingLara = false;
                hasDetectedLara = false;
                directions.Clear();
                return;
            }
            directions.RemoveAt(0);
        }
        else followingLara = false;
        
    }
}
