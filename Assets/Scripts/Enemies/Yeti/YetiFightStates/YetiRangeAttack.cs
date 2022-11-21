using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiRangeAttack : YetiState
{
    private const float MAX_STATE_TIMER = 6f;
    private const float THROW_COOLDOWN_COMBAT = .75f;
    private float stateTimer;
    private float throwTimer = 0;

    void Start()
    {
        stateTimer = MAX_STATE_TIMER;
    }

    public override void Init()
    {

    }

    void Update()
    {
        stateTimer -= Time.deltaTime;

        DoInteractions();
        ManageStateChange();
    }

    public override void DoInteractions()
    {
        if (throwTimer < 0)
        {
            yetiSnowballAttack.ThrowSnowballCombatPhase();
            throwTimer = THROW_COOLDOWN_COMBAT;
        }

        throwTimer -= Time.deltaTime;
    }

    public override void ManageStateChange()
    {
        if(stateTimer <= 0)
        {
            yetiFightStateManager.healthAfterRangeAttackPhase = yetiHealth.GetLife();
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.Movement);
        }
    }
}
