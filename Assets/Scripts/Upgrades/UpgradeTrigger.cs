using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private Upgrades.CardTypes upgradeChoice1 = Upgrades.CardTypes.SWORD_DAMAGE_UPGRADE;
    [SerializeField] private Upgrades.CardTypes upgradeChoice2 = Upgrades.CardTypes.SWORD_DAMAGE_UPGRADE;
    [SerializeField] private Upgrades.CardTypes upgradeChoice3 = Upgrades.CardTypes.SWORD_DAMAGE_UPGRADE;

    public void TriggerUpgradeSelection(GameObject player)
    {
        UpgradeCard[] upgradeCards = {
            Upgrades.GetCardFromType(upgradeChoice1),
            Upgrades.GetCardFromType(upgradeChoice2),
            Upgrades.GetCardFromType(upgradeChoice3),
        };
        UpgradeManager.Instance.StartUpgradeChoiceSelection(upgradeCards, player);
    }
}
