using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class YetiShield : MonoBehaviour
{
    private ParticleSystem part;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        part = GetComponent<ParticleSystem>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        part.Stop();
        circleCollider.enabled = false;
    }

    public void Activate()
    {
        part.Play();
        circleCollider.enabled = true;
    }

    public void Deactivate()
    {
        part.Stop();
        circleCollider.enabled = false;
    }
}
