using UnityEngine;

public class DemonStateFlee : DemonState
{
    private float distanceForMoveAwayWalls;
    Vector3 targetDestination;
    private Transform[] jumpPoints;
    private const float MaxHeightPos = 51f;
    void Start()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_move, true);
        distanceForMoveAwayWalls = 5f;
        jumpPoints = new Transform[5];
        SetJumpPoints();
    }
    private void SetJumpPoints()
    {
        for (int i = 0; i < transform.GetChild(8).childCount; i++)
        {
            jumpPoints[i] = transform.GetChild(8).GetChild(i);
        }
    }
    void Update()
    {
        demonManager.spellTimer -= Time.deltaTime;
        demonManager.smashTimer -= Time.deltaTime;
        demonManager.fleeJumpTimer -= Time.deltaTime;

        distanceToPlayer = Vector2.Distance(transform.position, new Vector2(playerTransform.position.x, transform.position.y));
        MoveDemon();

        if (!demonJump.jumpStarted && IsGrounded())
        {
            ManageStateChange();
        }

    }

    private bool PlayerAtSameHeight()
    {
        heightDistanceFromPlayer = transform.position.y - playerTransform.position.y;
        if (heightDistanceFromPlayer <= NormalHeightDistanceFromPlayer && heightDistanceFromPlayer > 0)
        {
            return true;
        }

        return false;
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
    public override void ManageStateChange()
    {
        if (demonManager.stageChangeTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Chase);
            demonManager.stageFleeIsActive = false;
            demonManager.stageChangeTimer = DefaultStageChangeTimer;
        }
        if (PlayerAtSameHeight() && distanceToPlayer <= DistanceToRepulsePlayer && demonManager.smashTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Smash);
        }
        if (demonManager.spellTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.LaunchSpell);
        }
        if (!PlayerAtSameHeight() || (distanceToPlayer >= FleeDistance - 0.5f))
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Idle);
        }
    }

    public override void MoveDemon()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


        if (!demonJump.jumpStarted)
        {
            if (neerRightWall || neerLeftWall)
            {
                MoveAwayWalls();
            }
            else if (PlayerAtSameHeight() && demonManager.fleeJumpTimer <= 0 && (transform.position.y <= MaxHeightPos))
            {
                ManageJump();
            }
            else if (PlayerAtSameHeight())
            {
                transform.position = MoveTowardsTarget(playerTransform.position);
            }
        }
        else if (demonJump.jumpStarted && PlayerAtSameHeight() && IsGrounded())
        {
            demonJump.ResetJumpState();
        }

    }

    private void ManageJump()
    {
        int random = UnityEngine.Random.Range(0, 5);
        targetDestination = jumpPoints[random].position;
        demonJump.JumpToFightPlayer(targetDestination);
        demonManager.fleeJumpTimer = DefaultFleeJumpTimer;
    }

    private Vector2 MoveTowardsTarget(Vector3 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        return new Vector2(
             transform.position.x + direction * -speed / 1.5f * Time.deltaTime,
             transform.position.y
        );
    }

    private void MoveAwayWalls()
    {
        if (neerLeftWall)
        {

            spriteRenderer.flipX = true;
            transform.position = MoveTowardsTarget(new Vector3(wallDectectedTransformX - distanceForMoveAwayWalls, transform.position.y, transform.position.z));
            if (transform.position.x >= (wallDectectedTransformX + distanceForMoveAwayWalls))
            {
                neerLeftWall = false;
            }
        }
        if (neerRightWall)
        {
            spriteRenderer.flipX = false;
            transform.position = MoveTowardsTarget(new Vector3(wallDectectedTransformX + distanceForMoveAwayWalls, transform.position.y, transform.position.z));

            if (transform.position.x <= (wallDectectedTransformX - distanceForMoveAwayWalls))
            {
                neerRightWall = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.MinLimitX) && !neerLeftWall)
        {
            neerLeftWall = true;
            SetWallPosition();
        }

        if (collision.gameObject.CompareTag(Harmony.Tags.MaxLimitX) && !neerRightWall)
        {
            neerRightWall = true;
            SetWallPosition();
        }
    }

    private void SetWallPosition()
    {
        wallDectectedTransformX = transform.position.x;
    }
}
