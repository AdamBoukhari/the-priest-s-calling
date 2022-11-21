using UnityEngine;

public class SmashCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            transform.parent.GetComponent<DemonStateSmash>().CollisionDetected(this);
        }
    }
}
