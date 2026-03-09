using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField] private Material[] frameMaterials;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private const string FRAME_STRING = "frameStyle";

    void Awake()
    {
        spriteRenderer.material = frameMaterials[PlayerPrefs.GetInt(FRAME_STRING, 0)];
    }

}
