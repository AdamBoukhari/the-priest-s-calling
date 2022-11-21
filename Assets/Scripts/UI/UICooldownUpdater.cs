using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownUpdater : MonoBehaviour
{
    private const int SWORD_INDEX = 0;
    private const int DASH_INDEX = 1;
    private const int FIREBALL_INDEX = 2;

    private int fullCooldown = 1;

    [SerializeField] private GameObject[] cooldowns;
    [SerializeField] private Animator[] circleAnimations;
    [SerializeField] private Image[] circleImages;

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            if (!XMLManager.Instance.savedData.playerInfo.listOfCapacities[0].acquired)
            {
                cooldowns[0].SetActive(false);
            }
            if (!XMLManager.Instance.savedData.playerInfo.listOfCapacities[1].acquired)
            {
                cooldowns[1].SetActive(false);
            }
            if (!XMLManager.Instance.savedData.playerInfo.listOfCapacities[2].acquired)
            {
                cooldowns[2].SetActive(false);
            }
        }
    }

    public void ShowAbilityCooldown(int index)
    {
        cooldowns[index].SetActive(true);
    }

    public void UpdateSwordCoolDown(float maxCooldown, float currentCooldown)
    {
        float currentCooldownRatio = (maxCooldown - currentCooldown) / maxCooldown;
        UpdateCooldown(SWORD_INDEX, currentCooldownRatio);
    }

    public void UpdateDashCoolDown(bool dashAvailable)
    {
        float currentCooldownRatio = 0;
        if (dashAvailable)
        {
            currentCooldownRatio = fullCooldown;
        }

        UpdateCooldown(DASH_INDEX, currentCooldownRatio);
    }

    public void UpdateFireballCoolDown(float maxCooldown, float currentCooldown)
    {
        float currentCooldownRatio = (maxCooldown - currentCooldown) / maxCooldown;
        UpdateCooldown(FIREBALL_INDEX, currentCooldownRatio);
    }

    private void UpdateCooldown(int index, float currentCooldownRatio)
    {
        circleImages[index].fillAmount = currentCooldownRatio;
        if(currentCooldownRatio >= fullCooldown)
        {
            circleAnimations[index].SetTrigger(Harmony.AnimatorParameters.Flash);
        }
    }
}
