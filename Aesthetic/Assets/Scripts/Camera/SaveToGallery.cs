using System.IO;
using TMPro;
using UnityEngine;
using System;

public class SaveToGallery : MonoBehaviour
{
    public Camera mainCamera; // Cámara principal del juego
    public Camera captureCamera; // Cámara específica para capturas
    private RenderTexture captureTexture;
    [SerializeField] private TextMeshProUGUI savedText;
    [SerializeField] private AudioSource cameraSound;
    [SerializeField] private GameObject flashEffect;
    [SerializeField] private GameObject savedEffect;
    [SerializeField] private GameObject photoButton;
    private bool isProcessing = false;

    private void Start()
    {
        if (PermissionManager.Instance == null)
        {
            Debug.LogError("No se encontró PermissionManager en la escena");
        }

        if (captureCamera == null)
        {
            Debug.LogError("No se ha asignado la cámara de captura");
            return;
        }
    }

    public void CaptureAndSaveImage()
    {
        captureCamera.backgroundColor =  mainCamera.backgroundColor;

        if (isProcessing)
        {
            savedText.text = "Ya se está procesando una imagen...";
            return;
        }

        if (captureCamera == null)
        {
            savedText.text = "Error: Cámara de captura no asignada";
            return;
        }

        try
        {
            isProcessing = true;
            SaveImage();
        }
        catch (UnauthorizedAccessException)
        {
            savedText.text = "Error: No hay permisos para guardar la imagen";
            Debug.LogError("Error de permisos al guardar la imagen");
        }
        catch (IOException ex)
        {
            savedText.text = "Error: No se pudo guardar la imagen (problema de almacenamiento)";
            Debug.LogError($"Error de IO al guardar: {ex.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error detallado al capturar imagen: {e.Message}\nStack Trace: {e.StackTrace}");
            savedText.text = "Error al guardar la imagen: " + e.Message;
        }
        finally
        {
            CleanupResources();
            isProcessing = false;
        }
    }

    private void SaveImage()
    {
        savedText.text = "Saving...";

        flashEffect.SetActive(true);
        cameraSound.Play();
        
        try
        {
            // Crear RenderTexture con la resolución de la pantalla
            int width = Screen.width;
            int height = Screen.height;

            if (captureTexture != null)
            {
                captureTexture.Release();
                Destroy(captureTexture);
            }

            // Crear nuevo RenderTexture con alta calidad
            captureTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            captureTexture.antiAliasing = 8;
            captureTexture.filterMode = FilterMode.Trilinear;
            captureTexture.Create();

            // Asignar el RenderTexture a la cámara de captura
            captureCamera.targetTexture = captureTexture;

            // Render la escena
            captureCamera.Render();

            // Crear textura para guardar la captura
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGBA32, false);
            RenderTexture.active = captureTexture;
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply();

            // Convertir a PNG
            byte[] imageBytes = screenshot.EncodeToPNG();
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new Exception("Error al codificar la imagen");
            }

            string fileName = "Capture_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

            if (Application.platform == RuntimePlatform.Android)
            {
                SaveToAndroidGallery(imageBytes, fileName);
            }
            else
            {
                SaveToLocalStorage(imageBytes, fileName);
            }

            Destroy(screenshot);
        }
        catch (Exception)
        {
            savedText.text = "Failed to saved the canvas";
            throw;
        }
        finally
        {
            // Limpiar el RenderTexture de la cámara
            if (captureCamera != null)
            {
                captureCamera.targetTexture = null;
            }
            RenderTexture.active = null;
        }
    }

    private void SaveToAndroidGallery(byte[] imageBytes, string fileName)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            throw new ArgumentException("Datos de imagen inválidos");
        }

        savedText.text = "Saving...";
        
        try
        {
            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var context = activity.Call<AndroidJavaObject>("getApplicationContext"))
            using (var contentValues = new AndroidJavaObject("android.content.ContentValues"))
            {
                contentValues.Call("put", "mime_type", "image/png");
                contentValues.Call("put", "_display_name", fileName);
                contentValues.Call("put", "relative_path", "DCIM/MyGame");

                using (var contentResolver = context.Call<AndroidJavaObject>("getContentResolver"))
                using (var imageCollection = new AndroidJavaClass("android.provider.MediaStore$Images$Media")
                    .GetStatic<AndroidJavaObject>("EXTERNAL_CONTENT_URI"))
                {
                    var uri = contentResolver.Call<AndroidJavaObject>("insert", imageCollection, contentValues);
                    if (uri == null)
                    {
                        throw new Exception("No se pudo crear la entrada en la galería");
                    }

                    using (uri)
                    {
                        var outputStream = contentResolver.Call<AndroidJavaObject>("openOutputStream", uri);
                        if (outputStream == null)
                        {
                            contentResolver.Call("delete", uri, null, null);
                            throw new Exception("No se pudo abrir el stream para escribir");
                        }

                        using (outputStream)
                        {
                            outputStream.Call("write", imageBytes);
                            outputStream.Call("flush");
                        }
                    }
                }

                PhotoSaved();

            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al guardar en galería: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw;
        }
    }

    private void SaveToLocalStorage(byte[] imageBytes, string fileName)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            throw new ArgumentException("Datos de imagen inválidos");
        }

        string folderPath = Path.Combine(Application.persistentDataPath, "MyGame");
        try
        {
            Directory.CreateDirectory(folderPath);
        }
        catch (Exception)
        {
            throw new IOException("No se pudo crear el directorio para guardar");
        }

        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllBytes(filePath, imageBytes);

        PhotoSaved();
    }

    private void PhotoSaved()
    {
        savedText.text = "Canvas saved in your Photos!";
        savedEffect.SetActive(true);
        photoButton.SetActive(false);
    }
    private void CleanupResources()
    {
        if (captureTexture != null)
        {
            captureTexture.Release();
            Destroy(captureTexture);
            captureTexture = null;
        }

        if (captureCamera != null)
        {
            captureCamera.targetTexture = null;
        }
        RenderTexture.active = null;
    }

    private void OnDisable()
    {
        CleanupResources();
    }
}