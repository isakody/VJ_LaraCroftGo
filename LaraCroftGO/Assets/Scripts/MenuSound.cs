using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSound : MonoBehaviour {
    public AudioSource sound;
    public AudioClip audio;
    public Slider volumeSlider;
    public void OnClick()
    {
        sound.PlayOneShot(audio);
    }

}

