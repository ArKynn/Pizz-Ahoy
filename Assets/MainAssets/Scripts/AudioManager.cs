using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup bgmusicMixer;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip wavesAmbience;
    [SerializeField] private AudioMixerGroup wavesMixer;
    [SerializeField] private List<AudioSource> wavesAmbienceAudioSources;
    [SerializeField] private AudioClip seagullsAmbience;
    [SerializeField] private AudioMixerGroup seagullsMixer;
    [SerializeField] private List<AudioSource> seagullAmbienceAudioSources;

    private void Start()
    {
        SetupAmbienceAudioSources(new List<AudioSource>(){musicAudioSource}, bgMusic, 0.15f, spatial3D:false);
        SetupAmbienceAudioSources(wavesAmbienceAudioSources, wavesAmbience, 0.2f, 0.5f, 25f);
        //SetupAmbienceAudioSources(seagullAmbienceAudioSources, seagullsAmbience, 0.8f, 0.5f, 20f);
    }


    public static void PlayLocalSound(AudioSource audioSource, AudioClip audioClip)
    {
        if(audioClip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static AudioSource CreateLocalAudioSource(GameObject gameObject, AudioMixerGroup mixerGroup,
        AudioClip clip = null, bool playOnAwake = false, bool loop = false, float spatialBlend = 1.0f,
            float dopplerLevel = 0f, float minDistance = 1f, float maxDistance = 15f, float volume = 1.0f)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.outputAudioMixerGroup = mixerGroup;
        newAudioSource.playOnAwake = playOnAwake;
        newAudioSource.loop = loop;
        newAudioSource.spatialBlend = spatialBlend;
        newAudioSource.dopplerLevel = dopplerLevel;
        newAudioSource.rolloffMode = AudioRolloffMode.Linear;
        newAudioSource.minDistance = minDistance;
        newAudioSource.maxDistance = maxDistance;
        newAudioSource.volume = volume;

        if(playOnAwake && loop && clip != null)
        {
            newAudioSource.clip = clip;
            newAudioSource.Play();
        }
        
        return newAudioSource;
    }

    private void SetupAmbienceAudioSources(List<AudioSource> audioSources, AudioClip clip,
        float volume = 1.0f, float minDistance = 1f, float maxDistance = 15f, bool spatial3D = true)
    {
        foreach(AudioSource a in audioSources)
        {
            if(spatial3D)
                a.spatialBlend = 1.0f;
            else
                a.spatialBlend = 0.0f;

            a.rolloffMode = AudioRolloffMode.Linear;
            a.volume = volume;
            a.minDistance = minDistance;
            a.maxDistance = maxDistance;
            a.clip = clip;
            a.loop = true;
            a.Play();
        }
    }
}