using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogues.DialogueNames dialogueName;
    [SerializeField] private Dialogues.DialogueNames alternateDialogue = Dialogues.DialogueNames.NULL;
    [SerializeField] private Consts.Abilities ability; 
    [SerializeField] private List<GameObject> interactableObjects;

    public void TriggerDialogue(GameObject player)
    {
        DialogueEvent[] dialogueFrame;

        if (alternateDialogue != Dialogues.DialogueNames.NULL && TargetAbilityAlreadyUnlocked(player))
        {
            dialogueFrame = Dialogues.GetDialogueFromName(alternateDialogue);
        }
        else
        {
            dialogueFrame = Dialogues.GetDialogueFromName(dialogueName);
        }

        DialogueManager.Instance.StartDialogue(dialogueFrame, player, interactableObjects);
    }

    private bool TargetAbilityAlreadyUnlocked(GameObject player)
    {
        switch (ability)
        {
            case Consts.Abilities.SWORD:
                if (player.GetComponent<PlayerSwordAttack>().IsSwordUnlocked())
                {
                    return true;
                }
                break;

            case Consts.Abilities.DASH:
                if (player.GetComponent<PlayerDash>().DashObtained())
                {
                    return true;
                }
                break;

            case Consts.Abilities.DOUBLE_JUMP:
                //Multiple conditions because NPC has 2 alternate dialogues
                if(XMLManager.Instance.savedData.darkhenBeaten)
                {
                    alternateDialogue = Dialogues.DialogueNames.END_GAME_DIALOGUE_ALTERNATIVE;
                    return true;
                }
                else
                {
                    if (player.GetComponent<PlayerMovement>().DoubleJumpObtained())
                    {
                        alternateDialogue = Dialogues.DialogueNames.ALTERNATE_DOUBLE_JUMP_OBTENTION;
                        return true;
                    }
                    break;
                }

            case Consts.Abilities.FIREBALL:
                if (player.GetComponent<FireballThrow>().FireballObtained())
                {
                    return true;
                }
                break;

            case Consts.Abilities.LIFESTEAL:
                if (player.GetComponent<PlayerHealth>().LifestealUnlocked())
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
