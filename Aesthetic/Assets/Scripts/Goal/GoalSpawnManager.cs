using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform hitEffectParent;
    [SerializeField] private Transform splashsEffectParent;
    [SerializeField] private Transform circleEffectParent;
    [SerializeField] private GameObject HitCircleEffect;
    [SerializeField] private GameObject HitExplotionEffect;
    [SerializeField] private GameObject Splash;
    [SerializeField] private int hitCircleAmount;
    [SerializeField] private int hitExplotionAmount;
    [SerializeField] private GameLoop gameLoop;
    private List<GameObject> hitCirclePool = new List<GameObject>();
    private List<GameObject> hitExplotionPool = new List<GameObject>();
    private List<GameObject> splashPool = new List<GameObject>();
    public static GoalSpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Create Circle Effect Pool List
        CreateInitialPool(hitCirclePool,HitCircleEffect,hitCircleAmount, circleEffectParent);
        // Create Hit Effect Pool List
        CreateInitialPool(hitExplotionPool, HitExplotionEffect, hitExplotionAmount, hitEffectParent);
        // Create Splash Pool List
        CreateInitialPool(splashPool, Splash, gameLoop.goalsAmount, splashsEffectParent);
    }

    private void CreateInitialPool(List<GameObject> list,GameObject prefab, int prefabAmount, Transform prefabParent)
    {
        for (int i = 0; i < prefabAmount; i++)
        {
            GameObject gameObject = Instantiate(prefab,prefabParent);
            gameObject.SetActive(false);
            
            list.Add(gameObject);
        }
    }

    public GameObject GetPooledCircelEffect()
    {
        for (int i = 0; i < hitCirclePool.Count; i++)
        {
            if (!hitCirclePool[i].activeInHierarchy)
            {
                GameObject prefab = hitCirclePool[i];
                return prefab;
            }
        }

        return null;
    }
    public GameObject GetPooledExplotionEffect()
    {
        for (int i = 0; i < hitExplotionPool.Count; i++)
        {
            if (!hitExplotionPool[i].activeInHierarchy)
            {
                GameObject prefab = hitExplotionPool[i];
                return prefab;
            }
        }

        return null;
    }
    public GameObject GetPooledSplash()
    {
        for (int i = 0; i < splashPool.Count; i++)
        {
            if (!splashPool[i].activeInHierarchy)
            {
                GameObject prefab = splashPool[i];
                return prefab;
            }
        }

        return null;
    }
}
