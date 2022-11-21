using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndGameDialogue : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Harmony.Tags.Player && XMLManager.Instance.savedData.darkhenBeaten)
        {
            dialogueTrigger.TriggerDialogue(collision.gameObject);
            XMLManager.Instance.savedData.darkhenBeaten = false;
        } 
    }
}
