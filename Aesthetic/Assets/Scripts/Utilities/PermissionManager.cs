using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
   public static PermissionManager Instance { get; private set; }
    private bool? permissionsGranted = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Verificar permisos inmediatamente al inicializar
        if (Application.platform == RuntimePlatform.Android)
        {
            CheckAndRequestPermissions();
        }
        else
        {
            permissionsGranted = true;
        }
    }

    private void CheckAndRequestPermissions()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            int sdkInt = version.GetStatic<int>("SDK_INT");

            if (sdkInt >= 33) // Android 13 o superior
            {
                string[] permissions = new string[] {
                    "android.permission.READ_MEDIA_IMAGES"
                };
                
                // Primero verifica si ya tenemos los permisos
                bool hasPermissions = CheckAndroid13Permissions(activity, permissions);
                if (hasPermissions)
                {
                    permissionsGranted = true;
                    return;
                }
                RequestPermissionsAndroid13(activity, permissions);
            }
            else // Android 12 o inferior
            {
                string[] permissions = new string[] {
                    Permission.ExternalStorageRead,
                    Permission.ExternalStorageWrite
                };
                
                // Primero verifica si ya tenemos los permisos
                bool hasPermissions = CheckLegacyPermissions(permissions);
                if (hasPermissions)
                {
                    permissionsGranted = true;
                    return;
                }
                RequestPermissionsLegacy(activity, permissions);
            }
        }
    }

    private bool CheckAndroid13Permissions(AndroidJavaObject activity, string[] permissions)
    {
        try
        {
            using (var contextCompat = new AndroidJavaClass("androidx.core.content.ContextCompat"))
            {
                foreach (string permission in permissions)
                {
                    if (contextCompat.CallStatic<int>("checkSelfPermission", activity, permission) != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    private bool CheckLegacyPermissions(string[] permissions)
    {
        foreach (string permission in permissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                return false;
            }
        }
        return true;
    }

    private void RequestPermissionsAndroid13(AndroidJavaObject activity, string[] permissions)
    {
        try
        {
            using (var activityCompat = new AndroidJavaClass("androidx.core.app.ActivityCompat"))
            {
                activityCompat.CallStatic("requestPermissions", activity, permissions, 0);
            }
        }
        catch (System.Exception)
        {
            permissionsGranted = false;
        }
    }

    private void RequestPermissionsLegacy(AndroidJavaObject activity, string[] permissions)
    {
        try
        {
            foreach (string permission in permissions)
            {
                Permission.RequestUserPermission(permission);
            }
        }
        catch (System.Exception)
        {
            permissionsGranted = false;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Application.platform == RuntimePlatform.Android)
        {
            // Actualiza el cache de permisos cuando la app vuelve a tener foco
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                int sdkInt = version.GetStatic<int>("SDK_INT");
                
                if (sdkInt >= 33)
                {
                    string[] permissions = new string[] { "android.permission.READ_MEDIA_IMAGES" };
                    permissionsGranted = CheckAndroid13Permissions(activity, permissions);
                }
                else
                {
                    string[] permissions = new string[] {
                        Permission.ExternalStorageRead,
                        Permission.ExternalStorageWrite
                    };
                    permissionsGranted = CheckLegacyPermissions(permissions);
                }
            }
        }
    }

    public bool ArePermissionsGranted(bool forceCheck = false)
    {
        if (permissionsGranted.HasValue && !forceCheck)
        {
            return permissionsGranted.Value;
        }

        // Si no tenemos un valor cacheado o se fuerza la verificación
        if (Application.platform != RuntimePlatform.Android)
        {
            permissionsGranted = true;
            return true;
        }

        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            int sdkInt = version.GetStatic<int>("SDK_INT");
            
            if (sdkInt >= 33)
            {
                string[] permissions = new string[] { "android.permission.READ_MEDIA_IMAGES" };
                permissionsGranted = CheckAndroid13Permissions(activity, permissions);
            }
            else
            {
                string[] permissions = new string[] {
                    Permission.ExternalStorageRead,
                    Permission.ExternalStorageWrite
                };
                permissionsGranted = CheckLegacyPermissions(permissions);
            }
        }

        if (!permissionsGranted.Value)
        {
            CheckAndRequestPermissions();
        }

        return permissionsGranted ?? false;
    }
}

