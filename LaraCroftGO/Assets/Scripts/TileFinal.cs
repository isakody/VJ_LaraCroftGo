using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFinal : MonoBehaviour {
    void OnTriggerEnter(Collider objectColiding)
    {
        if (objectColiding.gameObject.tag == "Lara")
        {
            objectColiding.gameObject.GetComponent<LaraController>().SendMessage("hasWon", true);
        }
    }
}
