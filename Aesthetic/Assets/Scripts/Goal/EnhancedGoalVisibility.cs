using UnityEngine;

public class EnhancedGoalVisibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer borderColor;
    [SerializeField] private Camera mainCamera;
    
    void Start()
    {
        UpdateBorderColor();
    }
        
    private void UpdateBorderColor()
    {
        if (borderColor == null || mainCamera == null) return;
        
        // Get the camera's background color
        Color cameraBackgroundColor = mainCamera.backgroundColor;
        
        // Calculate the complementary (opposite) color
        Color complementaryColor = GetComplementaryColor(cameraBackgroundColor);
        
        // Apply the complementary color to the border
        borderColor.color = complementaryColor;
    }
    
    private Color GetComplementaryColor(Color originalColor)
    {
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);
        
        // Calculate luminance to determine if background is light or dark
        float luminance = 0.299f * originalColor.r + 0.587f * originalColor.g + 0.114f * originalColor.b;
        
        Color contrastColor;
        
        if (luminance < 0.5f) // Dark background
        {
            // Make it lighter: keep similar hue but increase brightness
            contrastColor = GetLighterSimilarColor(h, s, v);
        }
        else // Light background
        {
            // Make it darker: keep similar hue but decrease brightness
            contrastColor = GetDarkerSimilarColor(h, s, v);
        }
        
        contrastColor.a = originalColor.a;
        return contrastColor;
    }
    
    private Color GetLighterSimilarColor(float h, float s, float v)
    {
        // Keep the hue similar but shift it slightly for variety
        float newHue = h + Random.Range(-0.1f, 0.1f); // Slight hue variation
        newHue = (newHue + 1f) % 1f; // Keep in 0-1 range
        
        // Increase brightness significantly
        float newValue = Mathf.Clamp(v + 0.4f, 0.7f, 1f);
        
        // Adjust saturation for better visibility
        float newSaturation = Mathf.Clamp(s + 0.2f, 0.6f, 1f);
        
        return Color.HSVToRGB(newHue, newSaturation, newValue);
    }
    
    private Color GetDarkerSimilarColor(float h, float s, float v)
    {
        // Keep the hue similar but shift it slightly for variety
        float newHue = h + Random.Range(-0.1f, 0.1f); // Slight hue variation
        newHue = (newHue + 1f) % 1f; // Keep in 0-1 range
        
        // Decrease brightness significantly
        float newValue = Mathf.Clamp(v - 0.4f, 0.1f, 0.5f);
        
        // Increase saturation for richer color
        float newSaturation = Mathf.Clamp(s + 0.3f, 0.7f, 1f);
        
        return Color.HSVToRGB(newHue, newSaturation, newValue);
    }
    
    // Alternative method for high contrast (black/white based on brightness)
    private Color GetHighContrastColor(Color originalColor)
    {
        // Calculate luminance
        float luminance = 0.299f * originalColor.r + 0.587f * originalColor.g + 0.114f * originalColor.b;
        
        // Return black for light backgrounds, white for dark backgrounds
        return luminance > 0.5f ? Color.black : Color.white;
    }
    
    // Public method to force update (call this if you change camera color via script)
    public void ForceUpdateBorderColor()
    {
        UpdateBorderColor();
    }
}