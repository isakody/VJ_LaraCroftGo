using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour {
    public AudioSource sound;
    public AudioClip audio;

    public void OnClick()
    {
        sound.PlayOneShot(audio);
    }
}

