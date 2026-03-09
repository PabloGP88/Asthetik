using UnityEngine;

public class GoalCollisionManager : MonoBehaviour
{
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private GoalMovement player;
    private float previousDistance;
    private float newDistance;
    private int count;
    private void Start()
    {
        previousDistance = Vector2.Distance(transform.position,player.transform.position);
    }

    private void HandlePlayerHitGoal()
    {
        newDistance = 0;
        previousDistance = 0;
    }
    private void LateUpdate()
    {
        previousDistance = newDistance;
    }
}
