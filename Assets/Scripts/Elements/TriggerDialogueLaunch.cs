using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueLaunch : MonoBehaviour
{
    private DialogueTrigger dialogue;

    private void Start()
    {
        dialogue = GetComponent<DialogueTrigger>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            dialogue.TriggerDialogue(collision.gameObject);
            StartCoroutine(disable());
        }
    }

    private IEnumerator disable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
