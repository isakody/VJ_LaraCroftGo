using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour {
    public List<GameObject> objectsToActivate;
    public bool activated = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}

    void OnTriggerEnter(Collider objectColiding)
    {
        activated = true;
        ActivateGameObjects();
    }

    void OnTriggerExit(Collider objectColiding)
    {
        activated = false;
        DeactivateGameObjects();
    }

    void ActivateGameObjects()
    {
        for(int i = 0; i < objectsToActivate.Count; ++i){
            objectsToActivate[i].SendMessage("activation", true);
        }
    }

    void DeactivateGameObjects()
    {
        for (int i = 0; i < objectsToActivate.Count; ++i)
        {
            objectsToActivate[i].SendMessage("activation", false);
        }
    }
}
