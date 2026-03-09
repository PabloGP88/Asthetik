using System.Collections.Generic;
using UnityEngine;

public class GoalSplashPool : MonoBehaviour
{
    [System.Serializable]
    public class GoalConfig
    {
        public GoalStateManager.States state;
        public Sprite mainSprite;
        public Sprite backgroundSprite; 
        public Sprite[] splashes;
    }

    [Header("Goal Configurations")]
    [SerializeField] private List<GoalConfig> goalConfigs;

    [Header("References")]
    [SerializeField] private GoalStateManager goalStateManager;
    [SerializeField] private SpriteRenderer mainSpriteRenderer;
    [SerializeField] private SpriteRenderer effectSpriteRenderer;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    private Dictionary<GoalStateManager.States, GoalConfig> configMap;

    void Awake()
    {
        configMap = new Dictionary<GoalStateManager.States, GoalConfig>();
        foreach (var config in goalConfigs)
        {
            configMap[config.state] = config;
        }
    }

    void Start()
    {
        ChangeGoalSprite();
    }

    public Sprite GetSplash()
    {
        var state = goalStateManager.GetCurrentState();
        if (configMap.TryGetValue(state, out var config))
        {
            var splashes = config.splashes;
            return splashes[Random.Range(0, splashes.Length)];
        }
        Debug.LogWarning($"No splash found for state: {state}");
        return null;
    }

    public void ChangeGoalSprite()
    {
        var state = goalStateManager.GetCurrentState();
        if (configMap.TryGetValue(state, out var config))
        {
            mainSpriteRenderer.sprite = config.mainSprite;
            effectSpriteRenderer.sprite = config.mainSprite;
            backgroundSpriteRenderer.sprite = config.backgroundSprite; // Uses the separate background sprite
        }
        else
        {
            Debug.LogWarning($"No sprite configuration found for state: {state}");
        }
    }
}

