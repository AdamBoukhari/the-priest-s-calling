using Cinemachine;
using System;
using UnityEngine;

public class EnemiesOnPatrolAttack : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToDetectPlayer;
    [SerializeField] private float attackTimerInitialValue = 150f;
    [SerializeField] private bool detectPlayerOnlyInFront = false;
    [SerializeField] private float heightDistanceToAttackPlayer = 1.5f;
    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private float attackRange = 0.5f;

    private Transform playerTransform;
    private Animator animator;
    private PolygonCollider2D zone;
    private CinemachineVirtualCamera vCam;
    private Collider2D vCamZone;
    private EnemiesPatrol patrolScript;
    private SpriteRenderer spriteRenderer;
    private Vector3 nextPosition;
    private LayerMask groundLayer;
    private SpriteRenderer sprite;

    private const float Damage = 1;
    private bool playerInRange;
    private float distanceToAttackPlayer;
    private float heightDistance;
    private float distanceToPlayer;
    private float attackTimer;
    private bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        patrolScript = GetComponent<EnemiesPatrol>();

        playerInRange = false;
        groundLayer = Harmony.Layers.Ground;
        attackTimer = attackTimerInitialValue;
        sprite = GetComponent<SpriteRenderer>();

        if (gameObject.CompareTag(Harmony.Tags.Grunt))
        {
            distanceToAttackPlayer = 1;
        }
        else if (gameObject.CompareTag(Harmony.Tags.Ghost))
        {
            distanceToAttackPlayer = distanceToDetectPlayer;
        }
    }
    private void OnDisable()
    {
        Publisher.PushData -= ResetState;
    }

    private void OnEnable()
    {
        Publisher.PushData += ResetState;
    }

    private void ResetState(bool loadPosition, bool playerDead)
    {
        playerInRange = false;
        isAttacking = false;
    }

    void Update()
    {
        vCam = (CinemachineVirtualCamera)GameObject.FindGameObjectWithTag(Harmony.Tags.MainCamera).GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        if (vCam)
        {
            vCamZone = vCam.GetComponentInParent<CinemachineConfiner>()?.m_BoundingShape2D;
        }
        if (!GetComponent<LifeManager>().IsDead())
        {
            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            heightDistance = Vector2.Distance(transform.position, new Vector3(transform.position.x, playerTransform.position.y, transform.position.z));
            if (IsGrounded() && PlayerIsInSameZone())
            {
                if (playerInRange)
                {
                    if (!isAttacking)
                    {
                        PursuitPlayer();
                    }
                    else
                    {
                        CheckForReturnInPursuit();
                    }
                }
                else
                {
                    CheckIsPlayerInRange();
                }
            }
            else
            {
                ReturnInPatrol();
            }
        }
    }

    private bool PlayerIsInSameZone()
    {
        if (vCamZone != zone)
            return false;

        return true;
    }

    private void ReturnInPatrol()
    {
        playerInRange = false;
        patrolScript.PlayerLost();
    }

    private void CheckIsPlayerInRange()
    {
        if (distanceToPlayer < distanceToDetectPlayer)
        {
            if (detectPlayerOnlyInFront)
            {
                if (!CheckIfPlayerInFront() || !PlayerAtSameHeight())
                {
                    playerInRange = false;
                    return;
                }
            }
            playerInRange = true;
            patrolScript.PlayerDetected();
        }
    }

    private bool CheckIfPlayerInFront()
    {
        if ((playerTransform.position.x < transform.position.x && spriteRenderer.flipX == false) || (playerTransform.position.x > transform.position.x && spriteRenderer.flipX == true))
        {
            return true;
        }
        return false;
    }

    private void CheckIfPlayerStillInRange()
    {
        if (distanceToPlayer > distanceToDetectPlayer)
        {
            playerInRange = false;
            patrolScript.PlayerLost();
        }
    }

    private void CheckForReturnInPursuit()
    {
        if (--attackTimer <= 0)
        {
            animator.ResetTrigger(Harmony.AnimatorParameters.isAttacking);
            animator.SetBool(Harmony.AnimatorParameters.isRunning, true);
            attackTimer = attackTimerInitialValue;
            isAttacking = false;
        }
    }

    private void PursuitPlayer()
    {
        if (!IsGrounded())
            return;
        animator.SetBool(Harmony.AnimatorParameters.isRunning, true);
        CheckIfPlayerStillInRange();
        nextPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = MoveTowardsTarget(nextPosition);
        ManageSpriteFlip();
        if (distanceToPlayer < distanceToAttackPlayer)
        {
            AttackPlayer();
        }
    }

    private void ManageSpriteFlip()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    private Vector2 MoveTowardsTarget(Vector3 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        return new Vector2(
             transform.position.x + direction * moveSpeed / 1.2f * Time.deltaTime,
             transform.position.y
        );
    }

    private bool PlayerAtSameHeight()
    {
        if (heightDistance <= heightDistanceToAttackPlayer)
        {
            return true;
        }
        return false;
    }

    private void DealDammage()
    {
        Collider2D[] hitPlayers;
        if (!sprite.flipX)
        {
            hitPlayers = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, Harmony.Layers.Player);
        }
        else
        {
            hitPlayers = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, Harmony.Layers.Player);
        }

        foreach (Collider2D player in hitPlayers)
        {
            player?.GetComponent<PlayerHealth>().TakeDamageKnockBack(Damage, !sprite.flipX);
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool(Harmony.AnimatorParameters.isRunning, false);
        animator.SetTrigger(Harmony.AnimatorParameters.isAttacking);
        isAttacking = true;
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointLeft == null || attackPointRight == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!zone && collision.gameObject.CompareTag(Harmony.Tags.CameraZone))
        {
            zone = collision.gameObject.GetComponent<PolygonCollider2D>();
        }
    }
}
