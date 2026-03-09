using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private AudioSource SE_Source1;
    [SerializeField] private AudioSource SE_Source2;
    [SerializeField] private List<AudioClip> splashSoundEffects;
    [SerializeField] private List<AudioClip> popSoundEffects;
    public static SoundManager Instance;
    public const float EFFECTS_VOLUME_L = 0.7f;
    public const float EFFECTS_VOLUME_M = 0.5f;
    public const float EFFECTS_VOLUME_S = 0.2f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySplashffect_Source1(float pitch, float volume)
    {
        SE_Source1.clip = splashSoundEffects[Random.Range(0,splashSoundEffects.Count)];
        SE_Source1.volume = volume;
        SE_Source1.pitch = pitch;
        SE_Source1.Play();
    }
    public void PlaySplashffect_Source2(float pitch, float volume)
    {
        SE_Source2.clip = popSoundEffects[Random.Range(0,popSoundEffects.Count)];
        SE_Source2.volume = volume;
        SE_Source2.pitch = pitch;
        SE_Source2.Play();
    }
}
