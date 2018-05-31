using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLaraToableFloorMovment : MonoBehaviour {
    
	// Use this for initialization
	

    // Update is called once per frame
    void OnTriggerEnter(Collider objectColiding)
    {
        Debug.Log("entering Colision");
        transform.parent.parent.gameObject.GetComponent<ObjectToMoveTilesController>().canMoveFloor(true);
    }
    void OnTriggerExit(Collider objectColiding)
    {
        transform.parent.parent.gameObject.GetComponent<ObjectToMoveTilesController>().canMoveFloor(false);
    }
}
