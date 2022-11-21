using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public enum Abilities
    {
        NULL, SWORD, DASH, FIREBALL, DOUBLE_JUMP, LIFESTEAL
    }

    // Characters dialogue box name
    public const string CHARACTER_JAB = "Jab";
    public const string CHARACTER_PRIEST = "The Priest";
    public const string CHARACTER_DARKHEN = "Darkhen";

    public const string CHARACTER_ANTI_SUICIDE_GUY = "Helpful Guy";

    public const string CHARACTER_BANDIT = "Bandit";
    public const string CHARACTER_GRAVEDIGGER = "Old man John";
    public const string CHARACTER_VENDOR = "Mathilda";
    public const string CHARACTER_MUSICIAN = "Rick";
    public const string CHARACTER_BARTENDER = "John Jr.";
    public const string CHARACTER_TRAVELING_MERCHANT = "???";
    public const string CHARACTER_TAVERN_WOMAN = "Lucie";
    public const string CHARACTER_TAVERN_MALE = "William";
    public const string CHARACTER_TAVERN_BEARDED_MALE = "Rob";

    // Characters dialogue box face sprites
    // *Mandatory* Sprites must be in the folder Assets/Resources
    public static Sprite FACE_JAB_ANGRY = Resources.Load("Jab/Angry") as Sprite;
    public static Sprite FACE_JAB_CONCERNED = Resources.Load("Jab/Concerned") as Sprite;
    public static Sprite FACE_JAB_CONFIDENT = Resources.Load("Jab/Confident") as Sprite;
    public static Sprite FACE_JAB_FIERCE = Resources.Load("Jab/Fierce") as Sprite;
    public static Sprite FACE_JAB_HAPPY = Resources.Load("Jab/Happy") as Sprite;
    public static Sprite FACE_JAB_MALDING = Resources.Load("Jab/Malding") as Sprite;
    public static Sprite FACE_JAB_SCARED = Resources.Load("Jab/Scared") as Sprite;
    public static Sprite FACE_PRIEST = Resources.Load("Priest/Priest") as Sprite;
    public static Sprite FACE_DARKHEN_NORMAL = Resources.Load("Darkhen/Normal") as Sprite;
    public static Sprite FACE_DARKHEN_SHOUTING = Resources.Load("Darkhen/Shouting") as Sprite;

    public static Sprite FACE_ANTI_SUICIDE_NORMAL = Resources.Load("NPCs/StrangeGuy_0") as Sprite;
    public static Sprite FACE_ANTI_SUICIDE_SHOCKED = Resources.Load("NPCs/StrangeGuy_1") as Sprite;

    public static Sprite FACE_BANDIT = Resources.Load("NPCs/Bandit") as Sprite;
    public static Sprite FACE_GRAVEDIGGER = Resources.Load("NPCs/GraveDigger") as Sprite;
    public static Sprite FACE_VENDOR = Resources.Load("NPCs/Vendor") as Sprite;
    public static Sprite FACE_MUSICIAN = Resources.Load("NPCs/Musician") as Sprite;
    public static Sprite FACE_BARTENDER = Resources.Load("NPCs/Bartender") as Sprite;
    public static Sprite FACE_TAVERN_WOMAN = Resources.Load("NPCs/Tavern_Woman") as Sprite;
    public static Sprite FACE_TAVERN_MALE = Resources.Load("NPCs/Tavern_Male") as Sprite;
    public static Sprite FACE_TAVERN_BEARDED_MALE = Resources.Load("NPCs/Tavern_Bearded_Male") as Sprite;
    public static Sprite FACE_TRAVELING_MERCHANT = Resources.Load("NPCs/Traveling_Merchant") as Sprite;

    //public const Texture T_DARKHEN = Resources.Load("Darkhen/Angry") as Texture;

    // Ability item names
    public const string ABILITY_SWORD = "Cool sword";
    public const string ABILITY_LIFESTEAL = "Lifesteal";
    public const string ABILITY_DASH = "Dash Ring";
    public const string ABILITY_FIREBALL = "Fireball Ring";
    public const string ABILITY_DOUBLE_JUMP = "Hermes Boots";

    // Ability item description
    public const string DESCRIPTION_SWORD = "A really cool looking sword that allows you to hit things.";
    public const string DESCRIPTION_LIFESTEAL = "New knowledge that gives you the ability to regen half a heart after each kill";
    public const string DESCRIPTION_DASH = "A shiny ring that allows you to dash horizontally while maintaining your height.";
    public const string DESCRIPTION_FIREBALL = "A shiny ring that allows you to concentrate fiery power into your hands.";
    public const string DESCRIPTION_DOUBLE_JUMP = "A pair of boots that once belonged to a famous god that allows you to gain an extra jump.";

    // Upgrade cards titles
    public enum UCTitles
    {
        U_SWORD, U_FIREBALL, U_DASH, U_HEALTH, U_MOVEMENT
    };

    // Upgrade cards types
    public enum UCTypes
    {
        U_DAMAGE, U_SPEED, U_RANGE, U_COOLDOWN, U_ULTIMATE
    };

    // Upgrade cards textures
    public static Texture T_CARD_BACK = Resources.Load("Cards/CardBack") as Texture;
    public static Texture T_CARD_SWORD = Resources.Load("Cards/Sword") as Texture;
    public static Texture T_CARD_FIREBALL = Resources.Load("Cards/Fireball") as Texture;
    public static Texture T_CARD_DASH = Resources.Load("Cards/Dash") as Texture;
    public static Texture T_CARD_HEALTH = Resources.Load("Cards/Health") as Texture;
    public static Texture T_CARD_MOVEMENT = Resources.Load("Cards/Movement") as Texture;

    //Hearts
    public static Texture T_FULL_HEART = Resources.Load("InGameUI/FullHeart") as Texture;
    public static Texture T_HALF_HEART = Resources.Load("InGameUI/HalfHeart") as Texture;
    public static Texture T_EMPTY_HEART = Resources.Load("InGameUI/EmptyHeart") as Texture;
}
