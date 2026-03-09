using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip[] audioClips;
    // 0 pause sound on - 1 pause sound off - 2 swoosh 
    private bool inPause = false;

    public void PlayPauseOn()
    {
        audioSource1.clip = audioClips[0];
        audioSource1.pitch = 1f;
        audioSource1.Play();
        inPause = true;
    }
    public void PlayPauseOff()
    {
        audioSource1.clip = audioClips[1];
        audioSource1.pitch = .97f;
        audioSource1.Play();
        inPause = false;
    }
    public void PlaySwoosh1()
    {
        audioSource2.clip = audioClips[2];
        audioSource2.Play();
    }
    public void PlaySwoosh2()
    {
        audioSource2.clip = audioClips[3];
        audioSource2.Play();
    }
    public void PlaySwoosh3()
    {
        audioSource2.clip = audioClips[3];
        audioSource2.Play();
    }
    public bool GetPause()
    {
        return inPause;
    }
}
