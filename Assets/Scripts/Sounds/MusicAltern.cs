using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAltern : MonoBehaviour
{
    private AudioSource levelMusic;
    private AudioSource bossMusic;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        levelMusic = audios[0];
        bossMusic = audios[1];
    }

    public void PlayBossMusic(bool playBossMusic)
    {
        if (playBossMusic)
        {
            StartCoroutine(changeAudioVolume(levelMusic, 0f, false, false, 0f));
            StartCoroutine(changeAudioVolume(bossMusic, 0.7f, true, false, 5f));
        }
        else
        {
            StartCoroutine(changeAudioVolume(levelMusic, 0.4f, false, false, 4f));
            StartCoroutine(changeAudioVolume(bossMusic, 0f, false, true, 0f));
        }
    }

    public void fadeMusic(bool boss, float volume, bool play, bool stop)
    {
        StartCoroutine(changeAudioVolume(boss ? bossMusic: levelMusic, volume, play, stop, 0f));
    }


    IEnumerator changeAudioVolume(AudioSource audio, float volume, bool startPlay, bool stopPlay, float wait)
    {
        yield return new WaitForSeconds(wait);
        if (startPlay)
        {
            audio.Play();
        }
        while (audio.volume-volume<=-0.001f || audio.volume - volume>=0.001f)
        {
            audio.volume = Mathf.Lerp(audio.volume, volume, 1f * Time.deltaTime);
            yield return 0f;
        }
        if (stopPlay)
        {
            audio.Stop();
        }
    }
}
