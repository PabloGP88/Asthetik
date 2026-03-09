using UnityEngine;

public class PaletteButtonLogic : MonoBehaviour
{
    [SerializeField] private int paletteNumber;
    [SerializeField] private GameObject colorSelectedShadow;
    [SerializeField] private ColorPalettes colorPalettes;
    private CurrentColorPalette currentColorPalette;
    private Animator animator;

    void Start()
    {
        if (transform.parent != null)
        {
            // Obtén el índice del hijo dentro de su objeto padre
            paletteNumber = transform.GetSiblingIndex();
        }

        animator = GetComponent<Animator>();
        currentColorPalette = FindFirstObjectByType<CurrentColorPalette>();
    }

    public void ChangeCurrentPalette()
    {

        if (colorPalettes.GetCurrentPaletteIndex() == paletteNumber)
        {
            // Play Sound Effect
            UISoundManager.Instance.PlaySoundEffect(1,1,UISoundManager.MEDIUM_VOLUME);

            // Play Fail Anim
             animator.Play("color_already_picked", 0, 0f);
        } else 
        {
            colorPalettes.SetNewCurrentPalette(paletteNumber);
            currentColorPalette.SetPaletteColor();

            // Play Sound Effect
            float pitch = Random.Range(0.98f,1.1f);
            UISoundManager.Instance.PlaySoundEffect(0,pitch,UISoundManager.MEDIUM_VOLUME);

            // Play Fail Anim
             animator.Play("color_palette_picked", 0, 0f);
        }  

    }

    private void Update()
    {
        // Activate Shadow
        if (colorPalettes.GetCurrentPaletteIndex() == paletteNumber)
        {
            colorSelectedShadow.SetActive(true);
        } else 
        {
            colorSelectedShadow.SetActive(false);
        } 
    }

}
