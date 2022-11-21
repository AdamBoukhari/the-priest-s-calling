using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsBandit : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Harmony.Tags.Player)
        {
            animator.SetBool("PlayerEnteredTheRoom", true);
        }
    }
}
