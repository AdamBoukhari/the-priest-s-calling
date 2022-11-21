using UnityEngine;

public class EndlessPit : MonoBehaviour
{
    private const float PIT_DAMAGE = 1f;

    [SerializeField] private Transform defaulSpawnPoint;

    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = defaulSpawnPoint.position;
    }

    public void SetSpawnPoint(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakePlainDamage(PIT_DAMAGE);
            collision.transform.position = spawnPosition;
        } 
        else if (collision.CompareTag(Harmony.Tags.Box))
        {
            collision.gameObject.GetComponent<PushableObject>()?.Reset();
        }
    }
}
