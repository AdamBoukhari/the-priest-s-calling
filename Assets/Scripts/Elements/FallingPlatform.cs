using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// A platform that falls a couple of seconds after the player has touched it.
/// This script needs a children with a sprite renderer of the platform.
/// </summary>
[RequireComponent (typeof (BoxCollider2D))]
public class FallingPlatform : MonoBehaviour
{
    private const float SHAKING_VALUE = 0.10f;
    private const float SHAKING_DURATION = 1.5f;
    private const float RESPAWN_DURATION = 2.5f;

    private BoxCollider2D boxCollider;
    private Transform render;
    private AudioSource audioSource;
    private Light2D light2d;

    private Vector3 originalPos;
    private bool hasTouchedPlayer = false;
    private float shakeTimer = 0;
    private float respawnTimer = 0;

    void Awake()
    {
        render = gameObject.transform.GetChild(0);
        boxCollider = GetComponent<BoxCollider2D>();
        originalPos = render.position;
        audioSource = GetComponent<AudioSource>();
        light2d = GetComponent<Light2D>();
    }

    void Update()
    {
        HandleDestruction();
        HandleRespawn();
    }

    private void HandleDestruction()
    {
        if (!hasTouchedPlayer)
        {
            return;
        }

        shakeTimer += Time.deltaTime;
        render.position = originalPos + Random.insideUnitSphere * SHAKING_VALUE;

        if(shakeTimer > SHAKING_DURATION)
        {
            hasTouchedPlayer = false;
            shakeTimer = 0;
            light2d.enabled = false;
            render.gameObject.SetActive(false);
            render.position = originalPos;
            boxCollider.enabled = false;
        }
    }

    private void HandleRespawn()
    {
        // Platform is already activated
        if (boxCollider.isActiveAndEnabled)
        {
            return;
        }

        respawnTimer += Time.deltaTime;

        if(respawnTimer >= RESPAWN_DURATION)
        {
            respawnTimer = 0;
            boxCollider.enabled = true;
            light2d.enabled = true;
            render.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            audioSource.PlayOneShot(Sounds.platform);
            hasTouchedPlayer = true;
        }
    }
}
