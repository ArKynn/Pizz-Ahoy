using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip wavesAmbience;
    [SerializeField] private List<AudioSource> wavesAmbienceAudioSources;
    [SerializeField] private AudioClip seagullsAmbience;
    [SerializeField] private List<AudioSource> seagullAmbienceAudioSources;

    private void Start()
    {
        SetupAmbienceAudioSources(wavesAmbienceAudioSources, wavesAmbience);
        SetupAmbienceAudioSources(seagullAmbienceAudioSources, seagullsAmbience);
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

    private void SetupAmbienceAudioSources(List<AudioSource> audioSources, AudioClip clip)
    {
        foreach(AudioSource a in audioSources)
        {
            a.clip = clip;
            a.loop = true;
            a.Play();
        }
    }
}