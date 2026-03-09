using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CustomArtStyleCard : MonoBehaviour
{
    [SerializeField] private int cardIndex;
    [SerializeField] private int currentIndex;
    [SerializeField] private Sprite[] icons;
    [SerializeField] private string[] names;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject emptyCard;
    [SerializeField] private GameObject fullCard;
    [SerializeField] private Image splash;
    [SerializeField] private TextMeshProUGUI nameText;

    private CustomStyleComponent[] customStyleComponents;

    void Start()
    {
        customStyleComponents = Resources.FindObjectsOfTypeAll<CustomStyleComponent>();
        currentIndex = PlayerPrefs.GetInt("CustomArt" + cardIndex.ToString(), currentIndex);
        RefreshUI();
    }
    public void SetCustomButtons()
    {
        foreach (CustomStyleComponent customStyle in customStyleComponents)
        {
            customStyle.SetDestCard(cardIndex);
        }
        UISoundManager.Instance.PlaySoundEffect(6, 1, UISoundManager.MEDIUM_VOLUME);
        panel.SetActive(true);
    }
    public void SetCurrentIndex(int index)
    {
        currentIndex = index;
        RefreshUI();
    }
    private void RefreshUI()
    {
        if (currentIndex < 0)
        {
            emptyCard.SetActive(true);
            fullCard.SetActive(false);
        }
        else
        {
            emptyCard.SetActive(false);
            splash.sprite = icons[currentIndex];
            nameText.text = names[currentIndex];
            fullCard.SetActive(true);
        }
    }

    public void SaveCustomArt()
    {
        PlayerPrefs.SetInt("CustomArt" + cardIndex.ToString(), currentIndex);
    }
}
