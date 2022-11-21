using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    [SerializeField] private Transform exitPosition;

    private Transition fadeOutCanvas;

    private void Start()
    {
        fadeOutCanvas = FindObjectOfType<Transition>();
    }

    public void Interact(GameObject player)
    {
        fadeOutCanvas?.LoadTransition();

        StartCoroutine(WaitForLoadingScreen(player));
    }

    private IEnumerator WaitForLoadingScreen(GameObject player)
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = exitPosition.position;
    }
}
