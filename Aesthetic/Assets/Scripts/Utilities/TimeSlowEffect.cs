using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class TimeSlowEffect : MonoBehaviour
{
    [Header("Time Slow Settings")]
    [SerializeField] private float slowTimeScale = 0.3f;
    [SerializeField] private float slowDuration = 0.5f;
    [SerializeField] private AnimationCurve timeScaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    [Header("Visual Drama")]
    [SerializeField] private PostProcessVolume postProcessVolume; 
    [SerializeField] private float vignetteIntensity = 0.4f;
    [SerializeField] private float chromaticAberrationIntensity = 0.5f;
    [SerializeField] private float saturationBoost = 0.3f;
    [SerializeField] private AnimationCurve effectCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    public static TimeSlowEffect Instance;
    
    // Cache references for performance
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private ColorGrading colorGrading;
    
    // Original values to restore
    private float originalVignetteIntensity;
    private float originalChromaticIntensity;
    private float originalSaturation;
    
    // Performance optimization
    private bool isEffectActive = false;
    private Coroutine currentEffect;
    
    void Awake()
    {
        Instance = this;
        CachePostProcessComponents();
    }
    
    private void CachePostProcessComponents()
    {        
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            // Cache components and original values - CORRECT WAY for Post-Processing Stack v2
            if (postProcessVolume.profile.TryGetSettings<Vignette>(out vignette))
            {
                originalVignetteIntensity = vignette.intensity.value;
            }
            
            if (postProcessVolume.profile.TryGetSettings<ChromaticAberration>(out chromaticAberration))
            {
                originalChromaticIntensity = chromaticAberration.intensity.value;
            }
            
            if (postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading))
            {
                originalSaturation = colorGrading.saturation.value;
            }
        }
        else
        {
            Debug.LogWarning("TimeSlowEffect: No PostProcessVolume or profile assigned!");
        }
    }
    
    public void TriggerTimeSlow()
    {
        // Prevent multiple effects running simultaneously
        if (isEffectActive) return;
        
        if (currentEffect != null)
        {
            StopCoroutine(currentEffect);
        }
        
        currentEffect = StartCoroutine(TimeSlowSequence());
    }
    
    private IEnumerator TimeSlowSequence()
    {
        isEffectActive = true;
        
        float elapsed = 0f;
        float originalTimeScale = Time.timeScale;
        
        // Pre-calculate target values for performance
        float targetVignette = originalVignetteIntensity + vignetteIntensity;
        float targetChromatic = originalChromaticIntensity + chromaticAberrationIntensity;
        float targetSaturation = originalSaturation + saturationBoost;
        
        while (elapsed < slowDuration)
        {
            float t = elapsed / slowDuration;
            float timeScaleCurveValue = timeScaleCurve.Evaluate(t);
            float effectCurveValue = effectCurve.Evaluate(t);
            
            // Update time scale
            Time.timeScale = Mathf.Lerp(slowTimeScale, originalTimeScale, timeScaleCurveValue);
            
            // Update visual effects (only if post-process is available)
            UpdateVisualEffects(effectCurveValue, targetVignette, targetChromatic, targetSaturation);
            
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Ensure everything is reset
        Time.timeScale = originalTimeScale;
        ResetVisualEffects();
        
        isEffectActive = false;
        currentEffect = null;
    }
    
    private void UpdateVisualEffects(float t, float targetVignette, float targetChromatic, float targetSaturation)
    {
        // Only update if components exist (performance check)
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(originalVignetteIntensity, targetVignette, t);
        }
        
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(originalChromaticIntensity, targetChromatic, t);
        }
        
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(originalSaturation, targetSaturation, t);
        }
    }
    
    private void ResetVisualEffects()
    {
        if (vignette != null)
        {
            vignette.intensity.value = originalVignetteIntensity;
        }
        
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = originalChromaticIntensity;
        }
        
        if (colorGrading != null)
        {
            colorGrading.saturation.value = originalSaturation;
        }
    }
    
    // Emergency reset if something goes wrong
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && isEffectActive)
        {
            Time.timeScale = 1f;
            ResetVisualEffects();
            isEffectActive = false;
        }
    }
    
    private void OnDestroy()
    {
        // Safety cleanup
        Time.timeScale = 1f;
        ResetVisualEffects();
    }
}