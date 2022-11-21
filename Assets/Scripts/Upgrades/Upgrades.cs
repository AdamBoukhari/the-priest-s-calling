
public class Upgrades
{
    public enum CardTypes
    {
        SWORD_DAMAGE_UPGRADE,
        FIREBALL_DAMAGE_UPGRADE,
        FIREBALL_SPEED_UPGRADE,
        FIREBALL_COOLDOWN_UPGRADE,
        DASH_SPEED_UPGRADE,
        DASH_DURATION_UPGRADE,
        MAXIMUM_HEALTH_UPGRADE,
        MOVEMENT_SPEED_UPGRADE,
    };


    public static UpgradeCard GetCardFromType(CardTypes cardType)
    {
        switch (cardType)
        {
            case CardTypes.SWORD_DAMAGE_UPGRADE:        return SWORD_DAMAGE_UPGRADE;
            case CardTypes.FIREBALL_DAMAGE_UPGRADE:     return FIREBALL_DAMAGE_UPGRADE;
            case CardTypes.FIREBALL_SPEED_UPGRADE:      return FIREBALL_SPEED_UPGRADE;
            case CardTypes.FIREBALL_COOLDOWN_UPGRADE:   return FIREBALL_COOLDOWN_UPGRADE;
            case CardTypes.DASH_SPEED_UPGRADE:          return DASH_SPEED_UPGRADE;
            case CardTypes.DASH_DURATION_UPGRADE:       return DASH_DURATION_UPGRADE;
            case CardTypes.MAXIMUM_HEALTH_UPGRADE:      return MAXIMUM_HEALTH_UPGRADE;
            case CardTypes.MOVEMENT_SPEED_UPGRADE:      return MOVEMENT_SPEED_UPGRADE;
        }
        return null;
    }

    public static string GetTitleFromCard(UpgradeCard card)
    {
        switch (card.upgradeTitle)
        {
            case Consts.UCTitles.U_SWORD:       return SWORD_TITLE;
            case Consts.UCTitles.U_FIREBALL:    return FIREBALL_TITLE;
            case Consts.UCTitles.U_DASH:        return DASH_TITLE;
            case Consts.UCTitles.U_HEALTH:      return HEALTH_TITLE;
            case Consts.UCTitles.U_MOVEMENT:    return MOVEMENT_TITLE;
        }
        return null;
    }



    // CARDS TITLES
    private const string SWORD_TITLE = "Sword upgrade!";
    private const string FIREBALL_TITLE = "Fireball upgrade!";
    private const string DASH_TITLE = "Dash upgrade!";
    private const string HEALTH_TITLE = "Health upgrade!";
    private const string MOVEMENT_TITLE = "Movement upgrade!";

    // Sword
    private static UpgradeCard SWORD_DAMAGE_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_SWORD, Consts.UCTypes.U_DAMAGE, Consts.T_CARD_SWORD, 0.5f, "Melee attack deals {0} more damage."
    );
    
    // Fireball
    private static UpgradeCard FIREBALL_DAMAGE_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_FIREBALL, Consts.UCTypes.U_DAMAGE, Consts.T_CARD_FIREBALL, 0.5f, "Fireballs deals {0} more damage."
    );
    private static UpgradeCard FIREBALL_SPEED_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_FIREBALL, Consts.UCTypes.U_SPEED, Consts.T_CARD_FIREBALL, 25f, "Fireballs flies {0}% faster."
    );
    private static UpgradeCard FIREBALL_COOLDOWN_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_FIREBALL, Consts.UCTypes.U_COOLDOWN, Consts.T_CARD_FIREBALL, 20f, "Fireballs' cooldown is {0}% shorter."
    );

    //Dash
    private static UpgradeCard DASH_SPEED_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_DASH, Consts.UCTypes.U_SPEED, Consts.T_CARD_DASH, 15f, "Dashes are {0}% faster."
    );
    private static UpgradeCard DASH_DURATION_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_DASH, Consts.UCTypes.U_RANGE, Consts.T_CARD_DASH, 15f, "Dashes lasts {0}% longer."
    );

    //Health
    private static UpgradeCard MAXIMUM_HEALTH_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_HEALTH, Consts.UCTypes.U_DAMAGE, Consts.T_CARD_HEALTH, 1f, "Permanently gain {0} maximum health."
    );

    //Movement
    private static UpgradeCard MOVEMENT_SPEED_UPGRADE = new UpgradeCard(
        Consts.UCTitles.U_MOVEMENT, Consts.UCTypes.U_SPEED, Consts.T_CARD_MOVEMENT, 15f, "Run {0}% faster."
    );
}
