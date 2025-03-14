using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    List<AudioSource> audioSources = new List<AudioSource>();

    AudioSource audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayAudioSource(audioSource);

        if(DataCarrier.instance != null && DataCarrier.instance.isMute)
            foreach (AudioSource i in audioSources) i.volume = 0;
    }

    public void PlayAudioSource(AudioSource source)
    {
        audioSources.Add(source);

        if (DataCarrier.instance != null && DataCarrier.instance.isMute) return;

        source.Play();
    }

    public void PlayAudioSource(AudioClip clip)
    {
        if (DataCarrier.instance != null && DataCarrier.instance.isMute) return;


        // 현재 볼륨 저장
        float originalVolume = audioSource.volume;
        float fadeDuration = 3.0f; // 페이드 지속 시간

        // 볼륨 줄이기
        audioSource.DOFade(0, fadeDuration).OnComplete(() =>
        {
            // 오디오 클립 변경
            audioSource.clip = clip;

            if (DataCarrier.instance != null && DataCarrier.instance.isMute) return;
            
            audioSource.Play();
            audioSource.DOFade(originalVolume, fadeDuration);
        });
    }

    [ContextMenu("Mute")]
    public void Mute()
    {
        if(DataCarrier.instance.isMute)
        {
            foreach (AudioSource i in audioSources) i.volume = 1;
        }
        else
        {
            foreach (AudioSource i in audioSources) i.volume = 0;
        }
        DataCarrier.instance.isMute = !DataCarrier.instance.isMute;
    }
}
