using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SizeSlider : MonoBehaviour
{
    enum Type
    {
        Min,
        Max
    }
    [SerializeField] private Type type;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;

    void Start()
    {
        switch ((int)type)
        {
            case 0:
                slider.value = PlayerPrefs.GetFloat("MinSize", 1f);
                break;

            case 1:
                slider.value = PlayerPrefs.GetFloat("MaxSize", 3f);
                break;
        }
    }
    void Update()
    {
        switch ((int)type)
        {
            case 0:
                PlayerPrefs.SetFloat("MinSize", slider.value);
                text.text = slider.value.ToString("F1");
                image.transform.localScale = new Vector3(
                    slider.value + 1,
                    slider.value + 1,
                    slider.value + 1);
                break;

            case 1:
                PlayerPrefs.SetFloat("MaxSize", slider.value);
                text.text = slider.value.ToString("F1");
                image.transform.localScale = new Vector3(
                    slider.value + 1,
                    slider.value + 1,
                    slider.value + 1);
                break;
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
