using TMPro;
using UnityEngine;

public class colorSelector : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private ColorPalettesManager colorPalettesManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private SpriteRenderer goal_1;
    [SerializeField] private SpriteRenderer goal_2;
    private int colorIndex = 0;
    private int currentIndex;
    private Color[] colors;
    private Gradient[] colors_trail;
    public static colorSelector Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (colorPalettesManager == null)
        {
            return;
        }

        // Wait for ColorPalettesManager to be ready
        if (!colorPalettesManager.isInitialized)
        {
            print("Waiting for ColorPalettesManager to initialize...");
        }

        currentIndex = colorIndex;
        colors = colorPalettesManager.GetActiveColors();
        
        if (colors == null)
        {
            return;
        }

        colors_trail = colorPalettesManager.GetActiveTrailGradients();
        SetColor();
    }
    
    public void SetColor()
    {        
        while (colorIndex == currentIndex)
        {
            colorIndex = Random.Range(0,colors.Length);
        }

        currentIndex = colorIndex;

        spriteRenderer.color = colors[currentIndex];
        goal_1.color = colors[currentIndex];
        goal_2.color = colors[currentIndex];
        trailRenderer.colorGradient = colors_trail[currentIndex];
    }

}