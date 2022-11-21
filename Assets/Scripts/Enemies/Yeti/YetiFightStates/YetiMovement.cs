using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiMovement : YetiState
{
    private bool initialized = false;

    private float phaseHealth = 10f;

    private float distanceToPlayerMelee = 2.5f;
    private float platformHeight = 2.8f;
    private float moveSpeed = 3.6f;
    private bool isMoving = true;

    private int left = -1;
    private int right = 1;
    private int noMovement = 0;
    private int platformEscaper = 0;

    private Vector3 playerPos;
    protected Vector3 currentTarget;
    private GameObject currentCollision;

    private void Start()
    {
        if (!initialized)
        {
            Init();
        }
    }

    public override void Init()
    {
        initialized = true;
        isMoving = true;
        yetiShield.Deactivate();
    }

    void Update()
    {
        DoInteractions();
        ManageStateChange();
    }

    public override void DoInteractions()
    {
        animator.SetBool(Harmony.AnimatorParameters.IsFalling, false);
        yetiJump.CheckForFallAnimation();
        if (yetiJump.jumpStarted)
        {
            return;
        }

        playerPos = player.transform.position;
        float distance = Vector2.Distance(playerPos, transform.position);
        ManageDownwardsMovement(playerPos, distance);
        ManageUpwardsMovement(playerPos);
        SetYetiSpriteFlip();
    }

    private void ManageDownwardsMovement(Vector3 playerPos, float distance)
    {
        //Different Platform
        if (transform.position.y - playerPos.y >= platformHeight)
        {
            if (platformEscaper == noMovement)
            {
                platformEscaper = GetPlayerDirection();
            }
            currentTarget = (transform.position + Vector3.right * platformEscaper);
            transform.position = MoveTowardsTarget(currentTarget);
        }
        else if (distance > distanceToPlayerMelee)
        {
            platformEscaper = noMovement;
            currentTarget = playerPos;
            transform.position = MoveTowardsTarget(currentTarget);
        }
        else
        {
            platformEscaper = noMovement;
            isMoving = false;
        }
        animator.SetBool(Harmony.AnimatorParameters.IsRunning, isMoving);
    }

    private void ManageUpwardsMovement(Vector3 playerPos)
    {
        if(playerPos.y - transform.position.y >= platformHeight && !yetiJump.jumpStarted)
        {
            yetiJump.JumpToFightPoint(player.transform.position + Vector3.up);
        }
    }

    private Vector2 MoveTowardsTarget(Vector3 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        return new Vector2(
             transform.position.x + direction * moveSpeed * Time.deltaTime,
             transform.position.y
        );
    }

    private int GetPlayerDirection()
    {
        if(player.transform.position.x - transform.position.x > 0)
        {
            return right;
        }
        return left;
    }

    private void SetYetiSpriteFlip()
    {
        spriteRenderer.flipX = transform.position.x < currentTarget.x ? true : false;
    }

    public override void ManageStateChange()
    {
        if(!isMoving)
        {
            initialized = false;
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.MeleeAttack);
        }
        if (yetiFightStateManager.healthAfterRangeAttackPhase >= yetiHealth.GetLife() + phaseHealth)
        {
            initialized = false;
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.Defense);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DetectPlatformCollision(collision))
        {
            currentCollision = collision.gameObject;
            platformEscaper = noMovement;

            if (rb.velocity.y <= 0 && yetiJump.jumpStarted)
            {
                yetiJump.ResetJumpState();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (DetectPlatformCollision(collision))
        {
            if (rb.velocity == Vector2.zero && yetiJump.jumpStarted && !yetiJump.jumpCalled && currentCollision == collision.gameObject)
            {
                yetiJump.ResetJumpState();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(currentCollision == collision.gameObject)
        {
            currentCollision = null;
        }
    }

    private bool DetectPlatformCollision(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & Harmony.Layers.Platform) != 0 || ((1 << collision.gameObject.layer) & Harmony.Layers.Ground) != 0)
        {
            return true;
        }
        return false;
    }
}
