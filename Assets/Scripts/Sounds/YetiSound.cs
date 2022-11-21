using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playGrowl()
    {
        audioSource.PlayOneShot(Sounds.Yeti);
    }

    public void playJump()
    {
        audioSource.PlayOneShot(Sounds.yeti_jump);
    }

    public void playPunch()
    {
        audioSource.PlayOneShot(Sounds.yeti_punch);
    }

    public void playThrow()
    {
        audioSource.PlayOneShot(Sounds.yeti_throw);
    }
}
