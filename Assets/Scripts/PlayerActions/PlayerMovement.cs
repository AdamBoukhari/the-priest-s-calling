using UnityEngine;
using UnityEngine.InputSystem;
using Harmony;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private const float MAX_HOLD_JUMP_TIME = 0.25f;
    private const float COYOTE_TIME = 0.1f;
    private const int DOUBLE_JUMP_INDEX = 3;
    private const float DOWNWARD_RAYCAST_LENGTH = 0.3f;
    private const float RAYCAST_FEET_OFFSET = 0.1f;
    private Vector3 RAYCAST_POSITION_OFFSET = new Vector3(0, 0.75f, 0);
    private Vector3 RAYCAST_OFFSET = new Vector3(0, 0.7f, 0);
    private const float SLOPE_ANGLE_THRESHOLD = 0.1f;
    private const float AERIAL_SPEED_BUFF_TIME = 0.75f;

    [SerializeField] private float groundMaxSpeed = 5.5f;
    [SerializeField] private float slopeSpeed = 7f;
    [SerializeField] private float accelerationTime = 0.25f;
    [SerializeField] private float deccelerationTime = 0.2f;
    [SerializeField] private int jumpForce = 8;
    [SerializeField] private float maxVerticalSpeed = 75f;
    [SerializeField] private bool doubleJumpObtained = false;

    // Components
    private PlayerWallJump wallJumpScript;
    private PlayerDash dashScript;
    private PlayerSwordAttack swordScript;
    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;
    private Publisher publisher;
    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private LayerMask slopeLayer;
    private LayerMask boxLayer;
    private LayerMask platformLayer;
    private AudioSource audioSource;


    // Movements
    private float direction = 0;
    private float lastFrameDirection = 0;
    private float currentSpeed = 0;
    private float speedIncrement = 0;
    private bool groundToTheRight = false;
    private bool groundToTheLeft = false;
    private bool wallToTheRight = false;
    private bool wallToTheLeft = false;
    private float aerialMaxSpeed;
    private float speedsDiff;
    private bool isGrounded;

    // Jumps
    private bool isHoldingJumpButton = false;
    private float holdJumpTimer = 0;
    private int numJumps = 0;
    private bool hasRedemptionJump = false;
    private float coyoteTimer = COYOTE_TIME;
    private bool coyoteTimerActivated = false;
    private float raycastLength = 0.4f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        wallJumpScript = GetComponent<PlayerWallJump>();
        dashScript = GetComponent<PlayerDash>();
        swordScript = GetComponent<PlayerSwordAttack>();
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        publisher = GetComponent<Publisher>();
        groundLayer = Layers.Ground;
        wallLayer = Layers.Wall;
        slopeLayer = Layers.Slope;
        boxLayer = Layers.Box;
        platformLayer = Layers.Platform;

        aerialMaxSpeed = groundMaxSpeed;
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadUpgrades;
        Publisher.FetchData += SaveUpgrades;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadUpgrades;
        Publisher.FetchData -= SaveUpgrades;
    }

    private void Update()
    {
        HandleCollisions();
        HandleMovement();
        HandleJump();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        // Stops player from falling too fast
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVerticalSpeed);
    }


    private void HandleCollisions()
    {
        isGrounded = IsOnJumpableSurface();

        // Player is standing on the ground
        if (rigidBody.velocity.y <= 0 && isGrounded && numJumps != 0 && !isHoldingJumpButton)
        {
            ResetJump();
        }

        // Checks for possible obstacles while in the air
        if (!isGrounded)
        {
            // Ground Layer
            int groundDirection = gameObject.GetLayerDirectionPrecisely(groundLayer, transform.position + RAYCAST_POSITION_OFFSET, RAYCAST_OFFSET, raycastLength);
            if (groundDirection == 1)
            {
                groundToTheRight = true;
                groundToTheLeft = false;
            }
            else if (groundDirection == -1)
            {
                groundToTheLeft = true;
                groundToTheRight = false;
            }
            else
            {
                groundToTheLeft = false;
                groundToTheRight = false;
            }

            // Wall Layer
            int wallDirection = gameObject.GetLayerDirectionPrecisely(wallLayer, transform.position + RAYCAST_POSITION_OFFSET, RAYCAST_OFFSET, raycastLength);
            if (wallDirection == 1)
            {
                wallToTheRight = true;
                wallToTheLeft = false;
            }
            else if (wallDirection == -1)
            {
                wallToTheLeft = true;
                wallToTheRight = false;
            }
            else
            {
                wallToTheLeft = false;
                wallToTheRight = false;
            }
        }
        else // Check for possible obstacles while on the ground
        {
            groundToTheLeft = false;
            groundToTheRight = false;
            wallToTheLeft = false;
            wallToTheRight = false;

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + RAYCAST_FEET_OFFSET), Vector2.down, DOWNWARD_RAYCAST_LENGTH, slopeLayer);

            // Is on a slope
            if (hit.collider != null && Mathf.Abs(hit.normal.x) > SLOPE_ANGLE_THRESHOLD && rigidBody.gravityScale != 0)
            {
                float angle = Mathf.Abs(180 - Vector2.Angle(hit.normal, Vector2.down));

                float heightDirection = 1;
                if(rigidBody.velocity.y < 0)
                {
                    heightDirection = -1; ;
                }

                rigidBody.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * direction * slopeSpeed, Mathf.Sin(angle * Mathf.Deg2Rad) * heightDirection * aerialMaxSpeed);
            }
        }

        // Stops player from sliding down a slope when not moving
        if(isGrounded && direction == 0)
        {
            rigidBody.gravityScale = 0;
        }
        else if((direction != 0 || isHoldingJumpButton || !isGrounded) && !dashScript.IsDashing())
        {
            rigidBody.gravityScale = 1;
        }
    }

    private void HandleMovement()
    {
        // Cannot move while performing those actions
        if (dashScript.IsDashing() || wallJumpScript.IsWallJumping() || (isHoldingJumpButton && holdJumpTimer <= MAX_HOLD_JUMP_TIME && direction == 0))
        {
            return;
        }

        // Air movement
        if (!isGrounded)
        {
            if (IsAccelerating())
            {
                speedIncrement = aerialMaxSpeed * Time.deltaTime / accelerationTime * direction;
            }
            else
            {
                // Slowly brings the speed back to zero
                if (currentSpeed > 0)
                {
                    speedIncrement = -(aerialMaxSpeed * Time.deltaTime / deccelerationTime);
                }
                else if (currentSpeed < 0)
                {
                    speedIncrement = aerialMaxSpeed * Time.deltaTime / deccelerationTime;
                }
            }

            currentSpeed += speedIncrement;

            // Keeps the speed in the limits
            currentSpeed = Mathf.Clamp(currentSpeed, -aerialMaxSpeed, aerialMaxSpeed);

            //Dash cancel speed decreaser
            if(aerialMaxSpeed > groundMaxSpeed)
            { 
                aerialMaxSpeed -= (speedsDiff * Time.deltaTime) / AERIAL_SPEED_BUFF_TIME;

                if(aerialMaxSpeed < groundMaxSpeed)
                {
                    ResetAerialSpeed();
                }
            }
        }
        else
        {
            dashScript.ResetDash();
            currentSpeed = direction * groundMaxSpeed;
        }

        // Stops player from getting stuck on the edge of wall
        if (IsOnTheEdgeOfAWall())
        {
            currentSpeed = 0;
        }

        rigidBody.velocity = new Vector2(currentSpeed, rigidBody.velocity.y);

        lastFrameDirection = direction;
    }

    private bool IsAccelerating()
    {
        // Player accelerates if he keeps going the same way or starts moving.
        return (lastFrameDirection > 0 && direction > 0) || (lastFrameDirection < 0 && direction < 0);
    }

    private bool IsOnTheEdgeOfAWall()
    {
        return (!isGrounded && !wallJumpScript.IsOnWall() && ((groundToTheRight || wallToTheRight) && direction > 0) || ((groundToTheLeft || wallToTheLeft) && direction < 0));
    }

    private void HandleJump()
    {
        // Player just fell off the ground (no jump pressed yet)
        if (!isHoldingJumpButton && !isGrounded)
        {
            coyoteTimerActivated = true;
        }

        if (coyoteTimerActivated)
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (coyoteTimer <= 0 && numJumps == 0)
        {
            hasRedemptionJump = true;
        }

        // Ramps up height as long as the player is holding jump
        if (isHoldingJumpButton && holdJumpTimer <= MAX_HOLD_JUMP_TIME)
        {
            holdJumpTimer += Time.deltaTime;

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);

            // Stops the player from moving left and right if he is jumping without moving
            if (direction == 0)
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
    }

    private void HandleAnimations()
    {
        // Flips sprite depending on the direction
        if (direction < 0)
        {
            sprite.flipX = true;
        }
        else if (direction > 0)
        {
            sprite.flipX = false;
        }

        animator.SetFloat(AnimatorParameters.Speed, Mathf.Abs(direction));
        animator.SetFloat(AnimatorParameters.yVelocity, rigidBody.velocity.y);
        animator.SetBool(AnimatorParameters.IsGrounded, isGrounded);
    }

    public void OnMove(InputValue input)
    {
        Vector2 currentInput = input.Get<Vector2>();
        direction = currentInput.x;
    }

    public void OnJumpPressed()
    {
        // Actions reserved for the walljump script or unavailable at the beggining of the game
        if (wallJumpScript.IsWallJumping() || wallJumpScript.IsOnWall() || !swordScript.IsSwordUnlocked())
        {
            return;
        }

        if (CanJump())
        {
            ManageDashCancelAerialSpeed();

            numJumps = 1;
            isHoldingJumpButton = true;
            audioSource.PlayOneShot(Sounds.jumpSnd);
        }
        else if (CanDoubleJump() || CanRedemptionJump())
        {
            ManageDashCancelAerialSpeed();

            hasRedemptionJump = false;
            numJumps = 2;
            isHoldingJumpButton = true;
            audioSource.PlayOneShot(Sounds.doubleJumpSnd);
        }
    }

    private void ManageDashCancelAerialSpeed()
    {
        if (dashScript.IsDashing() && isGrounded)
        {
            aerialMaxSpeed = dashScript.GetCurrentDashSpeed();
            speedsDiff = aerialMaxSpeed - groundMaxSpeed;
        }
    }

    private bool CanJump()
    {
        return (IsOnJumpableSurface() || coyoteTimer > 0) && numJumps == 0;
    }

    private bool CanDoubleJump()
    {
        return doubleJumpObtained && numJumps == 1 && !CanJump();
    }

    // Redemption jump is when the player falls off the ledge or walljumps, but can still do a final jump due to the double jump
    private bool CanRedemptionJump()
    {
        return doubleJumpObtained && numJumps == 0 && hasRedemptionJump;
    }

    private void ResetJump()
    {
        numJumps = 0;
        isHoldingJumpButton = false;
        coyoteTimerActivated = false;
        coyoteTimer = COYOTE_TIME;
        publisher.CallPlayerLandingEvent();
        ResetAerialSpeed();
    }

    public void ResetAerialSpeed()
    {
        aerialMaxSpeed = groundMaxSpeed;
    }

    public void OnJumpReleased()
    {
        isHoldingJumpButton = false;
        holdJumpTimer = 0;
    }

    public void WallJumpUsed(int direction)
    {
        hasRedemptionJump = true;
        lastFrameDirection = direction;
        currentSpeed = direction * groundMaxSpeed;
    }

    public bool IsOnJumpableSurface()
    {
         // Starts the vector at the player's feet
        Vector2 position = new Vector2(transform.position.x, transform.position.y + RAYCAST_FEET_OFFSET);

        bool hitGround = gameObject.IsStandingOnThisLayer(groundLayer, position, DOWNWARD_RAYCAST_LENGTH);
        bool hitBox = gameObject.IsStandingOnThisLayer(boxLayer, position, DOWNWARD_RAYCAST_LENGTH);
        bool hitPlatform = gameObject.IsStandingOnThisLayer(platformLayer, position, DOWNWARD_RAYCAST_LENGTH);
        bool hitSlope = gameObject.IsStandingOnThisLayer(slopeLayer, position, DOWNWARD_RAYCAST_LENGTH);

        // Use this line for debugging if player scale is changed and raycast length needs to be changed.
        //Debug.DrawRay(position, direction, Color.green);

        if (hitGround || hitBox || hitPlatform || hitSlope)
        {
            return true;
        }

        return false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetPlayerDirection()
    {
        return direction;
    }

    public void UnlockDoubleJump()
    {
        doubleJumpObtained = true;
    }

    public bool DoubleJumpObtained()
    {
        return doubleJumpObtained;
    }

    public void StopPlayer()
    {
        direction = 0;
        rigidBody.velocity = Vector2.zero;
    }

    public void UpgradeMovementSpeed(float value)
    {
        groundMaxSpeed *= 1 + (value / 100);
        aerialMaxSpeed *= 1 + (value / 100);
    }

    private void SaveUpgrades(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[DOUBLE_JUMP_INDEX].acquired = doubleJumpObtained;
        XMLManager.Instance.savedData.playerInfo.moveSpeed = groundMaxSpeed;
    }

    private void LoadUpgrades(bool loadPosition, bool playerDead)
    {
        doubleJumpObtained = XMLManager.Instance.savedData.playerInfo.listOfCapacities[DOUBLE_JUMP_INDEX].acquired;
        groundMaxSpeed = XMLManager.Instance.savedData.playerInfo.moveSpeed;
    }
}
