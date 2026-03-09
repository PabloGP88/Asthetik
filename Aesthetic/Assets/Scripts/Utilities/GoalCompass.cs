using UnityEngine;
using UnityEngine.UI;

public class GoalCompass : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform goal;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameLoop gameLoop; // Add reference to GameLoop
    
    [Header("Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab; // Simple arrow UI image
    [SerializeField] private Canvas compassCanvas;
    [SerializeField] private float edgeBuffer = 50f; // Distance from screen edge
    [SerializeField] private float arrowDistance = 150f; // How far from player center
    [SerializeField] private bool useOrbitMode = true; // Orbit around player instead of screen center
    [SerializeField] private float minOrbitDistance = 100f; // Minimum distance from player
    [SerializeField] private float maxOrbitDistance = 200f; // Maximum distance from player
    
    [Header("Visual")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseScale = 0.2f;
    [SerializeField] private float fadeDistance = 15f; // Hide arrow when goal is close
    
    [Header("Shadow/Outline")]
    [SerializeField] private bool useShadow = true;
    [SerializeField] private Vector2 shadowOffset = new Vector2(2f, -2f);
    [SerializeField] private Color shadowColor = new Color(0, 0, 0, 0.5f);
    
    private GameObject currentArrow;
    private GameObject shadowArrow;
    private Image arrowImage;
    private Image shadowImage;
    private RectTransform arrowRect;
    private RectTransform shadowRect;
    private Vector3 baseScale;
    
    void Start()
    {
        CreateArrow();
        baseScale = currentArrow.transform.localScale;
    }
    
    void Update()
    {
        if (goal == null || player == null) return;
        
        // Hide arrow when game is won
        if (gameLoop != null && gameLoop.haveWon)
        {
            HideArrow();
            return;
        }
        
        UpdateArrowDirection();
        UpdateArrowVisibility();
        UpdateArrowPulse();
    }
    
    private void CreateArrow()
    {
        if (arrowPrefab == null) return;
        
        // Create shadow first (renders behind)
        if (useShadow)
        {
            shadowArrow = Instantiate(arrowPrefab, compassCanvas.transform);
            shadowImage = shadowArrow.GetComponent<Image>();
            shadowRect = shadowArrow.GetComponent<RectTransform>();
            shadowImage.color = shadowColor;
            shadowArrow.name = "Arrow Shadow";
        }
        
        // Create main arrow
        currentArrow = Instantiate(arrowPrefab, compassCanvas.transform);
        arrowImage = currentArrow.GetComponent<Image>();
        arrowRect = currentArrow.GetComponent<RectTransform>();
        currentArrow.name = "Goal Arrow";
        
        // Start with white color - will be updated when goal is set
        arrowImage.color = Color.white;
        
        baseScale = currentArrow.transform.localScale;
    }
    
    private void UpdateArrowDirection()
    {
        // Calculate direction from player to goal
        Vector3 direction = (goal.position - player.position).normalized;
        
        Vector3 arrowPosition;
        
        if (useOrbitMode)
        {
            // Orbit mode: Arrow rotates around player position
            Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(player.position);
            
            // Calculate distance based on how far the goal is
            float goalDistance = Vector3.Distance(player.position, goal.position);
            float dynamicDistance = Mathf.Lerp(minOrbitDistance, maxOrbitDistance, 
                                             Mathf.Clamp01(goalDistance / 20f)); // Adjust 20f based on your game scale
            
            // Position arrow around player at calculated distance
            arrowPosition = playerScreenPos + (new Vector3(direction.x, direction.y, 0) * dynamicDistance);
        }
        else
        {
            // Original mode: Arrow around screen center
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            arrowPosition = screenCenter + (new Vector3(direction.x, direction.y, 0) * arrowDistance);
        }
        
        // Clamp to screen edges with buffer
        arrowPosition.x = Mathf.Clamp(arrowPosition.x, edgeBuffer, Screen.width - edgeBuffer);
        arrowPosition.y = Mathf.Clamp(arrowPosition.y, edgeBuffer, Screen.height - edgeBuffer);
        
        // Smooth position transition for less jittery movement
        Vector3 currentPos = arrowRect.position;
        Vector3 smoothedPosition = Vector3.Lerp(currentPos, arrowPosition, 8f * Time.deltaTime);
        
        // Set arrow position
        arrowRect.position = smoothedPosition;
        
        // Set shadow position (offset)
        if (useShadow && shadowRect != null)
        {
            shadowRect.position = smoothedPosition + (Vector3)shadowOffset;
        }
        
        // Rotate arrow to point toward goal
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        
        // Smooth rotation transition
        arrowRect.rotation = Quaternion.Lerp(arrowRect.rotation, targetRotation, 10f * Time.deltaTime);
        if (useShadow && shadowRect != null)
        {
            shadowRect.rotation = arrowRect.rotation;
        }
    }
    
    private void UpdateArrowVisibility()
    {
        // Calculate distance to goal
        float distance = Vector3.Distance(player.position, goal.position);
        
        // Fade out when very close to goal
        float alpha = Mathf.Clamp01(distance / fadeDistance);
        
        // Check if goal is on screen
        Vector3 goalScreenPos = mainCamera.WorldToScreenPoint(goal.position);
        bool isOnScreen = goalScreenPos.x > 50 && goalScreenPos.x < Screen.width - 50 && 
                         goalScreenPos.y > 50 && goalScreenPos.y < Screen.height - 50 && 
                         goalScreenPos.z > 0;
        
        // In orbit mode, keep arrow more visible even when goal is on screen
        if (isOnScreen && useOrbitMode)
        {
            alpha *= 0.6f; // Less fading in orbit mode
        }
        else if (isOnScreen)
        {
            alpha *= 0.3f; // More fading in classic mode
        }
        
        // Update main arrow alpha
        Color currentColor = arrowImage.color;
        currentColor.a = alpha;
        arrowImage.color = currentColor;
        
        // Update shadow alpha
        if (useShadow && shadowImage != null)
        {
            Color shadowCurrentColor = shadowColor;
            shadowCurrentColor.a = shadowColor.a * alpha;
            shadowImage.color = shadowCurrentColor;
        }
    }
    
    private void UpdateArrowPulse()
    {
        // Gentle pulsing animation
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        Vector3 pulsedScale = baseScale * pulse;
        
        currentArrow.transform.localScale = pulsedScale;
        if (useShadow && shadowArrow != null)
        {
            shadowArrow.transform.localScale = pulsedScale;
        }
    }
    
    public void SetGoal(Transform newGoal)
    {
        goal = newGoal;
        
        // Update arrow color to match new goal
        if (goal != null && arrowImage != null)
        {
            SpriteRenderer goalSprite = goal.GetComponent<SpriteRenderer>();
            if (goalSprite != null)
            {
                Color goalColor = goalSprite.color;
                goalColor.a = arrowImage.color.a; // Preserve current alpha
                arrowImage.color = goalColor;
            }
        }
    }
    
    public void UpdateGoalColor(Color newColor)
    {
        if (arrowImage != null)
        {
            Color currentColor = arrowImage.color;
            newColor.a = currentColor.a; // Preserve alpha
            arrowImage.color = newColor;
        }
    }
    
    public void HideArrow()
    {
        if (currentArrow != null)
        {
            currentArrow.SetActive(false);
        }
        if (shadowArrow != null)
        {
            shadowArrow.SetActive(false);
        }
    }
    
    public void ShowArrow()
    {
        if (currentArrow != null)
        {
            currentArrow.SetActive(true);
        }
        if (shadowArrow != null && useShadow)
        {
            shadowArrow.SetActive(true);
        }
    }
    
    private void OnDestroy()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (shadowArrow != null)
        {
            Destroy(shadowArrow);
        }
    }
}