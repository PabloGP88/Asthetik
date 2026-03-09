using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class TextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject textEffectPrefab;
    [SerializeField] private int spawnAmount = 8;
    [SerializeField] private bool enableVibration = true;
    [SerializeField] private float vibrationDuration = 0.1f;
    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject obj = Instantiate(textEffectPrefab, Vector3.zero, quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject available = GetAvailableObject();
        if (available != null)
        {
            available.transform.position = other.transform.position;

            float randomZ = UnityEngine.Random.Range(-25f, 25f);
            available.transform.rotation = Quaternion.Euler(0f, 0f, randomZ);

            available.SetActive(true);

            // Add subtle vibration after spawning text
            if (enableVibration)
            {
                TriggerVibration();
            }
        }
    }

    private void TriggerVibration()
    {
        // Check if running on mobile platform
        if (Application.isMobilePlatform)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // For Android - use custom vibration duration
            AndroidVibrate((long)(vibrationDuration * 1000)); // Convert to milliseconds
#elif UNITY_IOS && !UNITY_EDITOR
            // For iOS - use system haptic feedback (subtle)
            Handheld.Vibrate();
#else
            // Fallback for other mobile platforms
            Handheld.Vibrate();
#endif
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void AndroidVibrate(long milliseconds)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                {
                    vibrator.Call("vibrate", milliseconds);
                }
            }
        }
    }
#endif

    private GameObject GetAvailableObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // No available object
        return null;
    }
}
