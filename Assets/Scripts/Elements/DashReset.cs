using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReset : MonoBehaviour
{
    private PlayerDash playerDash;
    private Animator animator;
    private AudioSource audioSource;

    bool usedInCurrentJump = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Publisher.PlayerLanding += ResetDashReset;
    }

    private void OnDisable()
    {
        Publisher.PlayerLanding -= ResetDashReset;
    }

    private void ResetDashReset()
    {
        usedInCurrentJump = false;
        animator.SetBool(Harmony.AnimatorParameters.IsActive, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player) && !usedInCurrentJump)
        {
            playerDash = collision.GetComponent<PlayerDash>();

            if(playerDash.WasUsed())
            {
                usedInCurrentJump = true;
                animator.SetBool(Harmony.AnimatorParameters.IsActive, false);
                playerDash.ResetDash();
                audioSource.PlayOneShot(Sounds.dashReset);
            }
        }
    }
}
