using UnityEngine;

public class DemonStateFireBreath : DemonState
{
    private GameObject fireColliderRightObject;
    private GameObject fireColliderLeftObject;
    void Start()
    {
        demonFirebreathEnded = false;
        SetFireCollidersObjects();
    }

    private void SetFireCollidersObjects()
    {
        fireColliderRightObject = transform.GetChild(0).gameObject;
        fireColliderLeftObject = transform.GetChild(1).gameObject;
    }
    void Update()
    {
        MoveDemon();
        ManageStateChange();
    }
    public override void ManageStateChange()
    {
        if (demonFirebreathEnded)
        {
            animator.SetBool(Harmony.AnimatorParameters.Demon_fire, false);
            demonFirebreathEnded = false;
            demonManager.firebreathTimer = DefaultFirebreathTimer;
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Chase);
        }

    }

    public override void MoveDemon()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_fire, true);
    }

    public void CollisionDetected(FireBreathCollision fireScript)
    {
        if (playerTransform.position.x < transform.position.x)
        {
            if (fireScript.gameObject == fireColliderLeftObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakePlainDamage(0.5f);
            }
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            if (fireScript.gameObject == fireColliderRightObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakePlainDamage(0.5f);
            }
        }
    }

}
