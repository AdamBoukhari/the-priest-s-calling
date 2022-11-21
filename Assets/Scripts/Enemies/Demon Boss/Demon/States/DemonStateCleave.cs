using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonStateCleave : DemonState
{
    private GameObject cleaveColliderRightObject;
    private GameObject cleaveColliderLeftObject;
    void Start()
    {
        demonCleaveEnded = false;
        SetCleaveCollidersObjects();
    }

    private void SetCleaveCollidersObjects()
    {
        cleaveColliderRightObject = transform.GetChild(4).gameObject;
        cleaveColliderLeftObject = transform.GetChild(3).gameObject;
    }

    void Update()
    {    
        MoveDemon();
        ManageStateChange();     
    }

    public void CollisionDetected(CleaveCollisions cleaveCollisions)
    {
        if (playerTransform.position.x < transform.position.x)
        {
            if (cleaveCollisions.gameObject == cleaveColliderLeftObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakePlainDamage(0.5f);
            }
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            if (cleaveCollisions.gameObject == cleaveColliderRightObject)
            {
                playerTransform.gameObject.GetComponent<PlayerHealth>().TakePlainDamage(0.5f);
            }
        }
    }

    public override void ManageStateChange()
    {
        if (demonCleaveEnded)
        {
            animator.SetBool(Harmony.AnimatorParameters.Demon_cleave, false);
            demonCleaveEnded = false;
            demonManager.cleaveTimer = DefaultCleaveTimer;
            demonManager.ChangeDemonState(DemonManager.DemonStateToSwitch.Chase);
        }
    }

    public override void MoveDemon()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_cleave, true);
    }
}
