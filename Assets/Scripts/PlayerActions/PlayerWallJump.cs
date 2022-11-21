using UnityEngine;
using Harmony;

public class PlayerWallJump : MonoBehaviour
{
    [SerializeField] private float wallSlideSpeed = 3f;
    [SerializeField] private float wallJumpHorizontalSpeed = 7f;
    [SerializeField] private float wallJumpVerticalSpped = 6f;
    [SerializeField] private float wallJumpDuration = 0.4f;

    private PlayerMovement movementScript;
    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;
    private LayerMask wallLayer;

    private bool isOnWall = false;
    private bool isGrounded = false;
    private bool isWallJumping = false;
    private float wallJumpTimer = 0;
    private int wallJumpDirection = 0; // 1 is to the right, -1 is to the left
    private float raycastLength = 0.41f;

    private Vector3 RAYCAST_CALCULATION_OFFSET = new Vector3(0, 0.8f, 0);

    //Sound
    private AudioSource audioSource;

    void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        wallLayer = Layers.Wall;

        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        HandleCollisions();
        HandleWallJump();
    }

    private void HandleCollisions()
    {
        isOnWall = IsTouchingWall();
        animator.SetBool(AnimatorParameters.IsOnWall, isOnWall);
        isGrounded = movementScript.IsGrounded();

        if (CannotJump())
        {
            isOnWall = false;
            animator.SetBool(AnimatorParameters.IsOnWall, false);
        } 
        // Player is on the wall
        else if (isOnWall && !isWallJumping && !isGrounded)
        {
            animator.SetBool(AnimatorParameters.IsOnWall, true);
            movementScript.ResetAerialSpeed();
            wallJumpDirection = -GetWallDirection();

            // Sprite flipping
            if(wallJumpDirection < 0)
            {
                sprite.flipX = false;
            } 
            else
            {
                sprite.flipX = true;
            }

            // Stops the jittering on a wall
            if(movementScript.GetPlayerDirection() == 0)
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }

        }
        // Player is wallJumping
        else if(isWallJumping && !isOnWall && !isGrounded)
        {
            animator.SetBool(AnimatorParameters.IsOnWall, false);
            animator.SetBool(AnimatorParameters.IsWallJumping, true);

            if (wallJumpDirection > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }

        // Resets walljump animation
        if(isGrounded || isOnWall)
        {
            animator.SetBool(AnimatorParameters.IsWallJumping, false);
        }
    }

    private bool CannotJump()
    {
        // Player cannot jump if he is both on the wall and on the ground, simply on the ground or is simply not on a wall
        return ((isOnWall && isGrounded) || isGrounded || (!isOnWall && !isWallJumping));
    }

    private void HandleWallJump()
    {
        // Player slowly slides while on the wall
        if (isOnWall && rigidBody.velocity.y <= 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Vector2.down.y * wallSlideSpeed);
        }

        // Player hits a different wall during the wall jump
        if (isWallJumping && isOnWall)
        {
            if (GetWallDirection() == wallJumpDirection)
            {
                isWallJumping = false;
                wallJumpTimer = 0;
                animator.SetBool(AnimatorParameters.IsWallJumping, false);
                animator.SetBool(AnimatorParameters.IsOnWall, true);
            }
        }

        // Player WallJumps
        if (isWallJumping)
        {
            wallJumpTimer += Time.deltaTime;

            if (wallJumpTimer > 0 && wallJumpTimer <= wallJumpDuration)
            {
                rigidBody.velocity = new Vector2(wallJumpDirection * wallJumpHorizontalSpeed, wallJumpVerticalSpped);
            }
            else
            {
                isWallJumping = false;
                wallJumpTimer = 0;
            }
        }

        // Player hits a different wall during the wall jump
        if(isWallJumping && isOnWall)
        {
            if(GetWallDirection() == wallJumpDirection)
            {
                isWallJumping = false;
                animator.SetBool(AnimatorParameters.IsWallJumping, false);
                animator.SetBool(AnimatorParameters.IsOnWall, true);
            }
        }
    }

    public void OnJumpPressed()
    {
        if (isOnWall && !movementScript.IsOnJumpableSurface())
        {
            movementScript.WallJumpUsed(wallJumpDirection); // Allows for a double jump if unlocked
            animator.SetBool(AnimatorParameters.IsWallJumping, true);
            animator.SetBool(AnimatorParameters.IsOnWall, false);
            isWallJumping = true;

            audioSource.PlayOneShot(Sounds.wallJumpSnd);
        }
    }

    // -1 -> Wall to the left; 0 -> No collisions; 1 -> Wall to the right
    private int GetWallDirection()
    {
        return gameObject.GetLayerDirection(wallLayer, transform.position + RAYCAST_CALCULATION_OFFSET, raycastLength);
    }

    private bool IsTouchingWall()
    {
        int hit = GetWallDirection();
        return hit == 1 || hit == -1;
    }

    public bool IsOnWall()
    {
        return isOnWall;
    }

    public bool IsWallJumping()
    {
        return isWallJumping;
    }
}
