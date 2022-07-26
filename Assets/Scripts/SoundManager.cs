using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum AllGameSoundsEnums
{
    BGM,
    TileMatch,
    TilePlacement,
    TileUnmatch,
    UISFX,
    Deal
}

[System.Serializable]
public class EnumSoundToFile
{
    public AllGameSoundsEnums soundEnum;
    public AudioSource sound;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public EnumSoundToFile[] allGameSoundsCombos;

    public bool isMute;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMusic(AudioSource source)
    {
        if (isMute)
        {
            return;
        }

        source.volume = 0.1f;

        source.Play();

        //if (source.isPlaying)
        //{
        //    return;
        //}
        //else
        //{
        //    source.Play();
        //}
    }

    public void FindSoundToPlay(AllGameSoundsEnums enumToPlay)
    {
        EnumSoundToFile soundToPlay = allGameSoundsCombos.Where(P => P.soundEnum == enumToPlay).FirstOrDefault();

        if (soundToPlay != null)
        {
            soundToPlay.sound.gameObject.SetActive(true);
            PlayMusic(soundToPlay.sound);
        }
    }

    public void MuteMusic()
    {
        isMute = !isMute;

        if (isMute)
        {
            foreach (EnumSoundToFile soundCombo in allGameSoundsCombos)
            {
                soundCombo.sound.volume = 0;
            }
        }
        else
        {
            foreach (EnumSoundToFile soundCombo in allGameSoundsCombos)
            {
                soundCombo.sound.volume = 0.1f;
            }
        }
    }
}
