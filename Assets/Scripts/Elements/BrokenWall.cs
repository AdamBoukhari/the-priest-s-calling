using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour
{
    [SerializeField] public int id;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.gameObject.transform.parent = null;
    }

    public void destroy()
    {
        audioSource.PlayOneShot(Sounds.destroy);
        gameObject.SetActive(false);
    }
}
