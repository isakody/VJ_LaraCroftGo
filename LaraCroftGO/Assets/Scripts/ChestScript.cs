using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {
    Animator anim;
    float t = 0;
    bool isOpened;
    public GameObject deathEffect;
    public GameObject sceneController;
    public GameObject audioSource;

	// Use this for initialization
	void Start () {
        anim = transform.GetChild(0).GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpened)
        {
            t += Time.deltaTime;
            if(t > 4.0f)
            {
                die();
            }
        }
	}
    void OnMouseDown()
    {
        if (!isOpened)
        {
            anim.SetTrigger("isOpened");
            audioSource.SendMessage("ChestOpen");
            isOpened = true;
            sceneController.SendMessage("gemFound");
            t = 0;
        }
    }
    void die()
    {

        Destroy(Instantiate(deathEffect, transform.position + Vector3.up * 0.25f, Quaternion.identity) as GameObject, 5.0f);
        this.gameObject.SetActive(false);
        
    }
}
