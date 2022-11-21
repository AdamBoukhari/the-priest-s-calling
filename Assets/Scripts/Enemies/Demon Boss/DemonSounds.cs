using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSounds : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(Sounds.destroy);
    }

    public void Cleave()
    {
        audioSource.PlayOneShot(Sounds.hitSnd);
    }

    public void Fire()
    {
        audioSource.PlayOneShot(Sounds.platform);
    }

    public void FireThrow()
    {
        audioSource.PlayOneShot(Sounds.fireball);
    }

    public void Dead()
    {
        audioSource.PlayOneShot(Sounds.deathSnd);
    }

}
