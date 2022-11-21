using UnityEngine;

public class SaveBeforeFight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player))
        {
            XMLManager.Instance.Save(false);
        }
    }
}
