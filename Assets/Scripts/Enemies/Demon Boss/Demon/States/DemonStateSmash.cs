using UnityEngine;

public class DemonStateSmash : DemonState
{
    private GameObject smashColliderObject;
    // Start is called before the first frame update
    void Start()
    {
        demonSmashEnded = false;
        smashColliderObject = transform.GetChild(2).gameObject;
    }

    public void CollisionDetected(SmashCollisions attacksCollisions)
    {
        if (playerTransform.position.x < transform.position.x)
        {
            if (attacksCollisions.gameObject == smashColliderObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakeDamageKnockBack(0.5f, true);
            }
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            if (attacksCollisions.gameObject == smashColliderObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakeDamageKnockBack(0.5f, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveDemon();
        ManageStateChange();

    }
    public override void ManageStateChange()
    {
        if (demonSmashEnded)
        {
            animator.SetBool(Harmony.AnimatorParameters.Demon_smash, false);
            demonSmashEnded = false;
            demonManager.smashTimer = DefaultSmashTimer;
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Flee);
        }
    }

    public override void MoveDemon()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_smash, true);
    }
}
