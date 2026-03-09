using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorPalettesManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private Camera camera_;
    [SerializeField] private SpriteRenderer player_bg;
    [SerializeField] private SpriteRenderer goal_bg;
    
    [Header("Background Shader Material")]
    [SerializeField] private SpriteRenderer backgroundShaderMaterial; // Assign the material with your BG Lines shader
    [SerializeField] private float colorVariationIntensity = 0.15f; // How much lighter/darker the second color should be
    [SerializeField] private float darknessThreshold = 0.3f; // Below this luminance, we'll make it lighter instead of darker

    private Color[] activeColors;
    private Gradient[] activeTrailGradients;
    private const string FRAME_STRING = "frameStyle";

    public bool isInitialized { get; private set; } = false;

    private void Start()
    {
        InitializePalettes();
    }

    private void InitializePalettes()
    {
        if (colorPalettes == null)
        {
            return;
        }

        var selectedPalette = colorPalettes.GetColorPaletteForGame();
        if (selectedPalette == null)
        {
            return;
        }

        if (selectedPalette.colors == null || selectedPalette.colors.Length == 0)
        {
            return;
        }

        activeColors = selectedPalette.colors;
        activeTrailGradients = GenerateGradients(activeColors);

        // Assign background colors
        camera_.backgroundColor = selectedPalette.bgColor;
        player_bg.color = selectedPalette.bgColor;
        goal_bg.color = selectedPalette.bgColor;
        
        // Update background shader material
        UpdateBackgroundShaderMaterial(selectedPalette.bgColor);

        isInitialized = true;
    }

    private void UpdateBackgroundShaderMaterial(Color bgColor)
    {
        if (backgroundShaderMaterial == null)
        {
            Debug.LogWarning("Background shader material is not assigned!");
            return;
        }

        // Calculate luminance to determine if color is dark
        float luminance = GetLuminance(bgColor);
        
        Color secondColor;
        
        if (luminance < darknessThreshold)
        {
            // Color is dark (near black), make it lighter
            secondColor = LightenColor(bgColor, colorVariationIntensity);
        }
        else
        {
            // Color is bright enough, make it darker
            secondColor = DarkenColor(bgColor, colorVariationIntensity);
        }

        // Set the shader properties
        if (PlayerPrefs.GetInt(FRAME_STRING, 0) != 0)
        {
            backgroundShaderMaterial.material.SetColor("_Color1", bgColor);
            backgroundShaderMaterial.material.SetColor("_Color2", secondColor);
        }
        else
        {
            backgroundShaderMaterial.color = bgColor;
            backgroundShaderMaterial.gameObject.transform.position = new Vector3(
                0,
                0,
                1000
            );
        }

    }

    private float GetLuminance(Color color)
    {
        // Calculate relative luminance using standard RGB luminance formula
        return 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
    }

    private Color LightenColor(Color color, float amount)
    {
        // Convert to HSV, increase value (brightness)
        Color.RGBToHSV(color, out float h, out float s, out float v);
        
        // Increase brightness by the specified amount
        v = Mathf.Clamp01(v + amount);
        
        // If it's still too dark, also slightly decrease saturation for better visibility
        if (v < 0.4f)
        {
            s = Mathf.Clamp01(s - amount * 0.3f);
        }
        
        Color result = Color.HSVToRGB(h, s, v);
        result.a = color.a; // Preserve alpha
        return result;
    }

    private Color DarkenColor(Color color, float amount)
    {
        // Convert to HSV, decrease value (brightness)
        Color.RGBToHSV(color, out float h, out float s, out float v);
        
        // Decrease brightness by the specified amount
        v = Mathf.Clamp01(v - amount);
        
        // Slightly increase saturation to maintain color richness when darkening
        s = Mathf.Clamp01(s + amount * 0.2f);
        
        Color result = Color.HSVToRGB(h, s, v);
        result.a = color.a; // Preserve alpha
        return result;
    }

    public Color[] GetActiveColors()
    {
        if (!isInitialized)
        {
            InitializePalettes();
        }
        return activeColors;
    }

    private Gradient[] GenerateGradients(Color[] colors)
    {
        Gradient[] gradients = new Gradient[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(colors[i], 0f), new GradientColorKey(colors[i], 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );
            gradients[i] = gradient;
        }
        return gradients;
    }

    public Gradient[] GetActiveTrailGradients()
    {
        return activeTrailGradients;
    }

    // Public method to manually update the background material (useful for runtime palette changes)
    public void RefreshBackgroundMaterial()
    {
        if (colorPalettes != null)
        {
            var currentPalette = colorPalettes.GetCurentPalette();
            if (currentPalette != null)
            {
                UpdateBackgroundShaderMaterial(currentPalette.bgColor);
            }
        }
    }

    // Method to set custom shader properties if needed
    public void SetBackgroundShaderProperty(string propertyName, float value)
    {
        if (backgroundShaderMaterial != null)
        {
            backgroundShaderMaterial.material.SetFloat(propertyName, value);
        }
    }

    public void SetBackgroundShaderProperty(string propertyName, Color value)
    {
        if (backgroundShaderMaterial != null)
        {
            backgroundShaderMaterial.material.SetColor(propertyName, value);
        }
    }
}