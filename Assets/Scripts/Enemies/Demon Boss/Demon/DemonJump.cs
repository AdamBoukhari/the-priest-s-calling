using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonJump : MonoBehaviour
{
    private Rigidbody2D rb;

    private const float FIGHT_JUMP_DURATION = 1.5f;
    private const float JUMP_CHARGE_DURATION = 0.6f;

    [HideInInspector] public bool jumpStarted = false;

    private Vector3 point1;
    private Vector3 point2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }    
    public bool JumpStarted()
    {
        return jumpStarted;
    }

    public void JumpToFightPlayer(Vector3 jumpDestination)
    {
        InitializeJump(jumpDestination, FIGHT_JUMP_DURATION);
    }

    private void InitializeJump(Vector3 jumpDestination, float jumpDuration)
    {
        jumpStarted = true;
        point1 = transform.position;
        point2 = jumpDestination;
        StartCoroutine(ChargeJump(jumpDuration));
    }

    private IEnumerator ChargeJump(float jumpDuration)
    {
        yield return new WaitForSeconds(JUMP_CHARGE_DURATION);
        rb.velocity = ParabolaComputer.GetRBImpulse(point1, point2, jumpDuration);
    }
    public void ResetJumpState()
    {
        StartCoroutine(WaitForDeactivateJumpState());
    }

    private IEnumerator WaitForDeactivateJumpState()
    {
        yield return new WaitForSeconds(0.5f);
        jumpStarted = false;
    }
}
