using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomArtStyle : MonoBehaviour
{
    [SerializeField] private PreviewSplash previewSplash;
    [SerializeField] private Animator animator;
    private const string PREFS_COLLECTION_KEY = "ActiveSplashCollection";

    public void SetRandomArtStyle()
    {
        PlayerPrefs.SetInt(PREFS_COLLECTION_KEY, -2);

        previewSplash.SetRandomSprite();
        UISoundManager.Instance.PlaySoundEffect(0,1f,UISoundManager.MEDIUM_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(2,1f,UISoundManager.MEDIUM_VOLUME);
        animator.Play("random_button_pressed", 0, 0f);
        
    }
}
