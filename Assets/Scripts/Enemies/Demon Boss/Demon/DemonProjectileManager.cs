using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectileManager : MonoBehaviour
{
	[SerializeField] private int speed = 10;
	[SerializeField] private int maxMunitions = 4;
	[SerializeField] private float defaultFireTimer = 10f;
	[SerializeField] private float distanceToDeactivateProjectile = 30;
	[SerializeField] private Rigidbody2D projectile;

	private Animator animator;
	AnimatorClipInfo[] animatorinfo;
	string current_animation;
	private Vector3 projectileDirection;
	private float projectileDistanceFromLaunchPosition;
	private Rigidbody2D[] munitions;
	private Transform playerTransform;
	private float fireTimer;
	private int fireProjectileInOneAttack;
	private float[] projectileAngles;
	private int currentAngle;
	private float projectileAngle;

	void Awake()
	{
		playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
		animator = GetComponent<Animator>();
		munitions = new Rigidbody2D[maxMunitions];
		fireTimer = 0;
		fireProjectileInOneAttack = 3;
		for (int i = 0; i < maxMunitions; i++)
		{
			munitions[i] = Instantiate(projectile);
			munitions[i].gameObject.SetActive(false);
		}
		projectileAngles = new float[fireProjectileInOneAttack];
		projectileAngles[0] = -0.20f;
		projectileAngles[1] = 0.9f;
		projectileAngles[2] = 1.90f;
		currentAngle = 0;
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
			if (current_animation == Harmony.AnimatorStates.Demon_Spell)
			{
				for(int i=0; i< fireProjectileInOneAttack; i++)
                {
					currentAngle = i;
					FireProjectile();
				}
				
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
				munitions[i].transform.eulerAngles = new Vector3(180f, 0f, projectileAngle);

				Rigidbody2D rb1 = munitions[i].gameObject.GetComponent<Rigidbody2D>();
				rb1.velocity = new Vector2(projectileDirection.x , projectileDirection.y + projectileAngles[currentAngle]);
				break;
			}
		}
	}
}
