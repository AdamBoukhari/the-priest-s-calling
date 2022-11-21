using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard
{
    public Consts.UCTitles upgradeTitle;
    public Consts.UCTypes upgradeType;
    public Texture displayedCard;
    public float? upgradeValue;
    public string upgradeDescription;

    public UpgradeCard(
        Consts.UCTitles upgradeTitle,
        Consts.UCTypes upgradeType,
        Texture displayedCard, 
        float? upgradeValue,
        string upgradeDescription
    )
    {
        this.upgradeTitle = upgradeTitle;
        this.upgradeType = upgradeType;
        this.displayedCard = displayedCard;
        this.upgradeValue = upgradeValue;
        this.upgradeDescription = upgradeDescription;
    }
}
