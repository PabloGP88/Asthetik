using UnityEngine;

public class PlayUISoundEffect : MonoBehaviour
{
    public float pitch;

    public int soundIndex;
    
    public void PlaySFX()
    {
        UISoundManager.Instance.PlaySoundEffect(soundIndex, pitch, UISoundManager.HIGH_VOLUME);
    }
}
