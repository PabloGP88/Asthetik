using UnityEngine;

public class GoalSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float pitchIncrement;


    void Start()
    {
        pitchIncrement = maxPitch - minPitch;
        pitchIncrement = pitchIncrement / (gameLoop.goalsAmount - 1);
        audioSource.pitch = minPitch - pitchIncrement;
    }
    public void PlayCollectEffect()
    {
        audioSource.pitch += pitchIncrement;
        audioSource.Play();
    }
}
