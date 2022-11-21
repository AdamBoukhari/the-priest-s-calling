using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void Fall()
    {
        audioSource.PlayOneShot(Sounds.hitSnd);
        rb.gravityScale = 1;
    }
}
