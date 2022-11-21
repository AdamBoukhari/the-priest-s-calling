using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiSnowballAttack : MonoBehaviour
{
    [SerializeField] private GameObject snowballPrefab;
    private GameObject player;
    private YetiJump yetiJump;
    private YetiFightStateManager yetiFightStateManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const int NB_SNOWBALLS = 10;
    private const float SNOWBALL_TRAJECTORY_DURATION_ASCENSION = 1.5f;
    private const float SNOWBALL_TRAJECTORY_DURATION_COMBAT = .8f;
    private const float THROW_COOLDOWN_ASCENSION = 2.5f;
    private List<GameObject> snowballs = new List<GameObject>();
    private GameObject snowball;
    private float cooldownCount = 0;
    private bool activated = false;

    private Vector3 playerOffset;
    private float playerOffsetMaxValue = 2f;

    private YetiSound yetiSound;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Harmony.Tags.Player);
        yetiJump = GetComponent<YetiJump>();
        yetiFightStateManager = GetComponent<YetiFightStateManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        yetiSound = GetComponent<YetiSound>();

        for (int i = 0; i < NB_SNOWBALLS; i++)
        {
            GameObject snowball = Instantiate(snowballPrefab);
            snowball.SetActive(false);
            snowballs.Add(snowball);
        }
    }

    void Update()
    {
        if (cooldownCount < 0 && activated)
        {
            ThrowSnowballAscensionPhase();
        }

        cooldownCount -= Time.deltaTime;
    }

    public void SetYetiActivated()
    {
        activated = true;
    }

    public void AnimatorThrowSnowball()
    {
        yetiSound.playThrow();
        snowball.transform.position = transform.position + Vector3.up;
        snowball.SetActive(true);
        float flightTime = yetiFightStateManager.GetStartFight() ? SNOWBALL_TRAJECTORY_DURATION_COMBAT : SNOWBALL_TRAJECTORY_DURATION_ASCENSION;
        snowball.GetComponent<Rigidbody2D>().velocity = ParabolaComputer.GetRBImpulse(snowball.transform.position, player.transform.position + playerOffset, flightTime);
    }

    private void ThrowSnowballAscensionPhase()
    {
        if (!yetiJump.PlayerIsInYetiZone() || yetiJump.JumpStarted() || yetiFightStateManager.IsOutOfZone())
        {
            return;
        }

        snowball = GetNextSnowball();
        if (snowball != null)
        {
            SetYetiSpriteFlip(gameObject.transform.position, player.transform.position);
            animator.SetTrigger(Harmony.AnimatorParameters.ThrowSnow);
            playerOffset = Vector3.zero;
            cooldownCount = THROW_COOLDOWN_ASCENSION;
        }
    }

    public void ThrowSnowballCombatPhase()
    {
        snowball = GetNextSnowball();
        if (snowball != null)
        {
            SetYetiSpriteFlip(gameObject.transform.position, player.transform.position);
            animator.SetTrigger(Harmony.AnimatorParameters.ThrowSnow);
            playerOffset = new Vector3(Random.Range(-playerOffsetMaxValue, playerOffsetMaxValue), Random.Range(-playerOffsetMaxValue, playerOffsetMaxValue), 0);
        }
    }

    private GameObject GetNextSnowball()
    {
        foreach (GameObject snowball in snowballs)
        {
            if (!snowball.activeInHierarchy)
            {
                return snowball;
            }
        }
        return null;
    }

    private void SetYetiSpriteFlip(Vector3 yetiPos, Vector3 otherPos)
    {
        spriteRenderer.flipX = yetiPos.x - otherPos.x < 0 ? true : false;
    }
}
