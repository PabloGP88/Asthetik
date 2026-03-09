using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ColorPicker : MonoBehaviour
{
    [Header("Color Selection Components")]
    [SerializeField] private RawImage saturationValueImage;
    [SerializeField] private RawImage hueBar;
    [SerializeField] private RectTransform svCursor;
    [SerializeField] private RectTransform hueCursor;
    
    [Header("Preview Components")]
    [SerializeField] private Image colorPreview;
    [SerializeField] private TMP_InputField hexInput;
    
    public event Action onColorSaved;

    private Texture2D svTexture;
    private Texture2D hueTexture;
    
    private float currentHue;
    private float currentSaturation;
    private float currentValue;
    private Color currentColor;
    
    private void Start()
    {
        InitializeTextures();
        SetupEventListeners();
        UpdateColor(Color.white);
    }
    
    private void InitializeTextures()
    {
        svTexture = new Texture2D(256, 256);
        saturationValueImage.texture = svTexture;
        
        hueTexture = new Texture2D(1, 256);
        hueBar.texture = hueTexture;
        
        UpdateHueTexture();
        UpdateSVTexture();
    }
    
    private void UpdateHueTexture()
    {
        for (int y = 0; y < hueTexture.height; y++)
        {
            float hue = y / (float)hueTexture.height;
            Color hueColor = Color.HSVToRGB(hue, 1, 1);
            hueTexture.SetPixel(0, y, hueColor);
        }
        hueTexture.Apply();
    }
    
    private void UpdateSVTexture()
    {
        for (int y = 0; y < svTexture.height; y++)
        {
            float value = y / (float)svTexture.height;
            for (int x = 0; x < svTexture.width; x++)
            {
                float saturation = x / (float)svTexture.width;
                Color color = Color.HSVToRGB(currentHue, saturation, value);
                svTexture.SetPixel(x, y, color);
            }
        }
        svTexture.Apply();
    }
    
    private void SetupEventListeners()
    {
        EventTrigger svTrigger = saturationValueImage.gameObject.AddComponent<EventTrigger>();
        EventTrigger hueTrigger = hueBar.gameObject.AddComponent<EventTrigger>();
        
        AddEventTriggerEntry(svTrigger, EventTriggerType.Drag, (data) => HandleSVChange((PointerEventData)data));
        AddEventTriggerEntry(svTrigger, EventTriggerType.PointerDown, (data) => HandleSVChange((PointerEventData)data));
        
        AddEventTriggerEntry(hueTrigger, EventTriggerType.Drag, (data) => HandleHueChange((PointerEventData)data));
        AddEventTriggerEntry(hueTrigger, EventTriggerType.PointerDown, (data) => HandleHueChange((PointerEventData)data));
        
        hexInput.onValueChanged.AddListener(HandleHexInput);
    }
    
    private void AddEventTriggerEntry(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => { action((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    private void HandleSVChange(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            saturationValueImage.rectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localPoint))
            return;

        localPoint.x = Mathf.Clamp(localPoint.x, 
            -saturationValueImage.rectTransform.rect.width/2, 
            saturationValueImage.rectTransform.rect.width/2);
        localPoint.y = Mathf.Clamp(localPoint.y, 
            -saturationValueImage.rectTransform.rect.height/2, 
            saturationValueImage.rectTransform.rect.height/2);
        
        Vector2 normalizedPoint = new Vector2(
            (localPoint.x / saturationValueImage.rectTransform.rect.width) + 0.5f,
            (localPoint.y / saturationValueImage.rectTransform.rect.height) + 0.5f
        );
        
        currentSaturation = normalizedPoint.x;
        currentValue = normalizedPoint.y;
        
        svCursor.anchoredPosition = localPoint;
        
        UpdateColorFromHSV();
    }
    
    private void HandleHueChange(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            hueBar.rectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localPoint))
            return;
        
        localPoint.y = Mathf.Clamp(localPoint.y, 
            -hueBar.rectTransform.rect.height/2, 
            hueBar.rectTransform.rect.height/2);
        
        float normalizedY = (localPoint.y / hueBar.rectTransform.rect.height) + 0.5f;
        currentHue = normalizedY;
        
        hueCursor.anchoredPosition = new Vector2(
            hueCursor.anchoredPosition.x,
            localPoint.y
        );
        
        UpdateSVTexture();
        UpdateColorFromHSV();
    }
    
    private void HandleHexInput(string hexValue)
    {
        if (hexValue.Length == 6)
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#" + hexValue, out newColor))
            {
                UpdateColor(newColor);
            }
        }
    }
    
    private void UpdateColorFromHSV()
    {
        currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);
        UpdatePreview();
    }
    
    private void UpdateColor(Color color)
    {
        Color.RGBToHSV(color, out currentHue, out currentSaturation, out currentValue);
        currentColor = color;
        
        UpdateSVTexture();
        UpdatePreview();
        UpdateCursorsFromHSV();
    }
    
    private void UpdatePreview()
    {
        colorPreview.color = currentColor;
        hexInput.text = ColorUtility.ToHtmlStringRGB(currentColor);
    }
    
    private void UpdateCursorsFromHSV()
    {
        svCursor.anchoredPosition = new Vector2(
            (currentSaturation - 0.5f) * saturationValueImage.rectTransform.rect.width,
            (currentValue - 0.5f) * saturationValueImage.rectTransform.rect.height
        );
        
        hueCursor.anchoredPosition = new Vector2(
            hueCursor.anchoredPosition.x,
            (currentHue - 0.5f) * hueBar.rectTransform.rect.height
        );
    }
    
    public Color GetCurrentColor()
    {
        return currentColor;
    }

    public void SetColor(Color newColor)
    {
        UpdateColor(newColor);
    }

    public void SaveCustomColor()
    {
        onColorSaved?.Invoke();
    }
}