using UnityEngine;

public class GoalBehaivor : MonoBehaviour
{
    [SerializeField] private Vector2 frameSize;
    [SerializeField] private SpriteRenderer colorSprite;
    [SerializeField] private PlayeCollisionManager player;
    [SerializeField] private GameLoop gameLoop;

    [SerializeField] private GoalSplashPool goalSplashPool;
    [SerializeField] private GoalCompass goalCompass;
    [SerializeField] private GoalStateManager goalStateManager;
    [SerializeField] private GoalSoundManager goalSoundManager;

    
    private GameObject goalEffect;
    private GameObject HitEffect;
    private const string PLAYER_STRING = "Player";
    private const float MIN_X_POS = 2f;
    private const float MIN_Y_POS = 2f;

    private void Start()
    {
        SetNewPos(MIN_X_POS,MIN_Y_POS);
    }

    private void Update()
    {
        if (gameLoop.haveWon)
        {
            gameObject.SetActive(false);
        }
        
        if (goalCompass != null)
        {
            goalCompass.SetGoal(transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case PLAYER_STRING:
                // Get goal color for effects
                Color goalColor = colorSprite.color;
                
                // 1. Enhanced camera impact
                FindFirstObjectByType<SmoothCameraController>()?.DoImpactEffects();
                
                // 2. Color burst effect
                ColorBurstEffect.Instance?.TriggerColorBurst(goalColor);
                
                // 3. Dramatic time slow
                TimeSlowEffect.Instance?.TriggerTimeSlow();
                
                colorSelector.Instance.SetColor();
                SpawnGoalEffect();
                SetNewPos(frameSize.x, frameSize.y);
                
                // Update compass
                if (goalCompass != null)
                {
                    goalCompass.UpdateGoalColor(colorSprite.color);
                }

                DynamicTrailSystem trailSystem = other.GetComponent<DynamicTrailSystem>();
                if (trailSystem != null)
                {
                    trailSystem.TriggerSegmentedEffect(colorSprite.color);
                }

                if (goalStateManager != null && goalStateManager.IsInCustomMode())
                {
                    goalStateManager.NextCustomArtState();
                }

                goalSoundManager.PlayCollectEffect();
                goalSplashPool.ChangeGoalSprite();
                break;
        }
    }

    private void SetNewPos(float x, float y)
    {
        transform.position = new Vector2(
            Random.Range(-x,x),
            Random.Range(-y,y)
        );
    }
    private void SpawnGoalEffect()
    {
        goalEffect = GoalSpawnManager.Instance.GetPooledCircelEffect();
        goalEffect.transform.position = transform.position;
        goalEffect.GetComponent<SpriteRenderer>().color = colorSprite.color;
        goalEffect.SetActive(true);

        HitEffect = GoalSpawnManager.Instance.GetPooledExplotionEffect();
        HitEffect.transform.position = transform.position;
        ParticleSystem.MainModule particleSettings =  HitEffect.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = colorSprite.color;
        HitEffect.SetActive(true);
        HitEffect.GetComponent<ParticleSystem>().Play();
    }
}
