using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    [SerializeField] private Transform goal;
    [SerializeField] private PlayeCollisionManager player;
    [SerializeField] private GameLoop gameLoop;
    private const float MAX_SPEED = 9f;
    private Vector3 previous,dir;
    private float cameraSpeed;
    private const float DESVIATON = 1.5f;

    private void Update()
    {
       /* if (gameLoop.haveWon || gameLoop.haveLost) { return; }
        
        float speed = Mathf.Clamp((cameraSpeed + (player.goalScore / player.goalScore ) + 4.5f),0,MAX_SPEED)  * Time.deltaTime;

        if (previous != goal.position)
        {
            Vector3 goalPos = new Vector3
            (goal.position.x + Random.Range(DESVIATON,-DESVIATON)
            ,goal.position.y + Random.Range(DESVIATON,-DESVIATON),
            goal.position.z
            );

            dir = -(transform.position - goalPos).normalized;
            previous = goal.position;
        } else 
        {
            transform.position += dir * speed;
        }  */
    }

}
