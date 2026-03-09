using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateIcons : MonoBehaviour
{
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private GameObject randomIcons;

    private void Update()
    {
        if (colorPalettes.GetCurrentPaletteIndex() == 0)
        {
            randomIcons.SetActive(true);
        } else randomIcons.SetActive(false);
    }
}
