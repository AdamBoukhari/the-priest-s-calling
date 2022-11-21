using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    [SerializeField] private float touchDamage;

    private LifeManager lifeManager;

    private void Start()
    {
        lifeManager = GetComponent<LifeManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Multiple ifs necessary because of the ? condition.
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            if (lifeManager?.IsDead() == false)
            {
                if(lifeManager?.GetLife() > 0)
                {
                    PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
                    bool left = transform.position.x < collision.transform.position.x;
                    playerHP.TakeDamageKnockBack(touchDamage, !left);
                }
            }
        }
    }
}
