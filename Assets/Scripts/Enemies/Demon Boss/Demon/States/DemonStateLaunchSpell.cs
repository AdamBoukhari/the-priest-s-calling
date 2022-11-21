using UnityEngine;

public class DemonStateLaunchSpell : DemonState
{
    void Start()
    {
        demonCastEnded = false;
    }

    void Update()
    {
        MoveDemon();
        ManageStateChange();

    }
    public override void ManageStateChange()
    {
        if (demonCastEnded)
        {
            animator.SetBool(Harmony.AnimatorParameters.Demon_cast, false);
            demonCastEnded = false;
            demonManager.spellTimer = DefaultSpellTimer;
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Flee);
        }
    }

    public override void MoveDemon()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        animator.SetBool(Harmony.AnimatorParameters.Demon_cast, true);
        demonManager.smashTimer -= Time.deltaTime;
    }
}
