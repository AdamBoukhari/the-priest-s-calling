using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiFightStateManager : MonoBehaviour
{
    public enum YetiStateToSwitch { Death, Movement, MeleeAttack, RangeAttack, Defense }
    public string BOSS_NAME = "Yeti";
    public Transform attackPointLeft;
    public Transform attackPointRight;
    public GameObject[] jumpPoints;
    private YetiState yetiState;
    private YetiHealth yetiHealth;
    private YetiShield yetiShield;
    private YetiJump yetiJump;
    private YetiSound yetiSound;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private YetiFightTrigger yetiFightTrigger;
    [SerializeField] private ParticleSystem snowGenerator;
    [SerializeField] private ParticleSystem tempest;

    [HideInInspector] public YetiStateToSwitch currentState;
    [HideInInspector] public float healthAfterRangeAttackPhase;

    [HideInInspector] public bool dead = false;
    private bool fightStarted = false;
    private bool outOfAscensionZone;

    [HideInInspector] public float currentTimer;
    private float attackRange = .75f;
    private float damage = 1f;
    private LayerMask playerLayer;

    private void Awake()
    {
        yetiState = GetComponent<YetiState>();
        yetiHealth = GetComponent<YetiHealth>();
        yetiShield = GetComponent<YetiShield>();
        yetiJump = GetComponent<YetiJump>();
        yetiSound = GetComponent<YetiSound>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerLayer = Harmony.Layers.Player;
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadYeti;
        Publisher.FetchData += SaveYeti;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadYeti;
        Publisher.FetchData -= SaveYeti;
    }

    public void ChangeYetiState(YetiStateToSwitch nextState)
    {
        Destroy(yetiState);

        switch (nextState)
        {
            case YetiStateToSwitch.Movement:
                {
                    currentState = YetiStateToSwitch.Movement;
                    yetiState = gameObject.AddComponent<YetiMovement>();
                    break;
                }
            case YetiStateToSwitch.MeleeAttack:
                {
                    currentState = YetiStateToSwitch.MeleeAttack;
                    yetiState = gameObject.AddComponent<YetiMeleeAttack>();
                    break;
                }
            case YetiStateToSwitch.RangeAttack:
                {
                    currentState = YetiStateToSwitch.RangeAttack;
                    yetiState = gameObject.AddComponent<YetiRangeAttack>();
                    break;
                }
            case YetiStateToSwitch.Defense:
                {
                    currentState = YetiStateToSwitch.Defense;
                    yetiState = gameObject.AddComponent<YetiDefense>();
                    break;
                }
            case YetiStateToSwitch.Death:
                {
                    //Death is managed in YetiHealth
                    DeactivateTempest();
                    dead = true;
                    currentState = YetiStateToSwitch.Death;
                    yetiFightTrigger.StartCombatEndingAnimation();
                    XMLManager.Instance.Save(false);
                    break;
                }
        }
    }

    public bool IsOutOfZone()
    {
        return outOfAscensionZone;
    }

    public void SetOutOfAscensionZone(bool isOut)
    {
        outOfAscensionZone = isOut;
    }

    public void ActivateHealthBar()
    {
        yetiHealth.ActivateHealthBar();
    }

    public void ActivateTempest()
    {
        snowGenerator.Stop();
        tempest.Play();
    }

    public void DeactivateTempest()
    {
        snowGenerator.Play();
        tempest.Stop();
    }

    public void SetStartFight()
    {
        ChangeYetiState(YetiStateToSwitch.RangeAttack);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Yeti"), LayerMask.NameToLayer("Ground"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Yeti"), LayerMask.NameToLayer("Wall"), false);
        fightStarted = true;
    }

    public bool GetStartFight()
    {
        return fightStarted;
    }

    //This function must stay in an always present class, otherwise, animator can't access it
    public void AnimatorDamagePlayer()
    {
        yetiSound.playPunch();
        Collider2D[] hitPlayer;
        hitPlayer = Physics2D.OverlapCircleAll(spriteRenderer.flipX ? attackPointRight.position : attackPointLeft.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>()?.HitByYetiAttack(damage, transform.position.x > player.transform.position.x);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointLeft == null || attackPointRight == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
    }

    private void SaveYeti(bool switchScene)
    {
        XMLManager.Instance.savedData.yetiInfo.isOutOfAscensionZone = outOfAscensionZone;
        XMLManager.Instance.savedData.yetiInfo.dead = dead;
    }

    private void LoadYeti(bool loadPosition, bool playerDead)
    {
        outOfAscensionZone = XMLManager.Instance.savedData.yetiInfo.isOutOfAscensionZone;
        dead = XMLManager.Instance.savedData.yetiInfo.dead;

        if (dead)
        {
            gameObject.SetActive(false);
            yetiFightTrigger.SetActivated();
            return;
        }
        if (outOfAscensionZone)
        {
            yetiShield.Activate();
            yetiJump.SetPositionToLastPoint();
        }

        Destroy(yetiState);
        yetiJump.SetOutOfAscensionZone(outOfAscensionZone);
        yetiHealth.DeactivateHealthBar();
        yetiHealth.RestoreLife();
        yetiFightTrigger.ResetCombatState();
        ResetAnimatorParameters();
    }

    private void ResetAnimatorParameters()
    {
        animator.SetBool(Harmony.AnimatorParameters.IsJumping, false);
        animator.SetBool(Harmony.AnimatorParameters.IsFalling, false);
        animator.SetBool(Harmony.AnimatorParameters.IsRunning, false);
    }
}
