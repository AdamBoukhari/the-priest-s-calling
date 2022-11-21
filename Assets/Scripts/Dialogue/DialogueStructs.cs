using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DialogueEvent {}

public struct DialogueFrame : DialogueEvent
{
    public string name;
    public Sprite image;
    public string sentence;
    public bool isAbility;

    // Used for dialogues
    public DialogueFrame(string name, Sprite image, string sentence)
    {
        this.name = name;
        this.image = image;
        this.sentence = sentence;
        isAbility = false;
    }

    // Used for acquiring new abilites only. There should only be 5 references to this constructor (1 for each new ability).
    public DialogueFrame(string name, string sentence)
    {
        this.name = name;
        image = null;
        this.sentence = sentence;
        isAbility = true;
    }
}

public struct DialogueSound : DialogueEvent
{
    public AudioClip audio;

    public DialogueSound(AudioClip audio)
    {
        this.audio = audio;
    }
}

public struct DialogueWait : DialogueEvent
{
    public float time;

    public DialogueWait(float time)
    {
        this.time = time;
    }
}

public struct DialogueMusic : DialogueEvent
{
    public bool boss;
    public float volume;
    public bool play;
    public bool stop;

    public DialogueMusic(bool boss, float volume, bool play, bool stop)
    {
        this.boss = boss;
        this.volume = volume;
        this.play = play;
        this.stop = stop;
    }
}

public struct DialogueMove : DialogueEvent
{
    public int targetIndex;
    public Vector2 moveTarget;
    public float movespeed;


    public DialogueMove(int targetIndex, Vector2 targetVector, float movespeed)
    {
        this.targetIndex = targetIndex;
        moveTarget = targetVector;
        this.movespeed = movespeed;
    }
}

public struct DialogueSlimeBeaten : DialogueEvent {}

public struct DialogueEndGame : DialogueEvent {}

public struct DialogueBoss : DialogueEvent {}
