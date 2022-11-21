using UnityEngine;

public class EnemiesPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float patrolPauseTimerInitialValue = 300f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 nextPosition;
    private GameObject[] patrolPoints;
    private bool playerDetected = false;
    private int nextPatrolPoint = 1;
    private float distanceToPoint;
    private float patrolPauseTimer;
    private bool patrolIsInPause = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        patrolPoints = new GameObject[2];
    }

    void Start()
    {
        patrolPauseTimer = patrolPauseTimerInitialValue;
        animator.SetBool(Harmony.AnimatorParameters.isRunning, true);
        SetPatrolPoints();
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
        patrolIsInPause = false;
        playerDetected = false;
    }

    private void SetPatrolPoints()
    {
        Transform parent = transform.parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.CompareTag(Harmony.Tags.PatrolPoints))
            {
                for (int j = 0; j < parent.GetChild(i).gameObject.transform.childCount; j++)
                {
                    patrolPoints[j] = parent.GetChild(i).gameObject.transform.GetChild(j).gameObject;
                }
            }
        }
    }

    void Update()
    {
        if (!GetComponent<LifeManager>().IsDead())
        {
            if (!playerDetected)
            {
                if (!patrolIsInPause)
                {
                    MoveToNextPatrolPoint();
                }
                else
                {
                    CheckForContinuePatrol();
                }
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        nextPosition = new Vector3(patrolPoints[nextPatrolPoint].transform.position.x, transform.position.y, transform.position.z);
        distanceToPoint = Vector2.Distance(transform.position, nextPosition);
        transform.position = MoveTowardsTarget(nextPosition);
        if (patrolPoints[nextPatrolPoint].transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (patrolPoints[nextPatrolPoint].transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        if (distanceToPoint < 0.02f)
        {
            MakePatrolPause();
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

    internal void PlayerDetected()
    {
        playerDetected = true;
    }

    private void MakePatrolPause()
    {
        animator.SetBool(Harmony.AnimatorParameters.isRunning, false);
        patrolIsInPause = true;
    }
    private void CheckForContinuePatrol()
    {
        if (--patrolPauseTimer <= 0)
        {
            ChangeDirection();
            animator.SetBool(Harmony.AnimatorParameters.isRunning, true);
            patrolPauseTimer = patrolPauseTimerInitialValue;
            patrolIsInPause = false;
        }
    }

    internal void PlayerLost()
    {
        playerDetected = false;
    }

    private void ChangeDirection()
    {
        ChooseNexPatrolPoint();
    }

    private void ChooseNexPatrolPoint()
    {
        nextPatrolPoint++;
        if (nextPatrolPoint == patrolPoints.Length)
        {
            nextPatrolPoint = 0;
        }
    }
}
