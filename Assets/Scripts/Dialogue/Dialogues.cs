using UnityEngine;

public static class Dialogues
{
    // ATTENTION! Lors de l'ajout d'un nouveau dialogue, mettre le nom en avant dernier, juste avant le NULL.
    public enum DialogueNames {
        FIRST_PRISON_DIALOGUE_SWORD, SECOND_PRISON_DIALOGUE_DASH, BANDIT, FIREBALL_DIALOGUE, DOUBLE_JUMP_OBTENTION, GRAVEDIGGER, VENDOR, MUSICIAN, BARTENDER, TAVERN_MALE, TAVERN_WOMAN, TAVERN_BEARDED_MALE,
        DASH_RESET_DIALOGUE, PRE_BOSS_DIALOGUE, BAG_DIALOGUE, BOX_DIALOGUE, THIRD_PRISON_DIALOGUE_WALLJUMP, FOURTH_PRISON_DIALOGUE, SLIME_DEATH_DIALOGUE, END_GAME_DIALOGUE, ALTERNATE_FIREBALL_DIALOGUE,
        ALTERNATE_FIRST_PRISON_DIALOGUE_SWORD, ALTERNATE_SECOND_PRISON_DIALOGUE_DASH, ALTERNATE_BANDIT, ALTERNATE_DOUBLE_JUMP_OBTENTION, ANTI_SUICIDE_GUY_LINE, ANTI_SUICIDE_GUY, DARKHEN_SLIME_DEFEATED_DIALOGUE,
        END_GAME_DIALOGUE_ALTERNATIVE, NULL, 
    };

    // Mettre le case en bas de la liste lors d'un ajout.
    public static DialogueEvent[] GetDialogueFromName(DialogueNames dialogueName)
    {
        switch (dialogueName)
        {
            case DialogueNames.FIRST_PRISON_DIALOGUE_SWORD: return FIRST_PRISON_DIALOGUE_SWORD;
            case DialogueNames.SECOND_PRISON_DIALOGUE_DASH: return SECOND_PRISON_DIALOGUE_DASH;
            case DialogueNames.THIRD_PRISON_DIALOGUE_WALLJUMP: return THIRD_PRISON_DIALOGUE_WALLJUMP;
            case DialogueNames.FOURTH_PRISON_DIALOGUE: return FOURTH_PRISON_DIALOGUE;
            case DialogueNames.BANDIT: return BANDIT;
            case DialogueNames.FIREBALL_DIALOGUE: return FIREBALL_DIALOGUE;
            case DialogueNames.DOUBLE_JUMP_OBTENTION: return DOUBLE_JUMP_OBTENTION;
            case DialogueNames.GRAVEDIGGER: return GRAVEDIGGER;
            case DialogueNames.VENDOR: return VENDOR;
            case DialogueNames.MUSICIAN: return MUSICIAN;
            case DialogueNames.BARTENDER: return BARTENDER;
            case DialogueNames.TAVERN_MALE: return TAVERN_MALE;
            case DialogueNames.TAVERN_WOMAN: return TAVERN_WOMAN;
            case DialogueNames.TAVERN_BEARDED_MALE: return TAVERN_BEARDED_MALE;
            case DialogueNames.DASH_RESET_DIALOGUE: return DASH_RESET_DIALOGUE;
            case DialogueNames.PRE_BOSS_DIALOGUE: return PRE_BOSS_DIALOGUE;
            case DialogueNames.BAG_DIALOGUE: return BAG_DIALOGUE;
            case DialogueNames.BOX_DIALOGUE: return BOX_DIALOGUE;
            case DialogueNames.END_GAME_DIALOGUE: return END_GAME_DIALOGUE;
            case DialogueNames.SLIME_DEATH_DIALOGUE: return SLIME_DEATH_DIALOGUE;
            case DialogueNames.ALTERNATE_FIREBALL_DIALOGUE: return ALTERNATE_FIREBALL_DIALOGUE;
            case DialogueNames.ALTERNATE_FIRST_PRISON_DIALOGUE_SWORD: return ALTERNATE_FIRST_PRISON_DIALOGUE_SWORD;
            case DialogueNames.ALTERNATE_SECOND_PRISON_DIALOGUE_DASH: return ALTERNATE_SECOND_PRISON_DIALOGUE_DASH;
            case DialogueNames.ALTERNATE_BANDIT: return ALTERNATE_BANDIT;
            case DialogueNames.ALTERNATE_DOUBLE_JUMP_OBTENTION: return ALTERNATE_DOUBLE_JUMP_OBTENTION;
            case DialogueNames.ANTI_SUICIDE_GUY_LINE: return ANTI_SUICIDE_GUY_LINE;
            case DialogueNames.ANTI_SUICIDE_GUY: return ANTI_SUICIDE_GUY;
            case DialogueNames.DARKHEN_SLIME_DEFEATED_DIALOGUE: return DARKHEN_SLIME_DEFEATED_DIALOGUE;
            case DialogueNames.END_GAME_DIALOGUE_ALTERNATIVE: return END_GAME_DIALOGUE_ALTERNATIVE;
                
        }
        return null;
    }

