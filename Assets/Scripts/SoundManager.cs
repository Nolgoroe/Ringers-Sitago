using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    UISound,
    TileUnmatch,
    TilePlace,
    TileMatch,
    DealSeq
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource source;

    public bool isMute;

    public List<AudioSource> allAudioSources;
    Dictionary<SoundType, AudioSource> soundToSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        soundToSource = new Dictionary<SoundType, AudioSource>();

        for (int i = 0; i < System.Enum.GetValues(typeof(SoundType)).Length; i++)
        {
            soundToSource.Add((SoundType)i, allAudioSources[i]);
        }
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

    public void PlaySound(SoundType soundType)
    {
        AudioSource source = soundToSource[soundType];

        source.Play();
        //source.gameObject.SetActive(true);

        //float playTime = source.clip.length;

        //StartCoroutine(DeactivateAfterXTime(playTime, source));
    }

    //IEnumerator DeactivateAfterXTime(float time, AudioSource source)
    //{
    //    yield return new WaitForSeconds(time);
    //    source.Stop();
    //    source.gameObject.SetActive(false);
    //}
}
