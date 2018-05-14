using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController1 : MonoBehaviour {
    public GameObject lara;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
        if(lara != null)
        {
            if (lara.gameObject.GetComponent<LaraController>().hasWon1)
            {
                timer += Time.deltaTime;
                if(timer > 5.0f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }

        }
        else
        {
            timer += Time.deltaTime;
            if(timer > 2.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

           
        }
		
	}
}
