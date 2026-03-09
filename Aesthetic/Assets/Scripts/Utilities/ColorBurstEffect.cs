using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorBurstEffect : MonoBehaviour
{
    [Header("Color Burst Settings")]
    [SerializeField] private Image screenOverlay; // Full screen image for color flash
    [SerializeField] private float burstIntensity = 0.3f;
    [SerializeField] private float burstDuration = 0.4f;
    [SerializeField] private AnimationCurve burstCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    public static ColorBurstEffect Instance;
    
    void Awake()
    {
        Instance = this;
        
        // Create screen overlay if it doesn't exist
        if (screenOverlay == null)
        {
            CreateScreenOverlay();
        }
    }
    
    private void CreateScreenOverlay()
    {
        // Create a full-screen overlay canvas
        GameObject overlayGO = new GameObject("Color Burst Overlay");
        Canvas canvas = overlayGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // Render on top
        
        CanvasScaler scaler = overlayGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        // Create the overlay image
        GameObject imageGO = new GameObject("Overlay Image");
        imageGO.transform.SetParent(overlayGO.transform, false);
        
        screenOverlay = imageGO.AddComponent<Image>();
        screenOverlay.color = new Color(1, 1, 1, 0); // Transparent by default
        
        // Make it full screen
        RectTransform rect = screenOverlay.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
    
    public void TriggerColorBurst(Color goalColor)
    {
        StartCoroutine(ColorBurstSequence(goalColor));
    }
    
    private IEnumerator ColorBurstSequence(Color goalColor)
    {
        if (screenOverlay == null) yield break;
        
        float elapsed = 0f;
        Color startColor = new Color(goalColor.r, goalColor.g, goalColor.b, 0);
        Color peakColor = new Color(goalColor.r, goalColor.g, goalColor.b, burstIntensity);
        
        while (elapsed < burstDuration)
        {
            float t = elapsed / burstDuration;
            float curveValue = burstCurve.Evaluate(t);
            
            screenOverlay.color = Color.Lerp(startColor, peakColor, curveValue);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        screenOverlay.color = startColor; // Ensure it's fully transparent at the end
    }
}