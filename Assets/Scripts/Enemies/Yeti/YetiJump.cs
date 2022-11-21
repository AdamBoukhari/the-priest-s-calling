using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiJump : MonoBehaviour
{
    [SerializeField] private GameObject[] jumpPoints;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private CinemachineTargetGroup tgroup;
    private PlayerInteraction playerInteraction;
    private YetiShield yetiShield;
    private BoxCollider2D boxCollider2D;
    private YetiFightStateManager yetiFightStateManager;
    private int maxFollowDistance = 22;
    private int maxYAxisFollowDistance = 13;

    private GameObject lastJumpPoint;
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Queue<GameObject> queuedJumpPoints;

    private MusicAltern music;

    private const float ASCENSION_JUMP_DURATION = 1.5f;
    private const float FIGHT_JUMP_DURATION = 1f;
    private const float JUMP_CHARGE_DURATION = 0.35f;
    private const float WAKE_UP_DURATION = 2f;

    private bool woke = false; 
    [HideInInspector] public bool jumpStarted = false;
    [HideInInspector] public bool jumpCalled = false;
    [HideInInspector] public bool defensiveJump = false;
    private bool outOfAscensionZone = false;

    private YetiSound yetiSound;

    private Vector3 point1;
    private Vector3 point2;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Yeti"), LayerMask.NameToLayer("Ground"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Yeti"), LayerMask.NameToLayer("Wall"), true);

        player = GameObject.FindGameObjectWithTag(Harmony.Tags.Player);
        playerInteraction = player.GetComponent<PlayerInteraction>();
        yetiShield = GetComponent<YetiShield>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        yetiFightStateManager = GetComponent<YetiFightStateManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        queuedJumpPoints = new Queue<GameObject>();

        yetiSound = GetComponent<YetiSound>();

        music = GameObject.FindGameObjectWithTag(Harmony.Tags.MainCamera)?.GetComponent<MusicAltern>();
    }


    void Update()
    {
        if (jumpStarted && !outOfAscensionZone)
        {
            CheckForJumpLanding();
        }
        else if(!outOfAscensionZone)
        {
            CheckForCamOperations();
        }

        if(outOfAscensionZone && jumpStarted && defensiveJump)
        {
            CheckForJumpClip();
        }
    }

    public bool PlayerIsInYetiZone()
    {
        return vcam.isActiveAndEnabled;
    }

    public bool JumpStarted()
    {
        return jumpStarted;
    }

    public void SetOutOfAscensionZone(bool isOut)
    {
        outOfAscensionZone = isOut;
        yetiFightStateManager.SetOutOfAscensionZone(outOfAscensionZone);
        vcam.Follow = outOfAscensionZone ? player.transform : tgroup.transform;
    }

    public void CallJumpOnAscensionPosition(GameObject jumpPoint)
    {
        if (lastJumpPoint == jumpPoint)
        {
            return;
        }
        if (lastJumpPoint == jumpPoints[jumpPoints.Length - 1])
        {
            SetOutOfAscensionZone(false);
        }

        int lastJumpIndex = 0;
        int currentJumpIndex = 0;

        for (int i = 0; i < jumpPoints.Length; i++)
        {
            if (jumpPoints[i] == lastJumpPoint)
            {
                lastJumpIndex = i;
            }
            if (jumpPoints[i] == jumpPoint)
            {
                currentJumpIndex = i;
            }
        }

        //Jump is descending
        for (int i = lastJumpIndex - 1; i >= currentJumpIndex; i--)
        {
            queuedJumpPoints.Enqueue(jumpPoints[i]);

        }

        //Jump is ascending
        for (int i = lastJumpIndex; i <= currentJumpIndex; i++)
        {
            queuedJumpPoints.Enqueue(jumpPoints[i]);
        }

        playerInteraction.DeactivatePlayerControls();
        vcam.Follow = gameObject.transform;
        yetiSound.playGrowl();
        JumpToNextAscensionPoint();
    }

    private void JumpToNextAscensionPoint()
    {
        GameObject jumpPoint = queuedJumpPoints.Dequeue();
        lastJumpPoint = jumpPoint;
        InitializeJump(jumpPoint.transform.position, ASCENSION_JUMP_DURATION);
    }

    public void JumpToFightPoint(Vector3 jumpPoint)
    {
        InitializeJump(jumpPoint, FIGHT_JUMP_DURATION);
    }

    private void InitializeJump(Vector3 jumpPoint, float jumpDuration)
    {
        jumpCalled = true;
        jumpStarted = true;
        point1 = transform.position;
        point2 = jumpPoint;
        SetYetiSpriteFlip(point1, point2);
        StartCoroutine(ChargeJump(jumpDuration));
    }

    private IEnumerator ChargeJump(float jumpDuration)
    {
        if(!woke)
        {
            WakeUp();
            yield return new WaitForSeconds(WAKE_UP_DURATION);
            yetiShield.Activate();
        }

        animator.SetTrigger(Harmony.AnimatorParameters.StartJump);
        yield return new WaitForSeconds(JUMP_CHARGE_DURATION);

        yetiSound.playJump();
        animator.SetBool(Harmony.AnimatorParameters.IsFalling, false);
        animator.SetBool(Harmony.AnimatorParameters.IsJumping, true);
        rb.velocity = ParabolaComputer.GetRBImpulse(point1, point2, jumpDuration);
        jumpCalled = false;

        if (!outOfAscensionZone)
        {
            boxCollider2D.enabled = false;
        }
    }

    private void WakeUp()
    {
        woke = true;
        music.PlayBossMusic(true);
        animator.SetTrigger(Harmony.AnimatorParameters.Awaken);
    }

    private void SetYetiSpriteFlip(Vector3 yetiPos, Vector3 otherPos)
    {
        spriteRenderer.flipX = yetiPos.x - otherPos.x < 0 ? true : false;
    }

    private void CheckForJumpLanding()
    {
        CheckForFallAnimation();
        if(!CheckForJumpClip())
        {
            return;
        }

        if (queuedJumpPoints.Count == 0)
        {
            playerInteraction.ActivatePlayerControls();
            SetYetiSpriteFlip(point1, player.transform.position);
            SetOutOfAscensionZone(lastJumpPoint == jumpPoints[jumpPoints.Length - 1] ? true : false);
            XMLManager.Instance.Save(false);
        }
        else
        {
            JumpToNextAscensionPoint();
        }
    }

    private bool CheckForJumpClip()
    {
        point1 = transform.position;
        if (Vector3.Distance(point1, point2) > 1.5f)
        {
            return false;
        }
        transform.position = point2;
        rb.velocity = Vector3.zero;
        ResetJumpState();
        return true;
    }
    
    public void CheckForFallAnimation()
    {
        if (rb.velocity.y < 0)
        {
            animator.SetBool(Harmony.AnimatorParameters.IsFalling, true);
        }
    }

    public void ResetJumpState()
    {
        animator.SetBool(Harmony.AnimatorParameters.IsJumping, false);
        animator.SetBool(Harmony.AnimatorParameters.IsFalling, false);
        boxCollider2D.enabled = true;
        defensiveJump = false;
        jumpStarted = false;
        jumpCalled = false;
    }

    private void CheckForCamOperations()
    {
        Vector3 playerT = player.transform.position;
        Vector3 yetiT = gameObject.transform.position;
        if (Vector3.Distance(playerT, yetiT) > maxFollowDistance || Mathf.Abs(playerT.y - yetiT.y) > maxYAxisFollowDistance)
        {
            vcam.Follow = player.transform;
        }
        else
        {
            vcam.Follow = tgroup.transform;
        }
    }

    public void SetPositionToLastPoint()
    {
        WakeUp();
        GameObject finalPoint = jumpPoints[jumpPoints.Length - 1];
        transform.position = finalPoint.transform.position;
        lastJumpPoint = finalPoint;
    }
}
