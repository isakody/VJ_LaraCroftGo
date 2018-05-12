using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    bool isMoving = false;
    float t;
    public float attackSeconds = 0.5f;
    GameObject objectToDestroy;
    bool isKilling = false;
    Vector3 targetPosition;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        if (!isMoving)
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.forward, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up*0.5f, -transform.forward, out hit, 1.0f))
            {
                if (hit.collider.tag == "Lara")
                {
                    targetPosition = transform.position;
                    targetPosition -= transform.forward;
                    objectToDestroy = hit.collider.gameObject;
                    isKilling = true;
                    t = 0;
                    isMoving = true;
                    
                }
            }
        }

        else
        {
            
            if (targetPosition == transform.position)
            {
                Debug.Log("final Postition");
                isMoving = false;

            }
            else
            {
                t += Time.deltaTime;
                if (isKilling)
                {
                    anim.SetTrigger("isKilling");
                    if(t >= 0.3f)
                    {
                        t = 0;
                        anim.ResetTrigger("isKilling");
                        isKilling = false;
                        Destroy(objectToDestroy);
                    }
                }
                else
                {
                    t += Time.deltaTime / attackSeconds;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, t);
                }
               

            }
        }
        
    }
}
