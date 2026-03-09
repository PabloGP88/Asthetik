using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StyleComponent : MonoBehaviour
{
    [SerializeField] private int artStyleIndex;

    [Header ("References")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Image iconSpot;
    [SerializeField] private string styleName;
    [SerializeField] private TextMeshProUGUI styleNameText;
    [SerializeField] private GameObject shadow;
    [SerializeField] private PreviewSplash previewSplash;
    private Animator animator;
    private void Start()
    {
        SetUI();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        SetShadow();
    }
    public void SetArtStyle()
    {
        if (previewSplash.GetCurerntCollectionIndex() == artStyleIndex)
        {
            UISoundManager.Instance.PlaySoundEffect(1,1,UISoundManager.MEDIUM_VOLUME);
            animator.Play("style component already picked", 0, 0f);

        } else {

            previewSplash.SetActiveCollection(artStyleIndex);
            
            float pitch = Random.Range(0.98f,1.1f);
            UISoundManager.Instance.PlaySoundEffect(0,pitch,UISoundManager.MEDIUM_VOLUME);
            animator.Play("style component picked", 0, 0f);
        }
    }
    private void SetShadow()
    {
        if (previewSplash.GetCurerntCollectionIndex() == artStyleIndex)
        {
            shadow.SetActive(true);
        } else shadow.SetActive(false);
    }
    private void SetUI()
    {
        iconSpot.sprite = icon;
        styleNameText.text = styleName;
    }
}
