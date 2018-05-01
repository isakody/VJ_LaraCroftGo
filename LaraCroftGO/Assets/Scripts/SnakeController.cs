using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    bool isMoving = false;
    float t;
    public float attackSeconds = 0.5f;
    Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward);
        if (!isMoving)
        {
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
            {
                if (hit.collider.tag == "Lara")
                {
                    targetPosition = transform.position;
                    targetPosition += transform.forward;
                    Destroy(hit.collider.gameObject);
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
                t += Time.deltaTime / attackSeconds;
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            }
        }
        
    }
}
