using UnityEngine;

public class DemonStateIdle : DemonState
{

    void Start()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_move, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, true);
    }

    void Update()
    {
        MoveDemon();
        ManageStateChange();
    }

    public override void ManageStateChange()
    {
        distanceToPlayer = Vector2.Distance(transform.position, new Vector2(playerTransform.position.x, transform.position.y));
        if (!demonManager.stageFleeIsActive)
        {
            if ((distanceToPlayer <= FirebreathDistance) && demonManager.firebreathTimer <= 0)
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.FireBreath);
            }
            if ((distanceToPlayer <= CleaveDistance) && demonManager.cleaveTimer <= 0)
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Cleave);
            }
            if ((distanceToPlayer >= (ChaseDistance + 1f)))
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Chase);
            }
        }
        else
        {
            if (PlayerAtSameHeight() && distanceToPlayer <= FleeDistance - 1f)
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Flee);
            }
            if ((demonManager.spellTimer <= 0))
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.LaunchSpell);
            }
            if (PlayerAtSameHeight() && (distanceToPlayer <= DistanceToRepulsePlayer) && (demonManager.smashTimer <= 0))
            {
                demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Smash);
            }
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
    public override void MoveDemon()
    {
        if (demonManager.stageFleeIsActive)
        {
            demonManager.spellTimer -= Time.deltaTime;
            demonManager.fleeJumpTimer -= Time.deltaTime;
            demonManager.smashTimer -= Time.deltaTime;
        }
        else
        {
            demonManager.firebreathTimer -= Time.deltaTime;
            demonManager.cleaveTimer -= Time.deltaTime;
            demonManager.spellTimer -= Time.deltaTime;
        }
    }
}
