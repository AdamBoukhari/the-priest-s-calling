using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonManager : MonoBehaviour
{
    public string BOSS_NAME = "Darkhen's Demon";

    //En public pour eviter le reset lors du changement de state
    public float firebreathTimer;
    public float cleaveTimer;
    public float stageChangeTimer;
    public float spellTimer;
    public float smashTimer;
    public float fleeJumpTimer;
    public bool stageFleeIsActive;

    public enum DemonStateToSwitch { Chase, Cleave, FireBreath, Flee, LaunchSpell, Smash, Idle }

    private const float DefaultStageChangeTimer = 12f;
    private const float DefaultFirebreathTimer = 2f;
    private const float DefaultCleaveTimer = 2f;
    private const float DefaultSpellTimer = 2f;
    private const float DefaultSmashTimer = 2f;
    private const float DefaultFleeJumpTimer = 2f;

    private Animator animator;
    private SlimeDamage slimeDamageScript;
    private SlimeManager slimeManager;
    private DemonHealth demonHealthScript;
    private DemonState demonState;
    private Transform playerTransform;

    private bool dead = false;

    void Start()
    {
        stageFleeIsActive = false;
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
    }

    void Update()
    {
        stageChangeTimer -= Time.deltaTime;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadFight;
        Publisher.FetchData -= SaveFight;
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadFight;
        Publisher.FetchData += SaveFight;

        animator = GetComponent<Animator>();
        demonState = GetComponent<DemonState>();
        slimeManager= GetComponent<SlimeManager>();
        slimeDamageScript = GetComponent<SlimeDamage>();
        demonHealthScript = GetComponent<DemonHealth>();

        animator.SetBool(Harmony.AnimatorParameters.Slime_transformation, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, true);

        StartCoroutine(WaitBeforeStartStateMachine());
    }
    private void LoadFight(bool loadPosition, bool playerDead)
    {
        dead = XMLManager.Instance.savedData.bossFightInfo.dead;
        if (dead)
        {
            gameObject.SetActive(false);
            return;
        }
        ResetState();
        demonHealthScript.DeactivateHealthBar();
        demonHealthScript.RestoreLife();
        ResetAnimatorParameters();
        ResetScriptsActivationForSlime();
        slimeManager.ResetFight();
    }

    private void ResetScriptsActivationForSlime()
    {
        demonHealthScript.enabled = false;
        slimeManager.enabled = true;
        slimeDamageScript.enabled = true;
    }

    private void ResetAnimatorParameters()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_cast, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_cleave, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_die, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_fire, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_hit, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_move, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_smash, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_transformation, false);
    }

    private void ResetState()
    {
        Destroy(demonState);
        demonState = gameObject.AddComponent<DemonStateChase>() as DemonStateChase;
        demonState.enabled = false;
    }

    private void SaveFight(bool switchScene)
    {
        XMLManager.Instance.savedData.yetiInfo.dead = dead;
    }
    private void PassInStage2()
    {
        ResetState();
        ResetAnimatorParameters();
        ResetScriptsActivationForSlime();
        slimeManager.StartStage2();
    }

    private IEnumerator WaitBeforeStartStateMachine()
    {
        yield return new WaitForSeconds(0.2f);
        ResetTimers();
        ScriptsActivationForDemon();
        CheckIfHealthScriptIsActive();
        ManageAnimationForDemonStage();
    }

    private void ManageAnimationForDemonStage()
    {
        animator.SetBool(Harmony.AnimatorParameters.Demon_idle, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_hit, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_die, false);
        animator.SetBool(Harmony.AnimatorParameters.Demon_move, true);
    }

    private void ScriptsActivationForDemon()
    {
        demonState.enabled = true;
        slimeManager.enabled = false;
        slimeDamageScript.enabled = false;
        demonHealthScript.enabled = true;
    }

    private void ResetTimers()
    {
        spellTimer = DefaultSpellTimer;
        smashTimer = DefaultSmashTimer;
        stageChangeTimer = DefaultStageChangeTimer;
        cleaveTimer = DefaultCleaveTimer;
        firebreathTimer = DefaultFirebreathTimer;
        fleeJumpTimer = DefaultFleeJumpTimer;
    }

    private void CheckIfHealthScriptIsActive()
    {
        //Si le script n'est pas encore actif et que la vie n'est pas set on bloque le joueur.
        while (!demonHealthScript.isActiveAndEnabled && !demonHealthScript.HealthBarIsActive())
        {
            playerTransform.gameObject.GetComponent<PlayerInteraction>().DeactivatePlayerControls();
        }
        playerTransform.gameObject.GetComponent<PlayerInteraction>().ActivatePlayerControls();
    }

    public void ChangeDemonState(DemonStateToSwitch nextState)
    {
        Destroy(demonState);

        switch (nextState)
        {
            case DemonStateToSwitch.Chase:
                {
                    demonState = gameObject.AddComponent<DemonStateChase>() as DemonStateChase;
                    break;
                }
            case DemonStateToSwitch.Cleave:
                {
                    demonState = gameObject.AddComponent<DemonStateCleave>() as DemonStateCleave;
                    break;
                }
            case DemonStateToSwitch.FireBreath:
                {
                    demonState = gameObject.AddComponent<DemonStateFireBreath>() as DemonStateFireBreath;
                    break;
                }
            case DemonStateToSwitch.Flee:
                {
                    demonState = gameObject.AddComponent<DemonStateFlee>() as DemonStateFlee;
                    break;
                }
            case DemonStateToSwitch.LaunchSpell:
                {
                    demonState = gameObject.AddComponent<DemonStateLaunchSpell>() as DemonStateLaunchSpell;
                    break;
                }
            case DemonStateToSwitch.Smash:
                {
                    demonState = gameObject.AddComponent<DemonStateSmash>() as DemonStateSmash;
                    break;
                }
            case DemonStateToSwitch.Idle:
                {
                    demonState = gameObject.AddComponent<DemonStateIdle>() as DemonStateIdle;
                    break;
                }
        }
    }
    public void IsFullDead()
    {
        dead = true;
        XMLManager.Instance.Save(false);
    }
}
