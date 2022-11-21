using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds 
{
    public static AudioClip collectSnd = Resources.Load("Audio/SFX/Other/Collectible/Collectible") as AudioClip;
    public static AudioClip activation = Resources.Load("Audio/SFX/Other/Button/Button") as AudioClip;
    public static AudioClip hitSnd = Resources.Load("Audio/SFX/Enemy/Hit/Hit 2") as AudioClip;
    public static AudioClip deathSnd = Resources.Load("Audio/SFX/Enemy/Death/Enemy Kill 6") as AudioClip;
    public static AudioClip jumpSnd = Resources.Load("Audio/SFX/Player/Jump/Jump 3") as AudioClip;
    public static AudioClip doubleJumpSnd = Resources.Load("Audio/SFX/Player/Jump/Jump 2") as AudioClip;
    public static AudioClip wallJumpSnd = Resources.Load("Audio/SFX/Player/Jump/Jump 6") as AudioClip;
    public static AudioClip healSnd = Resources.Load("Audio/SFX/Player/Heal/Heal 1") as AudioClip;
    public static AudioClip hitPlayerSnd = Resources.Load("Audio/SFX/Player/Hit/Ugh 1") as AudioClip;
    public static AudioClip deathPlayerSnd =  Resources.Load("Audio/SFX/Enemy/Hit/Hit 6") as AudioClip;
    public static AudioClip interactionSnd = Resources.Load("Audio/SFX/Other/Chest/Open Chest 1") as AudioClip;
    public static AudioClip destroy = Resources.Load("Audio/SFX/Other/BrokenWall/Destroy") as AudioClip;
    public static AudioClip fireball = Resources.Load("Audio/SFX/Enemy/Hit/Hit 5") as AudioClip;
    public static AudioClip dash = Resources.Load("Audio/SFX/Enemy/Hit/Hit 6") as AudioClip;
    public static AudioClip sword = Resources.Load("Audio/SFX/Enemy/Hit/Hit 3") as AudioClip;
    public static AudioClip upgrade = Resources.Load("Audio/SFX/Event/PowerUp/Power Up 3") as AudioClip;
    public static AudioClip powerUp = Resources.Load("Audio/SFX/Event/PowerUp/Power Up 2") as AudioClip;
    public static AudioClip magic = Resources.Load("Audio/SFX/Event/Magic/Magic 5") as AudioClip;
    public static AudioClip blockFall = Resources.Load("Audio/SFX/Enemy/Hit/Hit 9") as AudioClip;
    public static AudioClip platform = Resources.Load("Audio/SFX/Enemy/Death/Enemy Kill 3") as AudioClip;
    public static AudioClip Yeti = Resources.Load("Audio/SFX/Enemy/Monster Growl") as AudioClip;
    public static AudioClip Darkhen = Resources.Load("Audio/SFX/Enemy/Evil Laugh") as AudioClip;
    public static AudioClip death = Resources.Load("Audio/SFX/Enemy/") as AudioClip;


    //Menu
    public static AudioClip cancel = Resources.Load("Audio/UI/Cancel 2") as AudioClip;
    public static AudioClip click = Resources.Load("Audio/UI/Select 3") as AudioClip;
    public static AudioClip select = Resources.Load("Audio/UI/GUI 2") as AudioClip;

    //Dialogues
    public static AudioClip dialogue1 = Resources.Load("Audio/Dialogue/Blip_Select") as AudioClip;
    public static AudioClip dialogue2 = Resources.Load("Audio/Dialogue/Blip_Select2") as AudioClip;
    public static AudioClip dialogue3 = Resources.Load("Audio/Dialogue/Blip_Select3") as AudioClip;

    //Dash reset
    public static AudioClip dashReset = Resources.Load("Audio/SFX/Other/Dash Reset/GemBreaking") as AudioClip;

    //Yeti
    public static AudioClip yeti_punch = Resources.Load("Audio/SFX/Enemy/Yeti/Punch") as AudioClip;
    public static AudioClip yeti_throw = Resources.Load("Audio/SFX/Enemy/Yeti/Throw") as AudioClip;
    public static AudioClip yeti_jump = Resources.Load("Audio/SFX/Enemy/Yeti/Jump") as AudioClip;
    public static AudioClip snowball_hit = Resources.Load("Audio/SFX/Enemy/Yeti/SnowBallHit") as AudioClip;
}
