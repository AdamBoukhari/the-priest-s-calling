using UnityEngine;

public class FireBreathCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            transform.parent.GetComponent<DemonStateFireBreath>().CollisionDetected(this);
        }
    }
}
