using UnityEngine;

public class GhostProjectile : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    [SerializeField] private int maxMunitions = 10;
    [SerializeField] private float defaultFireTimer = 10f;
    [SerializeField] private float distanceToDeactivateProjectile = 30;
    [SerializeField] private Rigidbody2D projectile;

    private Animator animator;
    private AnimatorClipInfo[] animatorinfo;
    private Rigidbody2D[] munitions;
    private Transform playerTransform;
    private Vector3 projectileDirection;

    private string current_animation;
    private float projectileAngle;
    private float projectileDistanceFromLaunchPosition;
    private float fireTimer;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        munitions = new Rigidbody2D[maxMunitions];
        fireTimer = 0;
        for (int i = 0; i < maxMunitions; i++)
        {
            munitions[i] = Instantiate(projectile);
            munitions[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        UpdateProjectiles();
        DeactivateProjectiles();
    }

    private void DeactivateProjectiles()
    {
        for (int i = 0; i < maxMunitions; i++)
        {
            if (munitions[i].gameObject.activeInHierarchy)
            {
                projectileDistanceFromLaunchPosition = Vector2.Distance(gameObject.transform.position, munitions[i].transform.position);
                if (projectileDistanceFromLaunchPosition >= distanceToDeactivateProjectile)
                {
                    Rigidbody2D rb = munitions[i].gameObject.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                    munitions[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void UpdateProjectiles()
    {
        if (fireTimer > 0f)
            fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);
            current_animation = animatorinfo[0].clip.name;
            if (current_animation == Harmony.AnimatorStates.Ghost_attack)
            {
                FireProjectile();
                fireTimer = defaultFireTimer;
            }

        }
    }

    private void FireProjectile()
    {
        for (int i = 0; i < maxMunitions; i++)
        {
            if (!munitions[i].gameObject.activeInHierarchy)
            {
                projectileDirection = (playerTransform.position - gameObject.transform.position).normalized * speed;
                projectileAngle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;

                munitions[i].gameObject.SetActive(true);
                munitions[i].transform.position = gameObject.transform.position;
                munitions[i].transform.eulerAngles = new Vector3(180f, 0f, -projectileAngle);
                Rigidbody2D rb = munitions[i].gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(projectileDirection.x, projectileDirection.y);
                break;
            }
        }
    }
}
