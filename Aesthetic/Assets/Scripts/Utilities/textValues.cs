using TMPro;
using UnityEngine;
using Febucci.UI;
using System.Collections;


public class textValues : MonoBehaviour
{
    [SerializeField] private string[] textOptions;
    [SerializeField] private TextMeshPro gameText;
    [SerializeField] private TypewriterByWord typewriter;

    void OnEnable()
    {
        gameText.text = textOptions[Random.Range(0, textOptions.Length)];
        StartCoroutine(FadeOutAndDisable());
    }

    IEnumerator FadeOutAndDisable()
    {
        yield return new WaitForSeconds(0.5f);
        // Start fade-out animation
        typewriter.StartDisappearingText();

        yield return new WaitForSeconds(0.6f);

        gameObject.SetActive(false);
    }
}
