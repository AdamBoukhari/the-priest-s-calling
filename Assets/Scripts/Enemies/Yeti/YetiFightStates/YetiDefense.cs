using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiDefense : YetiState
{
    private bool jumped = false;
    private GameObject currentCollision;

    private void Start()
    {
        jumped = false;
    }

    public override void Init()
    {

    }

    void Update()
    {
        DoInteractions();
        ManageStateChange();
    }

    public override void DoInteractions()
    {
        if (yetiJump.jumpStarted || jumped)
        {
            return;
        }

        yetiShield.Activate();
        rb.velocity = Vector3.zero;
        yetiJump.defensiveJump = true;
        yetiJump.JumpToFightPoint(yetiFightStateManager.jumpPoints[Random.Range(0, yetiFightStateManager.jumpPoints.Length - 1)].transform.position);
        jumped = true;
    }

    public override void ManageStateChange()
    {
        if(jumped && !yetiJump.jumpStarted)
        {
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.RangeAttack);
        }
    }
}
