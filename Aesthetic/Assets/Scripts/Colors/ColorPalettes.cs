using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorPalettes", menuName = "Game/Color Palettes")]

public class ColorPalettes : ScriptableObject
{
    [System.Serializable]
    public class ColorPalette
    {
        public Color bgColor; // Color de fondo
        public Color[] colors; // Colores de la paleta
    }

    [Header("Palettes")]
    [SerializeField] private List<ColorPalette> palettes = new List<ColorPalette>();
    [SerializeField] private List<ColorPalette> randomPalette = new List<ColorPalette>();
    [SerializeField] private List<ColorPalette> customPalette = new List<ColorPalette>();
    public ColorPalette GetRandomPalette()
    {
        if (palettes.Count == 0) 
        {
            Debug.LogError("No palettes defined!");
            return null;
        }
        int index = Random.Range(0, palettes.Count);
        return palettes[index];
    }
    
    public ColorPalette GetPaletteByIndex(int index)
    {
        if (index < 0 || index >= palettes.Count)
        {
            Debug.LogError($"Index {index} is out of bounds! Must be between 0 and {palettes.Count - 1}.");
            return null;
        }
        return palettes[index];
    }

    public List<ColorPalette> GetAllPalettes()
    {
        return palettes;
    }

    public ColorPalette GetCurentPalette()
    {
        switch (PlayerPrefs.GetInt("CurrentPalette",1))
        {
            case 0:
                return randomPalette[0];
            case -1:
                return customPalette[0];
            default:
                return palettes[PlayerPrefs.GetInt("CurrentPalette",1) -1];
        }
    }

    public ColorPalette GetColorPaletteForGame()
    {
        switch (PlayerPrefs.GetInt("CurrentPalette",1))
        {
            case 0:
                return GetRandomPalette();
            case -1:
                return customPalette[0];
            default:
                return palettes[PlayerPrefs.GetInt("CurrentPalette",1) -1];
        }
    }
    public void SetNewCurrentPalette(int value)
    {
        PlayerPrefs.SetInt("CurrentPalette",value);
    }

    public void SetCustomPalette(bool itsBg, int index, Color newColor)
    {
        switch(itsBg)
        {
            case true:
                customPalette[0].bgColor = newColor;
                PlayerPrefs.SetString("CustomBGColor", ColorUtility.ToHtmlStringRGBA(newColor));
                break;
            default:
                customPalette[0].colors[index] = newColor;
                PlayerPrefs.SetString($"CustomColor_{index}", ColorUtility.ToHtmlStringRGBA(newColor));
                break;
        }
        PlayerPrefs.Save();
    }

    private void LoadCustomColors()
    {
        // Load background color
        string bgColorHex = PlayerPrefs.GetString("CustomBGColor", ColorUtility.ToHtmlStringRGBA(customPalette[0].bgColor));
        ColorUtility.TryParseHtmlString("#" + bgColorHex, out customPalette[0].bgColor);

        // Load palette colors
        for (int i = 0; i < customPalette[0].colors.Length; i++)
        {
            string colorHex = PlayerPrefs.GetString($"CustomColor_{i}", ColorUtility.ToHtmlStringRGBA(customPalette[0].colors[i]));
            ColorUtility.TryParseHtmlString("#" + colorHex, out customPalette[0].colors[i]);
        }
    }
    public int GetCurrentPaletteIndex()
    {
        return PlayerPrefs.GetInt("CurrentPalette",1);
    }

    public ColorPalette GetCustomPalette()
    {
        return customPalette[0];
    }
    private void Awake()
    {
        LoadCustomColors();
    }
}
