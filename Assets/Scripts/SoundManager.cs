using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource source;

    public bool isMute;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMusic()
    {
        if (isMute)
        {
            return;
        }

        if (source.isPlaying)
        {
            source.volume = 0.1f;
        }
        else
        {
            source.Play();
        }
    }

    public void MuteMusic()
    {
        isMute = !isMute;

        if (isMute)
        {
            source.volume = 0;
        }
        else
        {
            PlayMusic();
        }
    }
}
