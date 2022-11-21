using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            PlayerHealth playerHP = collision.GetComponent<PlayerHealth>();
            bool left = transform.position.x < collision.transform.position.x;
            playerHP.TakeDamageKnockBack(damage, !left);
            if (System.Array.Exists(animator.parameters, p => p.name == "collision"))
            {
                animator.SetTrigger("collision");
            }

        }
    }

    private void CollisionAnimationEnded()
    {
        gameObject.SetActive(false);
    }
}
