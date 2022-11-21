using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class YetiState : MonoBehaviour
{
    protected YetiFightStateManager yetiFightStateManager;
    protected YetiJump yetiJump;
    protected YetiSnowballAttack yetiSnowballAttack;
    protected YetiShield yetiShield;
    protected YetiHealth yetiHealth;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected GameObject player;


    void Awake()
    {
        yetiFightStateManager = GetComponent<YetiFightStateManager>();
        yetiJump = GetComponent<YetiJump>();
        yetiSnowballAttack = GetComponent<YetiSnowballAttack>();
        yetiShield = GetComponent<YetiShield>();
        yetiHealth = GetComponent<YetiHealth>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(Harmony.Tags.Player);
    }

    public abstract void Init();
    public abstract void DoInteractions();
    public abstract void ManageStateChange();
}
