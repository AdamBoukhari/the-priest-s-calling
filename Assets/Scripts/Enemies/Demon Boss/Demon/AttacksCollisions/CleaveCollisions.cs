using UnityEngine;

public class CleaveCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            transform.parent.GetComponent<DemonStateCleave>().CollisionDetected(this);
        }
    }
}
