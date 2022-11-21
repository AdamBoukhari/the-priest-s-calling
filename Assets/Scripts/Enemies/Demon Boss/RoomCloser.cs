using UnityEngine;

public class RoomCloser : MonoBehaviour
{
    [SerializeField] private GameObject doorToClose;
    [SerializeField] private GameObject doorToDesable;
    [SerializeField] private SlimeDamage slimeDamage;

    private bool roomDoorIsClosed;
    private Vector3 originalDoorPos;
    private float doorTimer;
    private const float OpenTime = 7f;
    private float doorDistance;
    private Vector3 targetDoorPos;
    private bool playerEntered;
    private bool slimeHealthBarActive;


    private void Start()
    {
        originalDoorPos = doorToClose.transform.position;
        doorDistance = doorToClose.GetComponent<SpriteRenderer>().bounds.size.y;
        targetDoorPos = new Vector3(originalDoorPos.x, originalDoorPos.y - doorDistance, 0);
        ResetDoorPosition(false, false);
    }

    private void OnDisable()
    {
        Publisher.PushData -= ResetDoorPosition;
    }

    private void OnEnable()
    {
        Publisher.PushData += ResetDoorPosition;
    }

    private void ResetDoorPosition(bool loadPosition, bool playerDead)
    {
        doorToClose.transform.position = originalDoorPos;
        roomDoorIsClosed = false;
        doorToDesable.SetActive(true);
        doorToClose.SetActive(false);
        playerEntered = false;
        slimeHealthBarActive = false;
    }

    private void Update()
    {
        if (playerEntered && !roomDoorIsClosed)
            CloseTheRoomDoor();
    }

    private void CloseTheRoomDoor()
    {
        if (!slimeHealthBarActive)
        {
            slimeDamage.ActivateHealthBar();
            slimeHealthBarActive = true;
        }
        doorToDesable.SetActive(false);
        doorToClose.SetActive(true);
        doorTimer += Time.deltaTime;

        if (doorTimer >= OpenTime)
        {
            doorToClose.transform.position = targetDoorPos;
        }
        else
        {
            doorToClose.transform.position = Vector3.Lerp(doorToClose.transform.position, targetDoorPos, doorTimer / OpenTime);
        }
        doorTimer = Mathf.Clamp01(doorTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player) && !playerEntered)
        {
            playerEntered = true;
        }
    }
}
