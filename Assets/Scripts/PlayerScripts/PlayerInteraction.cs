using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionIcon;

    private Vector3 NPC_OFFSET = new Vector3(0.2f, 1.5f, 0);
    private Vector3 DOOR_OFFSET = new Vector3(0.2f, 1.5f, 0);
    private Vector3 NORMAL_CHEST_OFFSET = new Vector3(-0.2f, 0.8f, 0);
    private Vector3 FLIPPED_CHEST_OFFSET = new Vector3(0.2f, 0.8f, 0);
    private Vector3 iconOffset;
    private InteractionTypes interactionType;
    private ChestActivator chest;
    private DialogueActivator npcDialogue;
    private HouseDoor houseDoor;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private GameObject player;
    private GameObject interactiveObjectInRange;

    private bool playerStopped = false;

    private enum InteractionTypes
    {
        NONE, CHEST, DOOR, NPC
    };

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        interactionIcon?.SetActive(false);

        player = gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != interactiveObjectInRange && collision.CompareTag(Harmony.Tags.Interaction))
        {
            interactiveObjectInRange = collision.gameObject;
            FindInteractibleType();

            interactionIcon.SetActive(true);
            interactionIcon.transform.parent = collision.transform;
            interactionIcon.transform.position = collision.transform.position + iconOffset;

            if(interactionType == InteractionTypes.NONE)
            {
                interactionIcon.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(interactiveObjectInRange = collision.gameObject)
        {
            RemoveObjectInRange();
        }
    }

    private void RemoveObjectInRange()
    {
        interactiveObjectInRange = null;
        chest = null;
        npcDialogue = null;
        houseDoor = null;
        interactionType = InteractionTypes.NONE;
        interactionIcon.SetActive(false);
    }

    private void FindInteractibleType()
    {
        if (interactiveObjectInRange.TryGetComponent(out ChestActivator chest) && chest != null)
        {
            if (interactiveObjectInRange.GetComponent<ChestActivator>().isClosed())
            {
                interactionType = InteractionTypes.CHEST;
                this.chest = chest;
                iconOffset = NORMAL_CHEST_OFFSET;

                if (chest.GetComponent<SpriteRenderer>().flipX)
                {
                    iconOffset = FLIPPED_CHEST_OFFSET;
                }
           }
        }
        else if (interactiveObjectInRange.TryGetComponent(out DialogueActivator npcDialogue) && npcDialogue != null)
        {
            interactionType = InteractionTypes.NPC;
            this.npcDialogue = npcDialogue;
            iconOffset = NPC_OFFSET;
        }
        else if (interactiveObjectInRange.TryGetComponent(out HouseDoor houseDoor) && houseDoor != null)
        {
            interactionType = InteractionTypes.DOOR;
            this.houseDoor = houseDoor;
            iconOffset = DOOR_OFFSET;
        } 
        else
        {
            interactionType = InteractionTypes.NONE;
        }
    }

    public bool IsInInteractionRange()
    {
        return interactiveObjectInRange != null;
    }

    public void OnInteract()
    {
        if(interactiveObjectInRange != null)
        {
            switch(interactionType)
            {
                case InteractionTypes.CHEST:
                    chest.Interact(player);
                    break;

                case InteractionTypes.NPC:
                    npcDialogue.Interact(player);
                    break;

                case InteractionTypes.DOOR:
                    houseDoor.Interact(player);
                    break;
            }

            RemoveObjectInRange();
        }
    }

    public bool PlayerStopped()
    {
        return playerStopped;
    }

    public void ActivatePlayerControls()
    {
        playerStopped = false;
        playerInput.ActivateInput();
        playerInput.actions.Enable();
        playerHealth.SetPlayerStopped(playerStopped);
    }

    public void DeactivatePlayerControls()
    {
        playerStopped = true;
        playerInput.DeactivateInput(); 
        playerInput.actions.Disable();
        playerMovement.StopPlayer();
        playerHealth.SetPlayerStopped(playerStopped);
    }
}
