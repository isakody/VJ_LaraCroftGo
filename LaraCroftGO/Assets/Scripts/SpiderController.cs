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
    bool isKilling = false;
    public float attackSeconds = 0.2f;
    float t;
    GameObject objectToDestroy;
    Vector3 targetPosition;
    Animator anim;
    public GameObject deathEffect;
    bool isDead = false;
    public GameObject audioSource;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isDead)
        {
            t += Time.deltaTime;
            if (t > 2)
            {
                Destroy(this.gameObject);
            }
            return;
        }
        if (isKilling)
        {
            anim.SetTrigger("isKilling");
            t += Time.deltaTime;
            if (t > attackSeconds)
            {
                anim.ResetTrigger("isKilling");
                MoveToTargetPosition();
                isKilling = false;
                
            }
            
        }
        else
        {


            CheckForKill();
            if (canMove)
            {
                if (!isMooving)
                {
                    anim.SetTrigger("isMoving");
                    CalculatePosition();
                    isMooving = true;
                    t = 0;
                }
                else MoveToTargetPosition();
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
    void OnCollisionEnter(Collision objectColiding)
    {
        Debug.Log("destroying");
        if (objectColiding.gameObject.tag == "Lara")
        {
            objectColiding.gameObject.SendMessage("die");
        }
        else if(objectColiding.gameObject.tag == "Enemy")
        {
            objectColiding.gameObject.SendMessage("die");
        }
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
            anim.ResetTrigger("isMoving");
            anim.ResetTrigger("isKilling");
            t = 0;

        }
        else
        {
            t += Time.deltaTime / 0.2f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        }
    }

    void CheckForKill()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up*0.25f, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.tag == "Lara")
            {
                audioSource.SendMessage("PlaySpider");
                targetPosition = transform.position;
                targetPosition += transform.forward;
                hit.collider.gameObject.SendMessage("die");
                isKilling = true;
                t = 0;
            }
        }
    }

    void EnableMovement(bool canMove)
    {
        this.canMove = canMove;
    }

    void die()
    {

        Destroy(Instantiate(deathEffect, transform.position + Vector3.up * 0.25f, Quaternion.identity) as GameObject, 5.0f);
        this.gameObject.SetActive(false);
        t = 0;
        isDead = true;

    }
}
