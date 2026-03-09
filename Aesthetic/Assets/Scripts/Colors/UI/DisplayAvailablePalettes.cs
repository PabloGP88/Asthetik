using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAvailablePalettes : MonoBehaviour
{
    [Header ("Referecnes")]
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject paletteButtonPrefab;
    [SerializeField] private ColorPalettes colorPalettes;

    private List<ColorPalettes.ColorPalette> availablePalettes;

    void Awake()
    {
        availablePalettes = colorPalettes.GetAllPalettes();
        SpawnAllPalettes();
    }

    private void SpawnAllPalettes()
    {
        for (int i = 0; i < availablePalettes.Count; i ++)
        {
            GameObject paletteButton = Instantiate(paletteButtonPrefab,parent);
            
            Image bgColor = paletteButton.transform.GetChild(1).GetComponent<Image>();
            Image paletteColor1 = paletteButton.transform.GetChild(2).GetComponent<Image>();
            Image paletteColor2 = paletteButton.transform.GetChild(3).GetComponent<Image>();
            Image paletteColor3 = paletteButton.transform.GetChild(4).GetComponent<Image>();

            bgColor.color = availablePalettes[i].bgColor;
            paletteColor1.color = availablePalettes[i].colors[0];
            paletteColor2.color = availablePalettes[i].colors[1];
            paletteColor3.color = availablePalettes[i].colors[2];
        }
    }
}
