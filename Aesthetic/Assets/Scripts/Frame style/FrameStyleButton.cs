using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameStyleButton : MonoBehaviour
{
    enum FrameStyles
    {
        Basic,      // Index 0
        Lines,      // Index 1
        Chess,
        Circle,     
        Triangular, 
        Dots,      
        Waves       
    }

    [Header("Frame Material")]
    [SerializeField] private Animator animator;
    [SerializeField] private Material frameMaterial;
    [SerializeField] private Image framePreview;
    [SerializeField] private GameObject shadow;
    [SerializeField] private FrameStyles frameStyle;
    [SerializeField] private TextMeshProUGUI styleName;
    [SerializeField] private TextMeshProUGUI labelName;
    private const string FRAME_STRING = "frameStyle";

    void Awake()
    {
        labelName.text = frameStyle.ToString();
        
        if (PlayerPrefs.GetInt(FRAME_STRING, 0) == (int)frameStyle)
        {
            framePreview.material = frameMaterial;
            styleName.text = frameStyle.ToString();
        }
    }
    public void SetFrameStyle()
    {
        if (PlayerPrefs.GetInt(FRAME_STRING, 0) == (int)frameStyle)
        {
            UISoundManager.Instance.PlaySoundEffect(1, 1, UISoundManager.MEDIUM_VOLUME);
            animator.Play("frame already picked", 0, 0f);
        }
        else
        {
            PlayerPrefs.SetInt(FRAME_STRING, (int)frameStyle);
            framePreview.material = frameMaterial;
            styleName.text = frameStyle.ToString();

            float pitch = Random.Range(0.98f, 1.1f);
            UISoundManager.Instance.PlaySoundEffect(0, pitch, UISoundManager.MEDIUM_VOLUME);
            animator.Play("frame picked", 0, 0f);
        }
    }
    void Update()
    {
        if (PlayerPrefs.GetInt(FRAME_STRING, 0) == (int)frameStyle)
        {
            shadow.SetActive(true);
        }
        else
        {
            shadow.SetActive(false);
        }
    }

}
