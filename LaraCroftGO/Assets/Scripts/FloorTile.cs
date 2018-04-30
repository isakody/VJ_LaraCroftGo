using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    public bool canGoNorth = false;
    public bool canGoSouth = false;
    public bool canGoEast = false;
    public bool canGoWest = false;
    public bool canGoWestUp = false;
    public bool canGoEastDown = false;
    public bool canGoNorthUp = false;
    public bool canGoSouthDown = false;
    public bool canGoUp = false;
    public bool canGoDown = false;
    void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider objectColiding)
    {
        objectColiding.gameObject.GetComponent<LaraController>().parseInfoFloor(canGoNorth,canGoSouth, canGoEast,canGoWest);
        objectColiding.gameObject.GetComponent<LaraController>().parseWallDirections(canGoNorthUp, canGoSouthDown, canGoEastDown, canGoWestUp, canGoUp,canGoDown);
    }

}
