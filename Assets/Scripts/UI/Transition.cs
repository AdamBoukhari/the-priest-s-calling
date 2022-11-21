using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public GameObject continueButton, leaveButton;
    public Animator transition;

    public void LoadTransition()
    {
        transition.SetTrigger("Load");
    }

    public void DeathTransition()
    {
        transition.SetTrigger("Dead");
    }
    public void EndGameTransition()
    {
        GameManager.Instance.StopPlayerMovementAfterDialogueEndGame();
        transition.SetTrigger("EndGame");
        StartCoroutine(WaitToActiveButtons()); 
    }

    IEnumerator WaitToActiveButtons()
    {
        yield return new WaitForSeconds(1f);
        continueButton.GetComponent<Button>().enabled = true;
        leaveButton.GetComponent<Button>().enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    public void Continue()
    {
        transition.SetTrigger("EndGame");
        continueButton.GetComponent<Button>().enabled = false;
        leaveButton.GetComponent<Button>().enabled = false;
        StartCoroutine(WaitForFade());
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ActivatePlayerMovementAfterButtonClick();
    }

    public void GoToMainMenu()
    {
        continueButton.GetComponent<Button>().enabled = false;
        leaveButton.GetComponent<Button>().enabled = false;
        SceneSwitcher.Instance.GoBackToMainMenu(1f);
        GameManager.Instance.ActivatePlayerMovementAfterButtonClick();
    }

}
