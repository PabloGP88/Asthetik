using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private PlayeCollisionManager player;
    [SerializeField] private SpriteRenderer colorSprite;
    [SerializeField] private float minSize,maxSize; 
    [SerializeField] private GoalSplashPool goalSplashPool;
    private int zPos = 100;

    private void OnEnable() => player.OnPlayerHitGoal += HandlePlayerHitGoal;
    private void OnDisable() => player.OnPlayerHitGoal -= HandlePlayerHitGoal;
    private void Start()
    {
        minSize = PlayerPrefs.GetFloat("MinSize", 1.0f);
        maxSize =  PlayerPrefs.GetFloat("MaxSize", 3.0f);
    }
    private void HandlePlayerHitGoal()
    {
        GameObject splash = GoalSpawnManager.Instance.GetPooledSplash();

        float size = Random.Range(minSize, maxSize);
        float rotation = Random.Range(-360f, 360f);

        Color color = colorSprite.color;
        //color.a = Random.Range(0.8f,1.5f);
        splash.GetComponent<SpriteRenderer>().color = color;
        splash.GetComponent<SpriteRenderer>().sprite = goalSplashPool.GetSplash();

        splash.transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        zPos--;
        splash.transform.localScale = new Vector3(size, size, 1);
        splash.transform.Rotate(0, 0, rotation);

        splash.SetActive(true);
    }
}
