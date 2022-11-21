using UnityEngine;

public class DemonStateChase : DemonState
{
    Vector3 targetDestination;
    private bool downToLeft = false;

    void Start()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_move, true);
    }

    void Update()
    {
        MoveDemon();

        if (!demonJump.jumpStarted && IsGrounded() && PlayerAtSameHeight())
        {
            ManageStateChange();
        }
    }

    
    public override void MoveDemon()
    {
        ManageSpriteFlip();
        if (!demonJump.jumpStarted)
        {
            if (!PlayerAtSameHeight())
            {
                if (playerTransform.position.y > transform.position.y)
                {
                    demonJump.JumpToFightPlayer(playerTransform.position);
                }
                if (playerTransform.position.y < transform.position.y)
                {
                    GetOffPlatform();
                }
            }
            else
            {
                transform.position = MoveTowardsTarget(targetDestination);
            }
        }
        else if (PlayerAtSameHeight() && IsGrounded())
        {
            demonJump.ResetJumpState();
        }

        demonManager.firebreathTimer -= Time.deltaTime;
        demonManager.cleaveTimer -= Time.deltaTime;
    }

    private void GetOffPlatform()
    {
        if (!downToLeft)
        {
            spriteRenderer.flipX = false;
            transform.Translate(speed * Time.deltaTime * -Vector3.right);
        }
        else
        {
            spriteRenderer.flipX = true;
            transform.Translate(speed * Time.deltaTime * Vector3.right);
        }
    }

    private void ManageSpriteFlip()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
            targetDestination = new Vector3(playerTransform.position.x + ChaseDistance, playerTransform.position.y, playerTransform.position.z);
        }
        else
        {
            spriteRenderer.flipX = true;
            targetDestination = new Vector3(playerTransform.position.x - ChaseDistance, playerTransform.position.y, playerTransform.position.z);
        }
    }

    private Vector2 MoveTowardsTarget(Vector3 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        return new Vector2(
             transform.position.x + direction * speed / 1.5f * Time.deltaTime,
             transform.position.y
        );
    }

    private bool PlayerAtSameHeight()
    {
        heightDistanceFromPlayer = transform.position.y - playerTransform.position.y;
        if (heightDistanceFromPlayer <= NormalHeightDistanceFromPlayer && heightDistanceFromPlayer > 0)
        {
            downToLeft = !downToLeft;
            return true;
        }
       
        return false;
    }
    public override void ManageStateChange()
    {
        distanceToPlayer = Vector2.Distance(transform.position, new Vector2(playerTransform.position.x, transform.position.y));
        if ((distanceToPlayer <= FirebreathDistance) && demonManager.firebreathTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.FireBreath);
        }
        if ((distanceToPlayer <= CleaveDistance) && demonManager.cleaveTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Cleave);
        }
        if (distanceToPlayer <= (ChaseDistance+0.5f))
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Idle);
        }
        if(demonManager.stageChangeTimer <= 0)
        {
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Flee);
            demonManager.stageFleeIsActive = true;
            demonManager.stageChangeTimer = DefaultStageChangeTimer;
        }
    }

    private bool IsGrounded()
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

}
