using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsScript : MonoBehaviour {
    public AudioClip kickSound;
    public AudioClip swordSwing;
    public AudioClip leverPullSound;
    public AudioClip chestOpenSound;
    AudioSource audioSource;
    public AudioClip detectSound;
    public AudioClip snakeKill;
    public AudioClip spiderKill;
    public AudioClip skeletonKill;
	void Start () {
        this.audioSource = gameObject.GetComponent<AudioSource>();
	}


    public void PlayKickSound()
    {
        audioSource.PlayOneShot(kickSound,2);
    }

    public void PlaySwordSwing()
    {
        audioSource.PlayOneShot(swordSwing,2);
    }

    public void LeverPull()
    {
        audioSource.PlayOneShot(leverPullSound,2);
    }

    public void ChestOpen()
    {
        audioSource.PlayOneShot(chestOpenSound,3);
    }

    public void PlayDetect()
    {
        audioSource.PlayOneShot(detectSound,2);
    }

    public void PlaySnake()
    {
        audioSource.PlayOneShot(snakeKill, 2);
    }

    public void PlaySpider()
    {
        audioSource.PlayOneShot(spiderKill, 2);
    }
    public void PlaySkeleton()
    {
        audioSource.PlayOneShot(skeletonKill, 5);
    }
	
	
}
