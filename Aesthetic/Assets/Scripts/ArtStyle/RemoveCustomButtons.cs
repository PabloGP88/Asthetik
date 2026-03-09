using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCustomButtons : MonoBehaviour
{
    [SerializeField] private CustomArtStyleCard customArtStyleCard;
    public void RemoveCustomArt()
    {
        UISoundManager.Instance.PlaySoundEffect(9, 1, UISoundManager.MEDIUM_VOLUME);
        customArtStyleCard.SetCurrentIndex(-1);
    }
}
