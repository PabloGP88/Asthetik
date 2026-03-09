using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayeCollisionManager : MonoBehaviour
{
    [SerializeField] private MMF_Player goalHittedFeedback;
    public int goalScore {get; private set;}
    public event Action OnPlayerHitGoal;
    
    [Header ("Camera Shake")]
    private const string GOAL_STRING = "Goal";

    private void Start()
    {
        goalScore = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case GOAL_STRING:
                OnPlayerHitGoal?.Invoke();
                GoalHitted();
                GoalHittedSoundEffect();
                break;
        }
    }
    private void GoalHitted()
    {
        goalScore++;
        goalHittedFeedback.PlayFeedbacks();
    }

    private void GoalHittedSoundEffect()
    {
        float pitch_1 =  UnityEngine.Random.Range(0.9f,1f);
        float pitch_2 =  UnityEngine.Random.Range(0.9f,0.8f);

        SoundManager.Instance.PlaySplashffect_Source1(pitch_1, SoundManager.EFFECTS_VOLUME_M);
        SoundManager.Instance.PlaySplashffect_Source2(pitch_2, SoundManager.EFFECTS_VOLUME_S);
    }

}
