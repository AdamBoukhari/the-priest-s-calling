using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private const float KNOCKBACK_FORCE = 50000;
    private const float HIT_VISUAL_DURATION = 0.05f;

    [SerializeField] private Material flashingMaterial;
    [SerializeField] private float maxLife = 3;
    [SerializeField] private float life;

    private PolygonCollider2D resurrectionZone;
    private Animator animator;
    private Publisher publisher;
    private LootEnemies lootEnemies;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Material originalMaterial;
    private AudioSource audioSource;
    private AudioClip hitSnd;
    private AudioClip deathSnd;

    private List<GameObject> necromancersInZone;
    private bool isWaitingForNecromancer;

    private void Start()
    {
        publisher = GetComponent<Publisher>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lootEnemies = GameManager.Instance.GetComponent<LootEnemies>();

        originalMaterial = spriteRenderer.material;
        LoadSound();

        isWaitingForNecromancer = false;
        necromancersInZone = new List<GameObject>();
    }

    private void LoadSound()
    {
        hitSnd = Resources.Load("Audio/SFX/Enemy/Hit/Hit 2") as AudioClip;
        deathSnd = Resources.Load("Audio/SFX/Enemy/Death/Enemy Kill 6") as AudioClip;
    }

    private void OnEnable()
    {
        isWaitingForNecromancer = false;
        Publisher.NecromancerSpawn += AddNecromancerForRessurection;
        Publisher.NecromancerDead += RemoveNecromancerForRessurection;
        life = maxLife;

    }

    private void OnDisable()
    {
        Publisher.NecromancerSpawn -= AddNecromancerForRessurection;
        Publisher.NecromancerDead -= RemoveNecromancerForRessurection;

    }

    private void RemoveNecromancerForRessurection(PolygonCollider2D necromancerZone, GameObject necromancer)
    {
        if (necromancerZone == resurrectionZone)
        {
            necromancersInZone.Remove(necromancer);
        }
        if (isWaitingForNecromancer && necromancersInZone.Count == 0)
        {
            gameObject.SetActive(false);
            isWaitingForNecromancer = false;
        }
    }

    private void AddNecromancerForRessurection(PolygonCollider2D necromancerZone, GameObject necromancer)
    {
        if (necromancerZone == resurrectionZone && gameObject != necromancer)
        {
            if (necromancer.activeInHierarchy)
            {
                if (!necromancersInZone.Contains(necromancer))
                {
                    necromancersInZone.Add(necromancer);
                }
            }
        }
    }

    public void LoseLife(float amount, bool left)
    {
        if (isWaitingForNecromancer || life <= 0)
        {
            return;
        }

        life -= amount;
        StartCoroutine(FlashSpriteOnDamage());
        // Enemy is dead
        if (life <= 0)
        {
            publisher.CallEnemyDyingEvent(gameObject, resurrectionZone);
            animator.SetBool(Harmony.AnimatorParameters.isDead, true);
            lootEnemies.RandomDropHearts(gameObject.transform.position);
            publisher.CallEnemyDiedEvent();

            if (necromancersInZone.Count > 0)
            {
                isWaitingForNecromancer = true;
            }
            else if (gameObject.CompareTag(Harmony.Tags.Necromancer))
            {
                animator.SetTrigger("Death");
            }

            audioSource.PlayOneShot(deathSnd);
        }
        else
        {
            audioSource.PlayOneShot(hitSnd);
        }

        // Enemy is a necromancer, so no knockback
        if (gameObject.CompareTag(Harmony.Tags.Necromancer) && gameObject.activeInHierarchy)
        {
            AnimatorClipInfo[] animatorinfo = animator.GetCurrentAnimatorClipInfo(0);
            if (animatorinfo[0].clip.name == Harmony.AnimatorStates.Necro_Cast)
            {
                animator.Play(Harmony.AnimatorStates.Necro_Cast, -1, 0.0f);
            }
        }
        else
        {
            int directionMultiplier = left ? -1 : 1;
            if (!gameObject.CompareTag(Harmony.Tags.bat))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector2(directionMultiplier * KNOCKBACK_FORCE, 1.5f * KNOCKBACK_FORCE), ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator FlashSpriteOnDamage()
    {
        spriteRenderer.material = flashingMaterial;
        yield return new WaitForSeconds(HIT_VISUAL_DURATION);
        spriteRenderer.material = originalMaterial;
    }

    // These 2 methods are called through animations
    private void DeathAnimationEnded()
    {
        if (!isWaitingForNecromancer)
        {
            gameObject.SetActive(false);
        }
    }
    private void IsRevive()
    {
        animator.SetBool(Harmony.AnimatorParameters.isRevive, false);
        life = maxLife;
        isWaitingForNecromancer = false;
        publisher.CallEnemyRessurected(gameObject);
    }

    internal bool IsDead()
    {
        return isWaitingForNecromancer;
    }

    public float GetLife()
    {
        return life;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!resurrectionZone && collision.gameObject.CompareTag(Harmony.Tags.CameraZone))
        {
            resurrectionZone = collision.gameObject.GetComponent<PolygonCollider2D>();
        }
    }

}
