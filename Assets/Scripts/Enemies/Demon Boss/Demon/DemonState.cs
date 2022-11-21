using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemonState : MonoBehaviour
{
    protected const float DefaultStageChangeTimer = 12f;
    protected const float DefaultFirebreathTimer = 3f;
    protected const float DefaultCleaveTimer = 1f;
    protected const float DefaultSpellTimer = 1.5f;
    protected const float DefaultSmashTimer = 2f;
    protected const float FleeDistance = 8f;
    protected const float FirebreathDistance = 5f;
    protected const float CleaveDistance = 4f;
    protected const float ChaseDistance = 3f;
    protected const float DistanceToAttacKPlayer = 7f;
    protected const float DistanceToRepulsePlayer = 4f;
    protected const float NormalHeightDistanceFromPlayer = 4.05f;
    protected const float DefaultFleeJumpTimer = 5f;


    protected DemonManager demonManager;
    protected DemonJump demonJump;
    protected Transform playerTransform;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected LayerMask groundLayer;

    protected float speed = 8f;
    protected float distanceToPlayer;
    protected float heightDistanceFromPlayer;
    protected bool demonFirebreathEnded;
    protected bool demonCleaveEnded;
    protected bool demonCastEnded;
    protected bool demonSmashEnded;
    protected bool neerLeftWall;
    protected bool neerRightWall;
    protected float wallDectectedTransformX;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        demonManager = GetComponent<DemonManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        demonJump = GetComponent<DemonJump>();
        groundLayer = Harmony.Layers.Ground;

        demonFirebreathEnded = true;
        demonCleaveEnded = true;
        demonCastEnded = true;
        demonSmashEnded = true;
    }

    public abstract void MoveDemon();
    public abstract void ManageStateChange();

    private void FirebreathIsEnded()
    {
        demonFirebreathEnded = true;
    }
    private void CleaveIsEnded()
    {
        demonCleaveEnded = true;
    }
    private void CastIsEnded()
    {
        demonCastEnded = true;
    }
    private void SmashIsEnded()
    {
        demonSmashEnded = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            PlayerHealth playerHP = collision.GetComponent<PlayerHealth>();
            bool left = transform.position.x < collision.transform.position.x;
            playerHP.TakeDamageKnockBack(0.5f, !left);
        }
    }
}
