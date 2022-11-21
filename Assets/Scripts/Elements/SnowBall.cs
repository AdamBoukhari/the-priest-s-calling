using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    private float damage = 1f;
    private const int ROTATION_SPEED = 3;
    private const int MAX_LIFE_TIME = 3;
    private float lifetime;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.gameObject.transform.parent = null;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, ROTATION_SPEED));

        if(lifetime < 0)
        {
            gameObject.SetActive(false);
        }
        lifetime -= Time.deltaTime;
    }

    private void OnEnable()
    {
        lifetime = MAX_LIFE_TIME;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Harmony.Tags.Player) && gameObject.activeInHierarchy)
        {
            PlayerHealth playerHP = collision.GetComponent<PlayerHealth>();
            bool left = transform.position.x < collision.transform.position.x;
            playerHP.TakeDamageKnockBack(damage, !left);
            hitPlayer();
        }
    }

    public void hitPlayer()
    {
        audioSource.PlayOneShot(Sounds.snowball_hit);
        gameObject.SetActive(false);
    }
}
