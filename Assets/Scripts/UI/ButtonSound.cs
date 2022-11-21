using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, ISelectHandler
{
    private AudioSource audioSource;
    private Button button;

    private void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag(Harmony.Tags.MainMenuCanvas).GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnSelect(BaseEventData eventData)
    {
        audioSource.PlayOneShot(Sounds.select);
    }

    private void OnClick()
    {
        audioSource.PlayOneShot(Sounds.click);
    }
}
