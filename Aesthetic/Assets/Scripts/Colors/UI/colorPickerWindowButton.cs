using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorPickerWindowButton : MonoBehaviour
{
    [SerializeField] private Animator animatorButton;
    [SerializeField] private Animator animatorWindow;
    [SerializeField] private CurrentColorPalette currentColorPalette;
    [SerializeField] private ColorPalettes colorPalettes;
    
    private const string OPEN_BUTTON = "Custom Color Button";
    private const string OPEN_WINDOW = "Color Picker Open";
    private const string CLOSE_WINDOW = "Color Picker Close";

    public void OpenWindow()
    {
        animatorButton.Play(OPEN_BUTTON, 0, 0f);
        UISoundManager.Instance.PlaySoundEffect(2, 1f, UISoundManager.MEDIUM_VOLUME);
        animatorWindow.Play(OPEN_WINDOW, 0, 0);
        UISoundManager.Instance.PlaySecondUISound(3, 1f, UISoundManager.MEDIUM_VOLUME);
    }

    public void CloseWindow()
    {
        // Saving Color Palette Logic
        colorPalettes.SetNewCurrentPalette(-1);
        currentColorPalette.SetPaletteColor();
        animatorWindow.Play(CLOSE_WINDOW,0,0);
        UISoundManager.Instance.PlaySoundEffect(5,1f,UISoundManager.MEDIUM_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(4,1f,UISoundManager.LOW_VOLUME);
    }
}
