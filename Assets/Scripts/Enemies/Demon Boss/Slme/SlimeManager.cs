using TMPro;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    public string BOSS_NAME = "Slime Guardian";
    public bool stage2Started;
    public bool allAddsInactive;

    [SerializeField] private float speed;
    [SerializeField] private GameObject[] addsStage2;
    [SerializeField] private TextMeshProUGUI counterEnemiesText;
    [SerializeField] private GameObject counterEnemiesCanvas;
    [SerializeField] private ParticleSystem part;


    private Transform playerTransform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private DemonManager demonManager;
    private SlimeDamage slimeDamage;
    private Vector3 initialPosition;

    private float distanceForMoveAwayWalls;
    private float distanceToPlayer;
    private float distanceToDetectPlayer;
    private bool dead = false;
    private bool neerLeftWall;
    private bool neerRightWall;
    private float wallDectectedTransformX;
    private bool stopMove;


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        neerLeftWall = false;
        neerRightWall = false;
        stopMove = false;
        distanceToDetectPlayer = 4f;
        distanceForMoveAwayWalls = 4f;
        demonManager = GetComponent<DemonManager>();
        stage2Started = false;
        allAddsInactive = false;
        slimeDamage = GetComponent<SlimeDamage>();
        slimeDamage.ActivateHealthBar();
        initialPosition = transform.position;
        DeactivateShield();
    }
    private void OnDisable()
    {
        Publisher.PushData -= LoadFight;
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadFight;
    }

    private void LoadFight(bool loadPosition, bool playerDead)
    {
        dead = XMLManager.Instance.savedData.bossFightInfo.dead;
        if (dead)
        {
            gameObject.SetActive(false);
            return;
        }
        ResetFight();
    }

    private void ResetStage2()
    {
        stage2Started = false;
        for (int i = 0; i < addsStage2.Length; i++)
        {
            if (addsStage2[i].activeInHierarchy)
            {
                addsStage2[i].SetActive(false);
            }
        }
        allAddsInactive = false;
    }

    internal void ResetFight()
    {
        demonManager.enabled = false;
        slimeDamage.enabled = true;
        ResetStage2();
        slimeDamage.DeactivateHealthBar();
        slimeDamage.RestoreLife();
        ResetAnimatorParameters();
        transform.position = initialPosition;
        counterEnemiesCanvas.SetActive(false);
        DeactivateShield();
        animator.Play(Harmony.AnimatorStates.Slime_idle);
    }

    private void ResetAnimatorParameters()
    {
        animator.SetBool(Harmony.AnimatorParameters.Slime_die, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_hit, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_move, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_transformation, false);
    }

    internal void CanMove()
    {
        stopMove = false;
    }

    internal void StopMove()
    {
        stopMove = true;
    }

    void Update()
    {
        if (!allAddsInactive)
            CheckIfAddsIsInactive();

        if (!stopMove)
        {
            if (!neerRightWall && !neerLeftWall)
            {
                distanceToPlayer = Vector2.Distance(transform.position, new Vector2(playerTransform.position.x, transform.position.y));
                if (distanceToDetectPlayer >= distanceToPlayer)
                {
                    Move();
                }
                else
                {
                    animator.SetBool(Harmony.AnimatorParameters.Slime_move, false);
                }
            }
            else
            {
                MoveAwayWalls();
            }
        }
    }

    private void CheckIfAddsIsInactive()
    {
        if (!stage2Started)
            return;
        allAddsInactive = true;
        for (int i = 0; i < addsStage2.Length; i++)
        {
            if (addsStage2[i].activeInHierarchy)
            {
                allAddsInactive = false;
                break;
            }
        }
        if (!allAddsInactive)
        {
            int counter = 0;
            for (int i = 0; i < addsStage2.Length; i++)
            {
                if (addsStage2[i].activeInHierarchy)
                {
                    counter++;
                }
            }
            counterEnemiesText.text = counter.ToString();
        }
        if (allAddsInactive)
        {
            counterEnemiesCanvas.SetActive(false);
            DeactivateShield();
            stopMove = false;
        }
    }

    private void MoveAwayWalls()
    {
        if (neerLeftWall)
        {

            spriteRenderer.flipX = true;
            transform.position = MoveTowardsTarget(new Vector3(wallDectectedTransformX - distanceForMoveAwayWalls, transform.position.y, transform.position.z));
            if (transform.position.x >= (wallDectectedTransformX + distanceForMoveAwayWalls))
            {
                neerLeftWall = false;
            }
        }
        if (neerRightWall)
        {
            spriteRenderer.flipX = false;
            transform.position = MoveTowardsTarget(new Vector3(wallDectectedTransformX + distanceForMoveAwayWalls, transform.position.y, transform.position.z));

            if (transform.position.x <= (wallDectectedTransformX - distanceForMoveAwayWalls))
            {
                neerRightWall = false;
            }
        }
    }

    internal void StartStage2()
    {
        demonManager.enabled = false;
        stage2Started = true;
        animator.Play(Harmony.AnimatorStates.Slime_idle); GetComponent<SlimeDamage>().ActivateHealthBar();
        for (int i = 0; i < addsStage2.Length; i++)
        {
            if (!addsStage2[i].activeInHierarchy)
            {
                addsStage2[i].SetActive(true);
            }
        }
        counterEnemiesCanvas.SetActive(true);
        counterEnemiesText.text = addsStage2.Length.ToString();
        ActivateShield();
    }

    private void Move()
    {
        animator.SetBool(Harmony.AnimatorParameters.Slime_move, true);
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        transform.position = MoveTowardsTarget(playerTransform.position);
    }

    private Vector2 MoveTowardsTarget(Vector3 target)
    {
        float direction = Mathf.Sign(target.x - transform.position.x);
        return new Vector2(
             transform.position.x + direction * -speed / 1.2f * Time.deltaTime,
             transform.position.y
        );
    }

    private void IsTransformed()
    {
        demonManager.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.MinLimitX) && !neerLeftWall)
        {
            neerLeftWall = true;
            SetWallPosition();
        }

        if (collision.gameObject.CompareTag(Harmony.Tags.MaxLimitX) && !neerRightWall)
        {
            neerRightWall = true;
            SetWallPosition();
        }
    }

    private void SetWallPosition()
    {
        wallDectectedTransformX = transform.position.x;
    }

    public void ActivateShield()
    {
        part.Play();
    }

    public void DeactivateShield()
    {
        part.Stop();
    }
}
