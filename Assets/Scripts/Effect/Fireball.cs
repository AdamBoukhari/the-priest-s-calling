using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fireball : MonoBehaviour
{
    private const int FIREBALL_INDEX = 2;
    [SerializeField] private float speed = 10;
    [SerializeField] private float damage = 2;
    private float vOrientation=1;
    private bool hit = false;

    private Animator animator;
    private SpriteRenderer sprite;
    private List<string> ignoreTags = new List<string>();

    private float lifetime = 15;

    void Awake()
    {
        //Todo: modifier les collisions avec les layers
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        ignoreTags.Add(Harmony.Tags.Trigger);
        ignoreTags.Add(Harmony.Tags.Player);
        ignoreTags.Add(Harmony.Tags.CameraZone);
        ignoreTags.Add(Harmony.Tags.Interaction);
        ignoreTags.Add(Harmony.Tags.Collectible);
        ignoreTags.Add(Harmony.Tags.Heart);
        ignoreTags.Add(Harmony.Tags.Button);
        ignoreTags.Add(Harmony.Tags.DashReset);
        ignoreTags.Add(Harmony.Tags.Prison);
        ignoreTags.Add(Harmony.Tags.Cavern);
        ignoreTags.Add(Harmony.Tags.Village);
        ignoreTags.Add(Harmony.Tags.Mountain);
        ignoreTags.Add(Harmony.Tags.Castle);
        Publisher.PushData += LoadFireball;
        Publisher.FetchData += SaveFireball;
    }

    private void OnEnable()
    {
        hit = false;

        lifetime = 15;
    }

    void Update()
    {
        if(!hit)
            transform.position = new Vector2(transform.position.x+speed*Time.deltaTime*vOrientation, transform.position.y);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            gameObject.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeFireballDamage(float value)
    {
        damage += value;
    }

    public void UpgradeFireballSpeed(float value)
    {
        speed *= 1 + (value / 100);
    }

    public void SetOrientation(bool flip)
    {
        sprite.flipX = flip;
        if (flip)
        {
            vOrientation = -1;
        }
        else
        {
            vOrientation = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ShouldDestroy(collision.tag) || hit)
        {
            return;
        }

        hit = true;
        animator.SetTrigger(Harmony.AnimatorParameters.Hit);
        if (collision.CompareTag(Harmony.Tags.BrokenWall))
        {
            collision?.GetComponent<BrokenWall>()?.destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //Todo : Refactor
            SlimeDamage slimeDamage = collision.GetComponent<SlimeDamage>();
            if (slimeDamage && slimeDamage.isActiveAndEnabled)
            {
                slimeDamage.TakeDamage();
            }
            DemonHealth demonHealth = collision.GetComponent<DemonHealth>();
            if (demonHealth && demonHealth.isActiveAndEnabled)
            {
                demonHealth.TakeDamage(damage);
            }

            collision?.GetComponent<LifeManager>()?.LoseLife(damage, transform.position.x > collision.transform.position.x);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Yeti"))
        {
            collision?.GetComponent<YetiHealth>()?.LoseLife(damage, transform.position.x > collision.transform.position.x);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Necromancer"))
        {
            collision?.GetComponent<LifeManager>()?.LoseLife(damage, transform.position.x > collision.transform.position.x);
        }
        else if (collision.CompareTag(Harmony.Tags.Bag))
        {
            collision?.GetComponent<Bag>()?.Fall();
        }
        else if (collision.gameObject.CompareTag(Harmony.Tags.Box))
        {
            collision?.GetComponent<PushableObject>().Reset();
        }
        else if (collision.gameObject.CompareTag(Harmony.Tags.GhostProjectile))
        {
            collision.gameObject.SetActive(false);
        }
    }

    private bool ShouldDestroy(string tag)
    {
        return !ignoreTags.Contains(tag);
    }

    private void SaveFireball(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].speed = speed;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].damage = damage;
    }

    private void LoadFireball(bool changeScene, bool playerDead)
    {
        speed = XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].speed;
        damage = XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].damage;
    }
}
