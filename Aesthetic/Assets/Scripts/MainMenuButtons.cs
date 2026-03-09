using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private GameObject randomButton;
    [SerializeField] private GameObject customButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private int numFramesStyles;
    private const string PREFS_COLLECTION_KEY = "ActiveSplashCollection";
    private const string FRAME_STRING = "frameStyle";
    public void StartButton()
    {
        startButton.SetActive(false);

        randomButton.SetActive(true);
        customButton.SetActive(true);

        UISoundManager.Instance.PlaySoundEffect(6, 1f, UISoundManager.HIGH_VOLUME);
    }
    public void CustomPlay()
    {
        cameraAnimator.Play("Camera move", 0, 0);
        UISoundManager.Instance.PlaySoundEffect(3, Random.Range(0.95f, 1.1f), UISoundManager.HIGH_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(2, Random.Range(0.95f, 1.1f), UISoundManager.HIGH_VOLUME);

        startButton.SetActive(true);

        randomButton.SetActive(false);
        customButton.SetActive(false);
    }

    public void RandomPlay()
    {
        colorPalettes.SetNewCurrentPalette(0);
        PlayerPrefs.SetInt(PREFS_COLLECTION_KEY, -2);

        PlayerPrefs.SetInt(FRAME_STRING, (int)Random.Range(0, numFramesStyles));

        PlayerPrefs.SetFloat("MinSize", Random.Range(1.0f, 3.0f));
        PlayerPrefs.SetFloat("MaxSize", Random.Range(1.0f, 3.0f));
        PlayerPrefs.SetInt("AmountStrokes", (int)Random.Range(1, 30));
    }
    public void ReturnMenu()
    {
        cameraAnimator.Play("Camera Exit", 0, 0);
        UISoundManager.Instance.PlaySoundEffect(4, Random.Range(0.95f, 1.1f), UISoundManager.HIGH_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(2, Random.Range(0.95f, 1.1f), UISoundManager.HIGH_VOLUME);
    }
}
