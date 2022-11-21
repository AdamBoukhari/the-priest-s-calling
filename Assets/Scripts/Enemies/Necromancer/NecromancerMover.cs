using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float distanceForLaunchRessurection = 1f;

    private Animator animator;
    private List<GameObject> deadTargets;
    private Publisher publisher;
    private readonly Vector3[] patrolPositions = new Vector3[4];
    private int positionIndex;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D resurrectionZone;
    private Rigidbody2D rb;
    private float distanceToRessurectionTarget;

    void Start()
    {
        publisher = GetComponent<Publisher>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        patrolPositions[0] = new Vector3(transform.position.x + 2.5f, transform.position.y + 1f, transform.position.z);
        patrolPositions[1] = new Vector3(transform.position.x - 1f, transform.position.y + 1.5f, transform.position.z);
        patrolPositions[2] = new Vector3(transform.position.x + 3.5f, transform.position.y - 0.5f, transform.position.z);
        patrolPositions[3] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        deadTargets = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {
        Publisher.EnemyDying += AddEnemiForRessurection;
        Publisher.EnemyRevive += RemoveEnemyRessurected;
    }

    private void OnDisable()
    {
        Publisher.EnemyDying -= AddEnemiForRessurection;
        Publisher.EnemyRevive -= RemoveEnemyRessurected;
        resurrectionZone = null;
        deadTargets = new List<GameObject>();
    }

    private void RemoveEnemyRessurected(GameObject enemy)
    {
        if (deadTargets.Contains(enemy))
        {
            animator.SetBool(Harmony.AnimatorParameters.isCasting, false);
            deadTargets.Remove(enemy);
        }
        if (enemy == gameObject)
            publisher.CallNecromancerInZone(resurrectionZone, gameObject);
    }

    private void AddEnemiForRessurection(GameObject deadEnemy, PolygonCollider2D enemyResurrectionZone)
    {
        if (deadEnemy == gameObject)
        {
            publisher.CallNecromancerDeadInZone(resurrectionZone, gameObject);
        }
        else
        {
            if (!deadTargets.Contains(deadEnemy) && enemyResurrectionZone == resurrectionZone)
            {
                deadTargets.Add(deadEnemy);
            }
        }
    }
    void Update()
    {
        if (!GetComponent<LifeManager>().IsDead())
        {
            rb.gravityScale = 0;
            if (deadTargets.Count == 0)
            {
                UpdatePatrolMove();
            }
            else
            {
                UpdateResurrectionMove();
            }
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    private void UpdateResurrectionMove()
    {
        animator.SetBool(Harmony.AnimatorParameters.isRunning, true);
        distanceToRessurectionTarget = Vector2.Distance(transform.position, deadTargets[0].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, deadTargets[0].transform.position, moveSpeed * Time.deltaTime);

        if (deadTargets[0].transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (deadTargets[0].transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        if (distanceToRessurectionTarget < distanceForLaunchRessurection)
        {
            animator.SetBool(Harmony.AnimatorParameters.isCasting, true);
        }
    }

    private void LaunchRessurectionOnTarget()
    {
        animator.SetBool(Harmony.AnimatorParameters.isCasting, false);
        animator.SetBool(Harmony.AnimatorParameters.isRunning, false);
        deadTargets[0].GetComponent<Animator>().SetBool(Harmony.AnimatorParameters.isRevive, true);
        deadTargets[0].GetComponent<Animator>().SetBool(Harmony.AnimatorParameters.isDead, false);
        deadTargets.Remove(deadTargets[0]);
        CheckIfNextRessurectionTargetIsInRange();
    }

    private void CheckIfNextRessurectionTargetIsInRange()
    {
        if (deadTargets.Count > 0)
        {
            distanceToRessurectionTarget = Vector2.Distance(transform.position, deadTargets[0].transform.position);
        }
        if (distanceToRessurectionTarget < distanceForLaunchRessurection)
        {
            AnimatorClipInfo[] animatorinfo = animator.GetCurrentAnimatorClipInfo(0);
            if (animatorinfo[0].clip.name == Harmony.AnimatorStates.Necro_Cast)
            {
                animator.Play(Harmony.AnimatorStates.Necro_Cast, -1, 0.0f);
            }
        }
    }

    private void UpdatePatrolMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPositions[positionIndex], Time.deltaTime * moveSpeed);
        ManageSpriteFlip();
        if (transform.position == patrolPositions[positionIndex])
        {
            if (positionIndex == patrolPositions.Length - 1)
            {
                positionIndex = 0;
            }
            else
            {
                positionIndex++;
            }
        }
    }
    private void ManageSpriteFlip()
    {
        switch (positionIndex)
        {
            case 0:
                spriteRenderer.flipX = true;
                break;
            case 1:
                spriteRenderer.flipX = false;
                break;
            case 2:
                spriteRenderer.flipX = true;
                break;
            case 3:
                spriteRenderer.flipX = false;
                break;
            default:
                break;
        }
    }

    private IEnumerator OnTriggerStay2D(Collider2D collision)
    {
        if (!resurrectionZone && collision.gameObject.CompareTag(Harmony.Tags.CameraZone))
        {
            yield return new WaitForSeconds(1);
            resurrectionZone = collision.gameObject.GetComponent<PolygonCollider2D>();
            publisher.CallNecromancerInZone(resurrectionZone, gameObject);
        }
    }
}