    // PRISON DIALOGUES
    private static DialogueEvent[] FIRST_PRISON_DIALOGUE_SWORD = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Hello young Jab. You're probably wondering why you're here. That damn wizard Darkhen kidnapped you and put you in jail."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_SCARED, "What? Why are you here? Did he do the same to you?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "No. I am here for you. You can say I'm your guardian angel."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You need to escape and fight him, or he'll attack your village instead. Here, you're gonna need this if you want to leave this place."),
        new DialogueFrame(Consts.ABILITY_SWORD, Consts.DESCRIPTION_SWORD),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Woah! Nice! I always wanted a sword ever since I was a kid. Are there any guards guarding this place?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "A ton... Good luck!"),
        new DialogueSound(Sounds.magic),
        new DialogueMove(0, new Vector2(17, 0), 0.05f),
    };

    private static DialogueEvent[] ALTERNATE_FIRST_PRISON_DIALOGUE_SWORD = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Don't be scared little one, you got this!"),
    };

    private static DialogueEvent[] SECOND_PRISON_DIALOGUE_DASH = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Hello again young Jab. I forgot to give you something quite important."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "What is it?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "A special ring that allows you to reach greater distances. You won't be able to reach the exit without it. Here."),
        new DialogueFrame(Consts.ABILITY_DASH, Consts.DESCRIPTION_DASH),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_SCARED, "How does it work excatly?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You can roll on the ground and dash in the air, but only once after you leave the ground. Good luck!"),
    };

    private static DialogueEvent[] ALTERNATE_SECOND_PRISON_DIALOGUE_DASH = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You can roll on the ground and dash in the air, but only once after you leave the ground. Try it!"),
    };

    private static DialogueEvent[] THIRD_PRISON_DIALOGUE_WALLJUMP = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Hmmm that jump seems quite high. Think you can do it?"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "Of course, I was the best in my gym class."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "Watch me jump on these walls."),
    };

    private static DialogueEvent[] FOURTH_PRISON_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Don't go through here, you won't survive. You need to reach the caverns if you want to make it out alive."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Jump down there."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "Okay... I'll trust you."),
    };

    private static DialogueEvent[] BANDIT = {
        new DialogueFrame(Consts.CHARACTER_BANDIT, Consts.FACE_BANDIT, "Thanks! I was stuck in here forever! A boulder fell in and trapped me."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_HAPPY, "No problem. Is there any loot around here you can share ?"),
        new DialogueFrame(Consts.CHARACTER_BANDIT, Consts.FACE_BANDIT, "I see you're holding a sword!"),
        new DialogueFrame(Consts.CHARACTER_BANDIT, Consts.FACE_BANDIT, "I know how to handle one myself, I've perfected a secret technique over the years. Hear this out..."),
        new DialogueFrame(Consts.ABILITY_LIFESTEAL, Consts.DESCRIPTION_LIFESTEAL),
        new DialogueFrame(Consts.CHARACTER_BANDIT, Consts.FACE_BANDIT, "Also, there's this weird glowing bone right here. You can take it if you want, it's yours."),
    };

    private static DialogueEvent[] ALTERNATE_BANDIT = {
        new DialogueFrame(Consts.CHARACTER_BANDIT, Consts.FACE_BANDIT, "Thanks again for saving me!"),
    };


    // CAVERN DIALOGUES
    private static DialogueEvent[] FIREBALL_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You came all the way here young Jab. But now, you are stuck."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "To help you in your quest, I'll teach you the power of fireballs."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Take this."),
        new DialogueSound(Sounds.magic),
        new DialogueFrame(Consts.ABILITY_FIREBALL, Consts.DESCRIPTION_FIREBALL),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Thanks! I feel more powerful already !"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Watchout! Behind you!"),
        new DialogueWait(0.25f),
        new DialogueMove(0, new Vector2(-25, 17), 0.2f),
        new DialogueSound(Sounds.blockFall),
        new DialogueWait(0.75f),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You can use your fireballs to destroy those boulders."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_SCARED, "Ok. I understand! Thank you."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Alright, good luck young Jabwalker."),
    };

    private static DialogueEvent[] ALTERNATE_FIREBALL_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "What are you still doing talking to me? Blast these rocks!"),
    };

    private static DialogueEvent[] BAG_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Those doors are heavy."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You'll need to find some way to keep this button pressed if you want to get through it."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "What are you saying? If I can help you?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "You're a funny little guy!"),
        new DialogueSound(Sounds.magic),
        new DialogueMove(0, new Vector2(-15.11f, 10f), 0.25f),
    };

    private static DialogueEvent[] BOX_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Damn boxes. They can get stuck so easily."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "If you can't move it anymore, hit it like your life depends on it."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "It might magically reappear where it originally was."),
        new DialogueSound(Sounds.magic),
        new DialogueMove(0, new Vector2(22.6f, 10f), 0.25f),
    };


    // VILLAGE DIALOGUES
    private static DialogueEvent[] DOUBLE_JUMP_OBTENTION = {
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Pssst. Come here kid. Heard you were fighting that damn wizard. Got you something..."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "What is it ?"),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Here take those."),
        new DialogueFrame(Consts.ABILITY_DOUBLE_JUMP, Consts.DESCRIPTION_DOUBLE_JUMP),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Woah! Thanks! That'll be super useful!"),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "No problem. You'll need those if you want to reach Darkhen's castle. It sits atop the Scarlet Mountains."),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Go West from here. In the Creepy Grotto, near the entrance to the prison, you'll find a passage to reach the mountains."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Thanks for the information!"),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Good luck kid."),
    };

    private static DialogueEvent[] ALTERNATE_DOUBLE_JUMP_OBTENTION = {
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Go West from here. In the Creepy Grotto, near the entrance to the prison, you'll find a passage to reach the mountains."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Will do!"),
    };

    private static DialogueEvent[] GRAVEDIGGER = {
        new DialogueFrame(Consts.CHARACTER_GRAVEDIGGER, Consts.FACE_GRAVEDIGGER, "Hey Kid. Can i help you?"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "I need to get to Darkhen's castle. Can you help me?"),
        new DialogueFrame(Consts.CHARACTER_GRAVEDIGGER, Consts.FACE_GRAVEDIGGER, "You should go see my son. He's the bartender at the tavern."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_HAPPY, "Will do. Thanks!"),
    };

    private static DialogueEvent[] VENDOR = {
        new DialogueFrame(Consts.CHARACTER_VENDOR, Consts.FACE_VENDOR, "Hey sweetie. Want some chickens?"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "No thanks."),
    };

    private static DialogueEvent[] MUSICIAN = {
        new DialogueFrame(Consts.CHARACTER_MUSICIAN, Consts.FACE_MUSICIAN, "* Never gonna give you up. Never gonna let you down *"),
    };

    private static DialogueEvent[] BARTENDER = {
        new DialogueFrame(Consts.CHARACTER_BARTENDER, Consts.FACE_BARTENDER, "Hey kid. What can I get you?"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "Nothing, I just want some information. Can you tell me how I can get to Darkhen's castle?"),
        new DialogueFrame(Consts.CHARACTER_BARTENDER, Consts.FACE_BARTENDER, "You should go see that weird guy up there. Don't know much about him, but he sure as hell don't like Darkhen, that's all he would say."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "I'll go talk to him."),
    };

    private static DialogueEvent[] TAVERN_MALE = {
        new DialogueFrame(Consts.CHARACTER_TAVERN_MALE, Consts.FACE_TAVERN_MALE, "See that hot girl ? That's MY woman. Don't touch her."),
    };

    private static DialogueEvent[] TAVERN_BEARDED_MALE = {
        new DialogueFrame(Consts.CHARACTER_TAVERN_BEARDED_MALE, Consts.FACE_TAVERN_BEARDED_MALE, "Leave me alone."),
    };

    private static DialogueEvent[] TAVERN_WOMAN = {
        new DialogueFrame(Consts.CHARACTER_TAVERN_WOMAN, Consts.FACE_TAVERN_WOMAN, "My husband is so possessive..."),
    };


    // MOUNTAIN DIALOGUES
    private static DialogueEvent[] DASH_RESET_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "That shiny thingy? Elders call it a dash reset."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_FIERCE, "Cool! How does it work?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Sadly, they didn't explain that part."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "How come?"),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Well, How to say..."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "The elders..."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "They cannot dash"),
        new DialogueSound(Sounds.magic),
        new DialogueMove(0, new Vector2(-42.4f, 40), 0.3f),
    };

    private static DialogueEvent[] PRE_BOSS_DIALOGUE = {
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "He's aware of your presence."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Monsters are growing more and more powerful by the minute, and so does Darkhen."),
        new DialogueFrame(Consts.CHARACTER_PRIEST, Consts.FACE_PRIEST, "Good luck, young Jab. This will be the hardest part of your journey..."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "Thank you... For everything."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "I might be able to save my village, thanks to your help."),
    };

    private static DialogueEvent[] ANTI_SUICIDE_GUY_LINE =
    {
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_NORMAL, "Kid, you're young. You sill have a good life in front of you. Don't do anything you'll regret."),
    };

    private static DialogueEvent[] ANTI_SUICIDE_GUY = {
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_SHOCKED, "WAIT A MINUTE! Please, think about what you're doing "),
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_NORMAL, "I'm trying my best out here, but I get the feeling you're going to end up trying even harder in this world"),
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_NORMAL, "That's the feeling I get when I look at your face... But maybe I'm nuts."),
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_NORMAL, "You're fine. Just don't be so careless. There are too many enjoyable things in the world to be gambling with your life!"),
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_NORMAL, "You won't change the world by jumping carelessly to your doom, don't ya know?"),
        new DialogueFrame(Consts.CHARACTER_ANTI_SUICIDE_GUY, Consts.FACE_ANTI_SUICIDE_SHOCKED, "Now calm down and DON'T jump off the cliff. "),
    };


    // CASTLE DIALOGUES
    private static DialogueEvent[] SLIME_DEATH_DIALOGUE = {
        new DialogueMusic(false, 0f, false, false),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_NORMAL, "How dare you attack my castle?"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "You will die Darkhen! You will be punished for your crimes!"),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_NORMAL, "It's the end of the road for you kid..."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_ANGRY, "Show yourself!"),
        new DialogueSound(Sounds.Darkhen),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_SHOUTING, "Muhahahah ..."),
        new DialogueWait(1f),
        new DialogueBoss(),
        new DialogueWait(1f),
        new DialogueMusic(true, 0.7f,true, false),
    };

    private static DialogueEvent[] DARKHEN_SLIME_DEFEATED_DIALOGUE = {
        new DialogueMusic(false, 0f, false, false),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "Is that all you got?"),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_NORMAL, "For now, but I'll get my revenge."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "Show yourself! Let's end this right here, right now!"),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_NORMAL, "I think I'll pass on that offer. We'll comeback and you won't be able to stop us then. Muahahah"),
        new DialogueSound(Sounds.Darkhen),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "So you're just chickening out?"),
        new DialogueFrame(Consts.CHARACTER_DARKHEN, Consts.FACE_DARKHEN_NORMAL, "Maybe now but my motives are beyond your understanding boy. I can't risk anything. Yet..."),
        new DialogueWait(1f),
        new DialogueSlimeBeaten(),
    };


    // END GAME
    private static DialogueEvent[] END_GAME_DIALOGUE = {
        new DialogueWait(2f),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Hello there!"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONFIDENT, "Merchant Kenobi?"),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "???"),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_ANGRY, "Anyway, I defeated Darkhen's fierce demon but his master was nowhere in sight."),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Well that's gonna be a problem..."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "There must be something we can do to stop him once and for all."),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Actually, there is.. but you might not like it."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_MALDING, "I don't care, tell me."),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "Alright, alright, we must go to Darkhen's true forteress, a couple of miles east through The Abyss of Maidshire."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_CONCERNED, "And how exactly are we gonna get there ? By swimming?"),
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "I'm a traveller you know, I've got a boat. And we're leaving tonight."),
        new DialogueFrame(Consts.CHARACTER_JAB, Consts.FACE_JAB_HAPPY, "Now that's more like it!"),
        new DialogueEndGame(),
    };

    private static DialogueEvent[] END_GAME_DIALOGUE_ALTERNATIVE = {
        new DialogueFrame(Consts.CHARACTER_TRAVELING_MERCHANT, Consts.FACE_TRAVELING_MERCHANT, "We'll go on this adventure in the next game (if the developpers aren't lazy)."),
    };
}

