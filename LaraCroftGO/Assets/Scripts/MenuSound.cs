using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour {
    public AudioSource audio;
	// Use this for initialization
	void Start () {

        audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
