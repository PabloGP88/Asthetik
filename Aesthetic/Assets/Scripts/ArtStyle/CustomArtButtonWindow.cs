using UnityEngine;

public class CustomArtButtonWindow : MonoBehaviour
{
    [SerializeField] private Animator animatorButton;
    [SerializeField] private Animator animatorWindow;
    [SerializeField] private CustomArtStyleCard[] customArtStyleCards;
    [SerializeField] private PreviewSplash previewSplash;
    
    private const string OPEN_BUTTON = "Custom Color Button";
    private const string OPEN_WINDOW = "Color Picker Open";
    private const string CLOSE_WINDOW = "Color Picker Close";
    private const string PREFS_COLLECTION_KEY = "ActiveSplashCollection";

    public void OpenWindow()
    {
        animatorButton.Play(OPEN_BUTTON, 0, 0f);
        UISoundManager.Instance.PlaySoundEffect(2, 1f, UISoundManager.MEDIUM_VOLUME);
        animatorWindow.Play(OPEN_WINDOW, 0, 0);
        UISoundManager.Instance.PlaySecondUISound(3, 1f, UISoundManager.MEDIUM_VOLUME);
    }

    public void CloseWindow()
    {
        animatorWindow.Play(CLOSE_WINDOW, 0, 0);
        UISoundManager.Instance.PlaySoundEffect(5, 1f, UISoundManager.MEDIUM_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(4, 1f, UISoundManager.LOW_VOLUME);

        foreach (CustomArtStyleCard customArtStyleCard in customArtStyleCards)
        {
            customArtStyleCard.SaveCustomArt();
        }
        PlayerPrefs.SetInt(PREFS_COLLECTION_KEY, -1);
        previewSplash.SetCustomSprite();
    }
}

