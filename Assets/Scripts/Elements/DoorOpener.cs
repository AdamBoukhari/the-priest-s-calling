using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorOpener : MonoBehaviour
{
    private const float ACTIVATION_OFFSET = 0.1f;

    [SerializeField] private GameObject door;
    [SerializeField] private float openTime = 7f;
    [SerializeField] private List<DoorOpener> TEMPORAR_OTHER_BUTTON;
   
    private Transform buttonSprite;
    private AudioSource audioSource;

    private float originalButtonScale;
    private Vector3 originalDoorPos;
    private Vector3 targetDoorPos;
    private Vector3 originalButtonPos;
    private float timer = 0;
    
    private int nbCollision;

    private void Start()
    {
        buttonSprite = GetComponentInChildren<Transform>();
        originalButtonScale = buttonSprite.localScale.y;
        float distance = door.GetComponent<SpriteRenderer>().bounds.size.y;
        originalDoorPos = door.transform.position;
        targetDoorPos = new Vector3(originalDoorPos.x, originalDoorPos.y + distance, 0);
        originalButtonPos = buttonSprite.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Sounds.activation;
    }

    private void Update()
    {
        // Going Up
        if (IsBeingPressed()|| OtherButtonPressed())
        {
            timer += Time.deltaTime;

            if (timer >= openTime)
            {
                door.transform.position = targetDoorPos;

            }
            else
            {
                door.transform.position = Vector3.Lerp(door.transform.position, targetDoorPos, timer / openTime);
            }
        }
        // Going Down
        else
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                door.transform.position = originalDoorPos;
            }
            else
            {
                door.transform.position = Vector3.Lerp(door.transform.position, originalDoorPos, timer / openTime);
            }
        }

        timer = Mathf.Clamp01(timer);
    }

    public bool IsBeingPressed()
    {
        return nbCollision > 0;
    }

    private bool OtherButtonPressed()
    {
        foreach (DoorOpener button in TEMPORAR_OTHER_BUTTON)
        {
            if (button.IsBeingPressed())
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            if (nbCollision == 0)
            {
                audioSource.Play();
            }
            nbCollision++;
        }
        else if (collision.gameObject.CompareTag(Harmony.Tags.Box))
        {
            if (nbCollision == 0)
            {
                audioSource.Play();
            }
            nbCollision++;
        }
        else if (collision.CompareTag(Harmony.Tags.Bag))
        {
            if (nbCollision == 0)
            {
                audioSource.Play();
            }
            nbCollision++;
        }
        if (nbCollision>0)
        {
            buttonSprite.localScale = new Vector3(buttonSprite.localScale.x, originalButtonScale / 2, 0);
            buttonSprite.position = new Vector3(originalButtonPos.x, originalButtonPos.y - ACTIVATION_OFFSET, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            if (nbCollision == 0)
            {
                audioSource.Play();
            }
            nbCollision--;
        }
        else if (collision.gameObject.CompareTag(Harmony.Tags.Box))
        {
            if (nbCollision == 0)
            {
                audioSource.Play();
            }
            nbCollision--;
        }

        if (nbCollision==0)
        {
            buttonSprite.localScale = new Vector3(buttonSprite.localScale.x, originalButtonScale, 0);
            buttonSprite.position = new Vector3(originalButtonPos.x, originalButtonPos.y, 0);
        }
    }
}
