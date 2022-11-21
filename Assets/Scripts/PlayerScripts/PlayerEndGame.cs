using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Harmony.Tags.EndGame)
        {
            GameManager.Instance.ManageEndGame();
        }
    }
}
