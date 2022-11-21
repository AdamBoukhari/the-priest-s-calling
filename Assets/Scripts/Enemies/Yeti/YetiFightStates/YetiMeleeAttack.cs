using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiMeleeAttack : YetiState
{
    private bool attacked = false;
    private const float ATTACK_TIMER = 1.2f;

    public override void Init()
    {

    }

    void Update()
    {
        yetiFightStateManager.currentTimer -= Time.deltaTime;
        DoInteractions();
        ManageStateChange();
    }

    public override void DoInteractions()
    {
        if (!attacked && yetiFightStateManager.currentTimer <= 0)
        {
            yetiFightStateManager.currentTimer = ATTACK_TIMER;
            spriteRenderer.flipX = transform.position.x < player.transform.position.x ? true : false;
            animator.SetTrigger(Harmony.AnimatorParameters.Attack);
            attacked = true;
        }
    }

    public override void ManageStateChange()
    {
        if (!animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("Attack1") && yetiFightStateManager.currentTimer <= 0)
        {
            attacked = false;
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.Movement);
        }
    }
}
