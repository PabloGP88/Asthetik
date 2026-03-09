using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPalette : MonoBehaviour
{
    [SerializeField] colorPickerWindowButton colorPicker;
    [SerializeField] ColorPalettes colorPalettes; 
    public event Action modifyExistingColor;

    public void modifyColor()
    {
        colorPalettes.SetCustomPalette(true, 0, colorPalettes.GetCurentPalette().bgColor);
        colorPalettes.SetCustomPalette(false, 0, colorPalettes.GetCurentPalette().colors[0]);
        colorPalettes.SetCustomPalette(false, 1, colorPalettes.GetCurentPalette().colors[1]);
        colorPalettes.SetCustomPalette(false, 2, colorPalettes.GetCurentPalette().colors[2]);
        modifyExistingColor?.Invoke();
        colorPicker.OpenWindow();
    }
}
