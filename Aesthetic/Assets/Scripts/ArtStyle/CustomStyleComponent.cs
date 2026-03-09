using UnityEngine;

public class CustomStyleComponent : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private CustomArtStyleCard[] customArtStyleCardList;
    [SerializeField] private GameObject panel;
    [SerializeField] private int destinationCard;
    public void SetCustomArtStyle()
    {
        UISoundManager.Instance.PlaySoundEffect(6, 1.05f, UISoundManager.MEDIUM_VOLUME);
        customArtStyleCardList[destinationCard].SetCurrentIndex(index);
        panel.SetActive(false);
    }

    public void SetDestCard(int x)
    {
        destinationCard = x -1;
    }
}
