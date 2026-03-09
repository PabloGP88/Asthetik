using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomColorItem : MonoBehaviour
{
    enum ColorTypes
    {
        background,
        color_1,
        color_2,
        color_3
    }
    [Header ("Settings")]
    [SerializeField] private ColorTypes colorType;
    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private ModifyPalette modifyPalette;

    [Header ("References")]
    [SerializeField] private Image mainColor;
    [SerializeField] private Image shadowColor;
    [SerializeField] private Image[] colorLabel;
    [SerializeField] private GameObject customColorPicker;
    
    private void OnEnable()
    {
        modifyPalette.modifyExistingColor += handleModifyExistingColor;
    }

    private void Start()
    {
        switch(colorType)
        {
            case ColorTypes.background:
                mainColor.color = colorPalettes.GetCustomPalette().bgColor;
                break;
            case ColorTypes.color_1:
                mainColor.color = colorPalettes.GetCustomPalette().colors[0];
                break;
            case ColorTypes.color_2:
                mainColor.color = colorPalettes.GetCustomPalette().colors[1];
                break;
            case ColorTypes.color_3:
                mainColor.color = colorPalettes.GetCustomPalette().colors[2];
                break;
        }
        SetPreviewImages();
    }
    public void ChangeCustomColorPaletteColor()
    {
        switch(colorType)
        {
            case ColorTypes.background:
                colorPalettes.SetCustomPalette(true,0,colorPicker.GetCurrentColor());
                mainColor.color = colorPicker.GetCurrentColor();
                SetPreviewImages();
                customColorPicker.SetActive(false);
                break;
            case ColorTypes.color_1:
                colorPalettes.SetCustomPalette(false,0,colorPicker.GetCurrentColor());
                mainColor.color = colorPicker.GetCurrentColor();
                SetPreviewImages();
                customColorPicker.SetActive(false);
                break;
            case ColorTypes.color_2:
                colorPalettes.SetCustomPalette(false,1,colorPicker.GetCurrentColor());
                mainColor.color = colorPicker.GetCurrentColor();
                SetPreviewImages();
                customColorPicker.SetActive(false);
                break;
            case ColorTypes.color_3:
                colorPalettes.SetCustomPalette(false,2,colorPicker.GetCurrentColor());
                mainColor.color = colorPicker.GetCurrentColor();
                SetPreviewImages();
                customColorPicker.SetActive(false);
                break;
        }
    }
    public void CustomColorButtonPressed()
    {
        colorPicker.SetColor(mainColor.color);
        // Subscribe to the event when opening the picker
        colorPicker.onColorSaved += OnColorPickerSaved;
        customColorPicker.SetActive(true);
        UISoundManager.Instance.PlaySoundEffect(6,1f,UISoundManager.MEDIUM_VOLUME);
    }

    private void SetPreviewImages()
    {
        shadowColor.color = DarkenColor(mainColor.color,0.5f);
        foreach (Image color in colorLabel)
        {
            color.color = mainColor.color;
        }
    }

     private void OnColorPickerSaved()
    {
        colorPicker.onColorSaved -= OnColorPickerSaved;
        ChangeCustomColorPaletteColor();
        UISoundManager.Instance.PlaySoundEffect(2,1f,UISoundManager.MEDIUM_VOLUME);
    }
    private Color DarkenColor(Color color, float multiplier)
    {
        multiplier = Mathf.Clamp01(multiplier);
        return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a);
    }
    private void handleModifyExistingColor()
    {
        switch(colorType)
        {
            case ColorTypes.background:
                mainColor.color = colorPalettes.GetCustomPalette().bgColor;
                break;
            case ColorTypes.color_1:
                mainColor.color = colorPalettes.GetCustomPalette().colors[0];
                break;
            case ColorTypes.color_2:
                mainColor.color = colorPalettes.GetCustomPalette().colors[1];
                break;
            case ColorTypes.color_3:
                mainColor.color = colorPalettes.GetCustomPalette().colors[2];
                break;
        }
        SetPreviewImages(); 
    }
    private void OnDestroy()
    {
        if (colorPicker != null)
        {
            colorPicker.onColorSaved -= OnColorPickerSaved;
            modifyPalette.modifyExistingColor -= handleModifyExistingColor;
        }
    }
}
