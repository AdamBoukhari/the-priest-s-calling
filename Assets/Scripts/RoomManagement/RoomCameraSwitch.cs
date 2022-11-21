using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject vistualCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player)&& collision.isTrigger)
        {
            vistualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player) && collision.isTrigger)
        {
            vistualCamera.SetActive(false);
            XMLManager.Instance.Save(false);
        }
    }
}
