using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPaletteBehaivour : MonoBehaviour
{
    [SerializeField] private CurrentColorPalette currentColorPalette;
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private Animator animator;
    private const int PALETTE_NUMBER = 0;

    public void SetRandomColorPalette()
    {
        colorPalettes.SetNewCurrentPalette(PALETTE_NUMBER);
        UISoundManager.Instance.PlaySoundEffect(0,1f,UISoundManager.MEDIUM_VOLUME);
        UISoundManager.Instance.PlaySecondUISound(2,1f,UISoundManager.MEDIUM_VOLUME);
        animator.Play("random_button_pressed", 0, 0f);
        currentColorPalette.SetPaletteColor();
    }
}
