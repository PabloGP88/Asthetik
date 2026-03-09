using UnityEngine;
using UnityEngine.UI;

public class CurrentColorPalette : MonoBehaviour
{
    [Header("Palette Images")]
    [SerializeField] private GameObject bgColor;
    [SerializeField] private GameObject otherColor1;
    [SerializeField] private GameObject otherColor2;
    [SerializeField] private GameObject otherColor3;

    [Header("Descriptions")]
    [SerializeField] private GameObject descriptionBg;
    [SerializeField] private GameObject descriptionPalette;

    [Header("References")]
    [SerializeField] private ColorPalettes colorPalettes;
    [SerializeField] private ParticleSystem bgSplashEffect;
    [SerializeField] private ParticleSystem palette1SplashEffect;
    [SerializeField] private ParticleSystem palette2SplashEffect;
    [SerializeField] private ParticleSystem palette3SplashEffect;

    void Start()
    {
        ColorPalettes.ColorPalette currentPalette = colorPalettes.GetCurentPalette();
        if (currentPalette == null)
        {
            Debug.LogError("No current palette found!");
            return;
        }

        // Set background and palette colors
        SetImageColor(bgColor, currentPalette.bgColor);
        SetImageColor(otherColor1, currentPalette.colors[0]);
        SetImageColor(otherColor2, currentPalette.colors[1]);
        SetImageColor(otherColor3, currentPalette.colors[2]);
    }

    public void SetPaletteColor()
    {
        // Get the current palette
        ColorPalettes.ColorPalette currentPalette = colorPalettes.GetCurentPalette();
        if (currentPalette == null)
        {
            Debug.LogError("No current palette found!");
            return;
        }

        // Set background and palette colors
        SetImageColor(bgColor, currentPalette.bgColor);
        SetImageColor(otherColor1, currentPalette.colors[0]);
        SetImageColor(otherColor2, currentPalette.colors[1]);
        SetImageColor(otherColor3, currentPalette.colors[2]);

        // Particle Effects
        SetParticleEffect(bgSplashEffect, currentPalette.bgColor);
        SetParticleEffect(palette1SplashEffect, currentPalette.colors[0]);
        SetParticleEffect(palette2SplashEffect, currentPalette.colors[1]);
        SetParticleEffect(palette3SplashEffect, currentPalette.colors[2]);

        // Animations
        PlayAnimationIfNotPlaying(bgColor.GetComponent<Animator>(), "Bg_Color_Anim");
        PlayAnimationIfNotPlaying(otherColor1.GetComponent<Animator>(), "palette_1_anim");
        PlayAnimationIfNotPlaying(otherColor2.GetComponent<Animator>(), "palette_2_anim");
        PlayAnimationIfNotPlaying(otherColor3.GetComponent<Animator>(), "palette_3_anim");
        PlayAnimationIfNotPlaying(descriptionBg.GetComponent<Animator>(), "description_bg_icon");
        PlayAnimationIfNotPlaying(descriptionPalette.GetComponent<Animator>(), "desciption_palette_icon");
    }

    private void SetImageColor(GameObject obj, Color color)
    {
        Image mainImage = obj.transform.GetChild(1).GetComponent<Image>();
        mainImage.color = color;

        Image shadowImage = obj.transform.GetChild(0).GetComponent<Image>();
        shadowImage.color = DarkenColor(color, 0.5f);
    }

    private void SetParticleEffect(ParticleSystem particleSystem, Color color)
    {
        if (!particleSystem.isPlaying)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = color;
            particleSystem.Play();
        }
    }

    private void PlayAnimationIfNotPlaying(Animator animator, string animationName)
    {
        animator.Play(animationName, 0, 0f);
    }

    /// <summary>
    /// Darkens a given color by reducing its RGB values by a multiplier.
    /// </summary>
    /// <param name="color">The original color.</param>
    /// <param name="multiplier">The amount to darken the color (0.5 = 50% darker).</param>
    /// <returns>The darkened color.</returns>
    private Color DarkenColor(Color color, float multiplier)
    {
        multiplier = Mathf.Clamp01(multiplier);
        return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a);
    }
}
