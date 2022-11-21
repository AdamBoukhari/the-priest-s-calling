using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spike : MonoBehaviour
{
    private const float SPIKE_DAMAGE = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakePlainDamage(SPIKE_DAMAGE);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakePlainDamage(SPIKE_DAMAGE);
        }
    }
}
