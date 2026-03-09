using UnityEngine;
using Cinemachine;

public class SmoothCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private Transform player;
    [SerializeField] private Movment playerMovement; // To get velocity
    [SerializeField] private GameLoop gameLoop;

    [Header("Smooth Follow")]
    [SerializeField] private float followSmoothing = 5f;
    [SerializeField] private float velocityInfluence = 2f;
    [SerializeField] private float maxPrediction = 3f;

    [Header("Dynamic Zoom")]
    [SerializeField] private float baseZoom = 5f;
    [SerializeField] private float speedZoomFactor = 0.5f;
    [SerializeField] private float zoomSmoothing = 3f;
    [SerializeField] private float completionZoom = 8f; // Zoom out when painting complete

    [Header("Cinematic Feel")]
    [SerializeField] private float rotationSway = 2f;
    [SerializeField] private float bobAmplitude = 0.1f;
    [SerializeField] private float bobFrequency = 1f;

    [Header("Completion Behavior")]
    [SerializeField] private float completionMoveSpeed = 2f;

    [Header("Border Control")]
    [SerializeField] private float borderBuffer = 1.5f;
    [SerializeField] private float borderPushStrength = 2f;

    [Header("Impact Effects")]
    [SerializeField] private float impactPunchIntensity = 0.4f;
    [SerializeField] private float impactPunchDuration = 0.2f;
    [SerializeField] private float impactZoomPunch = 0.8f;  // Zoom in slightly on impact
    [SerializeField] private float impactZoomDuration = 0.3f;
    private Vector3 velocity;
    private Vector3 smoothVelocity;
    private Vector3 targetPosition;
    private float targetZoom;
    private Vector3 playerVelocity;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        targetZoom = baseZoom;
    }

    void LateUpdate()
    {
        // Get player velocity (you might need to modify based on your Movment script)
        if (playerMovement != null)
        {
            playerVelocity = playerMovement.GetVelocity();
        }

        if (gameLoop.haveWon)
        {
            HandleCompletionCamera();
        }
        else
        {
            HandleGameplayCamera();
        }
    }
    private void HandleGameplayCamera()
    {
        if (player == null) return;
        
        // Predictive following - camera leads slightly based on movement
        Vector3 prediction = playerVelocity * velocityInfluence;
        prediction = Vector3.ClampMagnitude(prediction, maxPrediction);
        
        // Border avoidance - push camera away from edges
        Vector3 borderOffset = CalculateBorderOffset();
        
        targetPosition = new Vector3(
            player.position.x + prediction.x + borderOffset.x,
            player.position.y + prediction.y + borderOffset.y,
            transform.position.z
        );
        
        // Rest of your existing code...
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref smoothVelocity, 
            1f / followSmoothing
        );
        
        // Dynamic zoom based on speed
        float speed = playerVelocity.magnitude;
        targetZoom = baseZoom + (speed * speedZoomFactor);
        
        // Smooth zoom
        mainCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            mainCamera.m_Lens.OrthographicSize, 
            targetZoom, 
            zoomSmoothing * Time.deltaTime
        );
        
        // Subtle cinematic effects
        ApplyCinematicEffects();
    }

    private void HandleCompletionCamera()
    {
        // Move to center to show full artwork
        Vector3 centerPosition = new Vector3(0, 0, transform.position.z);
        transform.position = Vector3.Lerp(
            transform.position,
            centerPosition,
            completionMoveSpeed * Time.deltaTime
        );

        // Zoom out to show complete painting
        mainCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            mainCamera.m_Lens.OrthographicSize,
            completionZoom,
            completionMoveSpeed * Time.deltaTime
        );

        // Remove cinematic effects for clean final view
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.identity,
            completionMoveSpeed * Time.deltaTime
        );
    }

    private void ApplyCinematicEffects()
    {
        // Subtle breathing effect
        float sway = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        // Tilt based on horizontal movement
        float tilt = playerVelocity.x * rotationSway;

        // Apply rotation smoothly
        Quaternion targetRotation = Quaternion.Euler(sway, 0, -tilt);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            2f * Time.deltaTime
        );
    }

    // Call this for screen shake effects on goal collection
    public void DoScreenPunch(float intensity = 0.5f, float duration = 0.1f)
    {
        // You can implement screen shake here or use a tween library
        StartCoroutine(ScreenPunchCoroutine(intensity, duration));
    }
    public void DoImpactEffects()
    {
        StartCoroutine(ImpactSequence());
    }

    private System.Collections.IEnumerator ImpactSequence()
    {
        // 1. Quick zoom in
        float originalZoom = mainCamera.m_Lens.OrthographicSize;
        float targetImpactZoom = originalZoom - impactZoomPunch;
        
        // 2. Screen shake + zoom in simultaneously
        StartCoroutine(ScreenPunchCoroutine(impactPunchIntensity, impactPunchDuration));
        
        float elapsed = 0f;
        while (elapsed < impactZoomDuration * 0.3f) // Quick zoom in (30% of duration)
        {
            float t = elapsed / (impactZoomDuration * 0.3f);
            mainCamera.m_Lens.OrthographicSize = Mathf.Lerp(originalZoom, targetImpactZoom, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // 3. Smooth zoom back out (70% of duration)
        elapsed = 0f;
        while (elapsed < impactZoomDuration * 0.7f)
        {
            float t = elapsed / (impactZoomDuration * 0.7f);
            mainCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetImpactZoom, originalZoom, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        mainCamera.m_Lens.OrthographicSize = originalZoom;
    }
    private System.Collections.IEnumerator ScreenPunchCoroutine(float intensity, float duration)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            // Elastic easing out for smooth impact
            float easedIntensity = intensity * (1f - (t * t * (3f - 2f * t)));
            
            float x = Random.Range(-1f, 1f) * easedIntensity;
            float y = Random.Range(-1f, 1f) * easedIntensity;
            
            transform.position = originalPos + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPos;
    }
    private Vector3 CalculateBorderOffset()
    {
        Vector3 offset = Vector3.zero;
        
        // Get camera bounds
        float cameraHeight = mainCamera.m_Lens.OrthographicSize;
        float cameraWidth = cameraHeight * mainCamera.m_Lens.Aspect;
        
        // Check if player is too close to borders and push camera accordingly
        Vector3 playerPos = player.position;
        Vector3 cameraPos = transform.position;
        
        // Calculate where player appears on screen relative to camera
        float playerScreenX = playerPos.x - cameraPos.x;
        float playerScreenY = playerPos.y - cameraPos.y;
        
        // Push camera if player is too close to edges
        if (playerScreenX > cameraWidth - borderBuffer)
        {
            offset.x = (playerScreenX - (cameraWidth - borderBuffer)) * borderPushStrength;
        }
        else if (playerScreenX < -cameraWidth + borderBuffer)
        {
            offset.x = (playerScreenX + (cameraWidth - borderBuffer)) * borderPushStrength;
        }
        
        if (playerScreenY > cameraHeight - borderBuffer)
        {
            offset.y = (playerScreenY - (cameraHeight - borderBuffer)) * borderPushStrength;
        }
        else if (playerScreenY < -cameraHeight + borderBuffer)
        {
            offset.y = (playerScreenY + (cameraHeight - borderBuffer)) * borderPushStrength;
        }
        
        return offset;
    }
}