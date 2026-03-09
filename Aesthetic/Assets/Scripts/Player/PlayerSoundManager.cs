using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> drawingEffects;
    private const float VOLUME = 0.9f;
    private Vector2 mouseStartPos;
    
    private void Awake()
    {
        audioSource.clip = drawingEffects[0];
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:

                    audioSource.volume = VOLUME;
                    break;

                default:
                    audioSource.volume = 0;
                    break;
            }

        } else if (Input.GetMouseButtonDown(0))
        {
            // When the mouse button is pressed, record the start position
            mouseStartPos = Input.mousePosition;
        }

        // Check if the left mouse button is being held down
        if (Input.GetMouseButton(0))
        {
            // If the mouse is moved while the button is held down, consider it a drag
            if (Input.mousePosition != (Vector3)mouseStartPos)
            {
                audioSource.volume = VOLUME;
            }
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            audioSource.volume = 0;
        }
    }
}
