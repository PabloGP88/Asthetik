using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmountSlider : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetInt("AmountStrokes", 8);
    }

    void Update()
    {
        PlayerPrefs.SetInt("AmountStrokes", (int)slider.value);
        text.text = slider.value.ToString("F0");
    }
    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
