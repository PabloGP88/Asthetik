using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicTrailSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Movment playerMovement;
    [SerializeField] private ColorPalettesManager colorManager;
    
    [Header("Segmented Color Effect")]
    [SerializeField] private bool useSegmentedMode = false;
    [SerializeField] private int colorSegments = 4;
    [SerializeField] private float segmentDuration = 0.3f;
    [SerializeField] private AnimationCurve segmentTransition = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Dynamic Width")]
    [SerializeField] private float baseWidth = 0.5f;
    [SerializeField] private float maxWidth = 1.2f;
    [SerializeField] private float widthSensitivity = 2f;
    [SerializeField] private float widthSmoothness = 5f;
    
    [Header("Trail Behavior")]
    [SerializeField] private float dynamicLifetime = 2f;
    [SerializeField] private float fastMovementLifetime = 3f;
    [SerializeField] private float velocityThreshold = 5f;
    
    private Vector3 lastPosition;
    private float currentVelocity;
    private float targetWidth;
    private Color[] paletteColors;
    private Material trailMaterial;
    
    // Segmented color system
    private List<Color> segmentedColors = new List<Color>();
    private bool isSegmentedActive = false;
    private float segmentTimer = 0f;
    
    void Start()
    {
        InitializeTrail();
        lastPosition = transform.position;
    }
    
    void Update()
    {
        UpdateVelocity();
        UpdateTrailWidth();
        UpdateTrailColor();
        UpdateTrailLifetime();
        UpdateSegmentedEffect();
    }
    
    private void InitializeTrail()
    {
        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();
            
        // Create a copy of the material
        trailMaterial = new Material(trailRenderer.material);
        trailRenderer.material = trailMaterial;
        
        // Get colors from palette manager
        if (colorManager != null && colorManager.isInitialized)
        {
            paletteColors = colorManager.GetActiveColors();
            SetupSegmentedColors();
        }
        
        targetWidth = baseWidth;
    }
    
    private void SetupSegmentedColors()
    {
        segmentedColors.Clear();
        if (paletteColors != null && paletteColors.Length > 0)
        {
            for (int i = 0; i < colorSegments; i++)
            {
                segmentedColors.Add(paletteColors[i % paletteColors.Length]);
            }
        }
    }
    
    private void UpdateVelocity()
    {
        Vector3 currentPosition = transform.position;
        currentVelocity = Vector3.Distance(currentPosition, lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;
    }
    
    private void UpdateTrailWidth()
    {
        float velocityFactor = Mathf.Clamp01(currentVelocity / widthSensitivity);
        targetWidth = Mathf.Lerp(baseWidth, maxWidth, velocityFactor);
        
        float currentWidth = Mathf.Lerp(trailRenderer.startWidth, targetWidth, widthSmoothness * Time.deltaTime);
        
        trailRenderer.startWidth = currentWidth;
        trailRenderer.endWidth = currentWidth * 0.1f;
    }
    
    private void UpdateTrailColor()
    {
        if (isSegmentedActive)
        {
            UpdateSegmentedColors();
        }
        else
        {
            UpdateNormalColors();
        }
    }
    
    private void UpdateNormalColors()
    {
        if (paletteColors == null || paletteColors.Length == 0) return;
        
        Color targetColor = paletteColors[0]; // Use first color as default
        
        // Apply color with smooth transition
        Color currentColor = Color.Lerp(trailRenderer.startColor, targetColor, 3f * Time.deltaTime);
        trailRenderer.startColor = currentColor;
        
        Color endColor = currentColor;
        endColor.a *= 0.2f;
        trailRenderer.endColor = endColor;
    }
    
    private void UpdateSegmentedColors()
    {
        if (segmentedColors.Count == 0) return;
        
        // Create gradient with distinct color bands
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[segmentedColors.Count * 2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[segmentedColors.Count * 2];
        
        for (int i = 0; i < segmentedColors.Count; i++)
        {
            float position = (float)i / (segmentedColors.Count - 1);
            float nextPosition = (float)(i + 1) / (segmentedColors.Count - 1);
            
            // Create sharp transitions
            colorKeys[i * 2] = new GradientColorKey(segmentedColors[i], position);
            if (i < segmentedColors.Count - 1)
            {
                colorKeys[i * 2 + 1] = new GradientColorKey(segmentedColors[i], nextPosition - 0.01f);
            }
            
            alphaKeys[i * 2] = new GradientAlphaKey(1f, position);
            if (i < segmentedColors.Count - 1)
            {
                alphaKeys[i * 2 + 1] = new GradientAlphaKey(1f, nextPosition - 0.01f);
            }
        }
        
        // Ensure the last keys are set properly
        if (colorKeys.Length > 0)
        {
            colorKeys[colorKeys.Length - 1] = new GradientColorKey(segmentedColors[segmentedColors.Count - 1], 1f);
            alphaKeys[alphaKeys.Length - 1] = new GradientAlphaKey(0.2f, 1f); // Fade out at end
        }
        
        gradient.SetKeys(colorKeys, alphaKeys);
        trailRenderer.colorGradient = gradient;
    }
    
    private void UpdateSegmentedEffect()
    {
        if (isSegmentedActive)
        {
            segmentTimer += Time.deltaTime;
            if (segmentTimer >= segmentDuration)
            {
                isSegmentedActive = false;
                segmentTimer = 0f;
            }
        }
        
        // Update shader parameters if using custom shader
        if (trailMaterial != null)
        {
            if (trailMaterial.HasProperty("_Segments"))
            {
                float targetSegments = isSegmentedActive ? colorSegments : 1;
                float currentSegments = Mathf.Lerp(
                    trailMaterial.GetFloat("_Segments"), 
                    targetSegments, 
                    5f * Time.deltaTime
                );
                trailMaterial.SetFloat("_Segments", currentSegments);
            }
        }
    }
    
    private void UpdateTrailLifetime()
    {
        float targetLifetime = currentVelocity > velocityThreshold ? fastMovementLifetime : dynamicLifetime;
        trailRenderer.time = Mathf.Lerp(trailRenderer.time, targetLifetime, 2f * Time.deltaTime);
    }
    
    public void TriggerSegmentedEffect(Color hitColor)
    {
        // Add the hit color to our segmented palette
        if (segmentedColors.Count > 0)
        {
            segmentedColors[0] = hitColor; // Replace first color with hit color
        }
        
        isSegmentedActive = true;
        segmentTimer = 0f;
        
        // Boost trail temporarily
        StartCoroutine(TemporaryTrailBoost());
    }
    
    private IEnumerator TemporaryTrailBoost()
    {
        float originalMaxWidth = maxWidth;
        float originalLifetime = fastMovementLifetime;
        
        maxWidth *= 1.4f;
        fastMovementLifetime *= 1.2f;
        
        yield return new WaitForSeconds(segmentDuration);
        
        maxWidth = originalMaxWidth;
        fastMovementLifetime = originalLifetime;
    }
    
    public void SetSegmentedMode(bool enabled)
    {
        useSegmentedMode = enabled;
    }
}