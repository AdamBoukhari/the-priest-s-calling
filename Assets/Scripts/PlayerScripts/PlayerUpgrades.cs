using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    private UpgradeCard newUpgrade;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerHealth playerHealth;
    private FireballThrow playerFireball;
    private PlayerSwordAttack playerSwordAttack;

    float value;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerHealth = GetComponent<PlayerHealth>();
        playerFireball = GetComponent<FireballThrow>();
        playerSwordAttack = GetComponent<PlayerSwordAttack>();
    }

    public void UnlockAbility(string name)
    {
        switch (name)
        {
            case Consts.ABILITY_SWORD:
                playerSwordAttack.UnlockSword();
                break;
            case Consts.ABILITY_LIFESTEAL:
                playerHealth.UnlockLifeSteal();
                break;
            case Consts.ABILITY_DASH:
                playerDash.UnlockDash();
                break;
            case Consts.ABILITY_FIREBALL:
                playerFireball.UnlockFireball();
                break;
            case Consts.ABILITY_DOUBLE_JUMP:
                playerMovement.UnlockDoubleJump();
                break;
        }
    }

    public void RedirectUpgradeToPlayerStats(UpgradeCard upgradeCard)
    {
        newUpgrade = upgradeCard;
        if(newUpgrade.upgradeValue != null)
        {
            value = (float)newUpgrade.upgradeValue;
        }

        switch (newUpgrade.upgradeTitle)
        {
            case Consts.UCTitles.U_SWORD: 
                UpgradeSword(); 
                break;
            case Consts.UCTitles.U_FIREBALL:
                UpgradeFireball();
                break;
            case Consts.UCTitles.U_DASH:
                UpgradeDash();
                break;
            case Consts.UCTitles.U_HEALTH:
                UpgradeHealth();
                break;
            case Consts.UCTitles.U_MOVEMENT:
                UpgradeMovement();
                break;
        }
    }

    // These methods settle the correct method to call depending on the upgrade type.
    private void UpgradeSword()
    {
        UpgradeSelectionFromCardType(
            damageUpgradeFunction: playerSwordAttack.UpgradeSwordDamage,
            speedUpgradeFunction: null,
            rangeUpgradeFunction: null,
            cooldownUpgradeFunction: null,
            ultimateUpgradeFunction: null // NOT IMPLEMENTED
        );
    }

    private void UpgradeFireball()
    {
        UpgradeSelectionFromCardType(
            damageUpgradeFunction: playerFireball.UpgradeFireballsDamage,
            speedUpgradeFunction: playerFireball.UpgradeFireballsSpeed,
            rangeUpgradeFunction: null,
            cooldownUpgradeFunction: playerFireball.DecreaseFireballCooldown,
            ultimateUpgradeFunction: null // NOT IMPLEMENTED
        );
    }

    private void UpgradeDash()
    {
        UpgradeSelectionFromCardType(
            damageUpgradeFunction: null,
            speedUpgradeFunction: playerDash.UpgradeSpeed,
            rangeUpgradeFunction: playerDash.UpgradeRange,
            cooldownUpgradeFunction: null,
            ultimateUpgradeFunction: null // NOT IMPLEMENTED
        );
    }

    private void UpgradeHealth()
    {
        UpgradeSelectionFromCardType(
            damageUpgradeFunction: playerHealth.AddMaxHeart,
            speedUpgradeFunction: null,
            rangeUpgradeFunction: null,
            cooldownUpgradeFunction: null,
            ultimateUpgradeFunction: null // Given through dialogues
        );
    }

    private void UpgradeMovement()
    {
        UpgradeSelectionFromCardType(
            damageUpgradeFunction: null,
            speedUpgradeFunction: playerMovement.UpgradeMovementSpeed,
            rangeUpgradeFunction: null,
            cooldownUpgradeFunction: null,
            ultimateUpgradeFunction: null
        );
    }

    private void UpgradeSelectionFromCardType(
        Action<float> damageUpgradeFunction,
        Action<float> speedUpgradeFunction,
        Action<float> rangeUpgradeFunction,
        Action<float> cooldownUpgradeFunction,
        Action ultimateUpgradeFunction
    )
    {
        switch (newUpgrade.upgradeType)
        {
            case Consts.UCTypes.U_DAMAGE:
                if (damageUpgradeFunction != null) damageUpgradeFunction(value);
                break;
            case Consts.UCTypes.U_SPEED:
                if (speedUpgradeFunction != null) speedUpgradeFunction(value);
                break;
            case Consts.UCTypes.U_RANGE:
                if (rangeUpgradeFunction != null) rangeUpgradeFunction(value);
                break;
            case Consts.UCTypes.U_COOLDOWN:
                if (cooldownUpgradeFunction != null) cooldownUpgradeFunction(value);
                break;
            case Consts.UCTypes.U_ULTIMATE:
                if (ultimateUpgradeFunction != null) ultimateUpgradeFunction();
                break;
        }
    }
}
