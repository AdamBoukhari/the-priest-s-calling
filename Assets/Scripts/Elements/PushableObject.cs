using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private const float PUSH_SPEED = 3.5f;

    private Rigidbody2D rigidBody;
    private LayerMask playerLayer;
    private Vector3 originalPos;

    private bool isBeingPushed = false;
    private float direction = 0;
    private float raycastLength = 0.4f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerLayer = Harmony.Layers.Player;
        originalPos = transform.position;
    }

    void Update()
    {
        if (!isBeingPushed)
        {
            return;
        }

        rigidBody.velocity = new Vector2(direction * PUSH_SPEED, rigidBody.velocity.y);
    }

    private int GetPlayerDirection()
    {
        return gameObject.GetLayerDirection(playerLayer, transform.position, raycastLength);
    }

    public void Reset()
    {
        transform.position = originalPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            isBeingPushed = true;
            direction = GetPlayerDirection();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            isBeingPushed = false;
            direction = 0;
        }
    }
}
