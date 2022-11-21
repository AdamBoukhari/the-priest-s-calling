using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private const int DASH_INDEX = 1;

    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float distanceBetweenImages = 0.3f;
    [SerializeField] private bool dashObtained = true;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;
    private CapsuleCollider2D playerCollider;
    private CircleCollider2D dashCollider;
    private PlayerWallJump playerWallJump;
    private AudioSource audioSource;

    private bool dashActivated = false;
    private bool dashAvailable = false;
    private float dashTimer = 0;
    private int dashDirection = 0;
    private float lastImageXpos;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        dashCollider = GetComponent<CircleCollider2D>();
        playerWallJump = GetComponent<PlayerWallJump>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadDash;
        Publisher.FetchData += SaveDash;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadDash;
        Publisher.FetchData -= SaveDash;
    }

    private void Update()
    {
        HandleDash();
    }

    private void HandleDash()
    {
        // Start
        if (dashActivated && dashTimer == 0)
        {
            rigidBody.gravityScale = 0;
        }

        if (dashActivated)
        {
            dashTimer += Time.deltaTime;
            rigidBody.velocity = new Vector2(dashDirection * dashSpeed, 0);

            if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }

        // End
        if (dashActivated && dashTimer >= dashDuration)
        {
            dashTimer = 0;
            dashActivated = false;
            rigidBody.gravityScale = 1;

            playerCollider.enabled = true;
            dashCollider.enabled = false;
        }
    }

    public void OnDash()
    {
        if (dashObtained && dashAvailable && !playerWallJump.IsWallJumping() && !playerWallJump.IsOnWall())
        {
            audioSource.PlayOneShot(Sounds.dash);
            dashActivated = true;
            dashAvailable = false;
            dashCollider.enabled = true;
            playerCollider.enabled = false;
            animator.SetTrigger(Harmony.AnimatorParameters.Dashed);
            GameManager.Instance.UpdateDashCoolDown(dashAvailable);

            // Checks for dash direction (1 is right, -1 is left)
            if (!sprite.flipX)
            {
                dashDirection = 1;
            }
            else
            {
                dashDirection = -1;
            }

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
    }

    public bool IsDashing()
    {
        return dashObtained && dashActivated;
    }

    public float GetCurrentDashSpeed()
    {
        return dashSpeed;
    }

    public void UnlockDash()
    {
        dashObtained = true;
        GameManager.Instance.ShowAbilityInUI(DASH_INDEX);
    }

    public bool DashObtained()
    {
        return dashObtained;
    }

    public void UpgradeRange(float value)
    {
        dashDuration *= 1 + (value / 100);
    }

    public void UpgradeSpeed(float value)
    {
        dashSpeed *= 1 + (value / 100);
    }

    public bool WasUsed()
    {
        return !dashAvailable;
    }

    public void ResetDash()
    {
        if(!dashAvailable)
        {
            dashAvailable = true;
            GameManager.Instance.UpdateDashCoolDown(dashAvailable);
        }
    }

    private void SaveDash(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].acquired = dashObtained;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].range = dashDuration;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].speed = dashSpeed;
    }

    private void LoadDash(bool loadPosition, bool playerDead)
    {
        dashObtained = XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].acquired;
        dashDuration = XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].range;
        dashSpeed = XMLManager.Instance.savedData.playerInfo.listOfCapacities[DASH_INDEX].speed;
    }
}
