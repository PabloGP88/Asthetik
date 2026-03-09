using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalEffect : MonoBehaviour
{
    private Animator animator;
    private const string ANIM_STRING = "OnGoalEffect";
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(ANIM_STRING,0,0f);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            gameObject.SetActive(false);
        }
    }
}
