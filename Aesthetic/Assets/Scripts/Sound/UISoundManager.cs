using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance { get; private set; }

    [Header("Audio Effects")]
    [SerializeField] private AudioClip[] soundEffects;

    // 0 - Color Palette Selected
    // 1 - Failed Button Pressed
    // 2 - Button Pressed
    // 3 - Swoosh open
    // 4 - Swoosh close
    // 5 - Custom Palette Open
    // 6 - Open Sound 2
    // 7 - Pop Slider Sound

    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceTwo;
    public const float HIGH_VOLUME = 0.8f;
    public const float MEDIUM_VOLUME = 0.6f;
    public const float LOW_VOLUME = 0.4f;


    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundEffect(int soundIndex, float pitch, float volume)
    {
        // Validate soundIndex
        if (soundIndex < 0 || soundIndex >= soundEffects.Length)
        {
            Debug.LogWarning($"Sound index {soundIndex} is out of range. No sound will be played.");
            return;
        }

        audioSource.clip = soundEffects[soundIndex];
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlaySecondUISound(int soundIndex, float pitch, float volume)
    {
        // Validate soundIndex
        if (soundIndex < 0 || soundIndex >= soundEffects.Length)
        {
            Debug.LogWarning($"Sound index {soundIndex} is out of range. No sound will be played.");
            return;
        }

        audioSourceTwo.clip = soundEffects[soundIndex];
        audioSourceTwo.pitch = pitch;
        audioSourceTwo.volume = volume;
        audioSourceTwo.Play();
    }

    public void PlayCloseSound()
    {
        PlaySoundEffect(7, 1F, HIGH_VOLUME);
    }

    public void NextClickSound()
    {
         PlaySoundEffect(10, Random.Range(0.93f,1.1f), HIGH_VOLUME);
    }
}
