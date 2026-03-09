using UnityEngine;

public class RandomSettings : MonoBehaviour
{
    [SerializeField] private SizeSlider minSize;
    [SerializeField] private SizeSlider maxSize;
    [SerializeField] private AmountSlider amountSlider;

    public void RandomValues()
    {
        UISoundManager.Instance.PlaySoundEffect(0, Random.Range(0.95f, 1.1f), UISoundManager.MEDIUM_VOLUME);
        minSize.SetSliderValue(Random.Range(1.0f, 3.0f));
        maxSize.SetSliderValue(Random.Range(1.0f, 3.0f));
        amountSlider.SetSliderValue((int)Random.Range(1,30));
    }
}
