using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SplashCollection
{
    public string name;
    public Sprite[] sprites;
}

public class PreviewSplash : MonoBehaviour
{
    [SerializeField] private Image mainPreview;
    [SerializeField] private List<SplashCollection> splashCollections = new List<SplashCollection>();
    [SerializeField] private List<SplashCollection> randomCollections = new List<SplashCollection>();
    [SerializeField] private List<SplashCollection> customCollections = new List<SplashCollection>();
    
    [Header("Preview Settings")]
    [SerializeField] private TextMeshProUGUI collectionNameText; // Optional: to display current collection name
    
    private int currentCollectionIndex = 0;
    private int currentSpriteIndex = 0;
    
    private const string PREFS_COLLECTION_KEY = "ActiveSplashCollection";
    private const string PREFS_SPRITE_KEY = "ActiveSplashSprite";
    private bool inCustom = false;

    void Start()
    {
        LoadSavedCollection();
        UpdatePreview();
    }
    
    // Load the saved collection and sprite indices from PlayerPrefs
    private void LoadSavedCollection()
    {
        if (splashCollections.Count == 0) return;
        
        currentCollectionIndex = PlayerPrefs.GetInt(PREFS_COLLECTION_KEY, 0);
        // Ensure the index is within bounds
        currentCollectionIndex = Mathf.Clamp(currentCollectionIndex, 0, splashCollections.Count - 1);
        
        // Load the saved sprite index for this collection
        currentSpriteIndex = PlayerPrefs.GetInt(PREFS_SPRITE_KEY, 0);
        
        // Ensure the sprite index is within bounds for the current collection
        if (splashCollections[currentCollectionIndex].sprites.Length > 0)
        {
            currentSpriteIndex = Mathf.Clamp(currentSpriteIndex, 0, 
                splashCollections[currentCollectionIndex].sprites.Length - 1);
        }
    }
    
    // Update the preview image and optional text
    private void UpdatePreview()
    {
        if (splashCollections.Count == 0) return;
        
        SplashCollection activeCollection = splashCollections[currentCollectionIndex];
        
        // Update collection name if text component is assigned
        if (collectionNameText != null)
        {
            collectionNameText.text = activeCollection.name;
        }
        
        // Update the preview image
        if (activeCollection.sprites.Length > 0)
        {
            mainPreview.sprite = activeCollection.sprites[currentSpriteIndex];
        }
        else
        {
            Debug.LogWarning($"Collection '{activeCollection.name}' has no sprites!");
        }
    }
    
    // Save the current selection to PlayerPrefs
    private void SaveCurrentSelection()
    {
        PlayerPrefs.SetInt(PREFS_COLLECTION_KEY, currentCollectionIndex);
        PlayerPrefs.SetInt(PREFS_SPRITE_KEY, currentSpriteIndex);
        PlayerPrefs.Save();
    }
    
    // Public methods for UI buttons
    
    // Navigate to the next sprite in the current collection
    public void NextSprite()
    {
        if (splashCollections.Count == 0 || inCustom) return;
        
        SplashCollection activeCollection = splashCollections[currentCollectionIndex];
        if (activeCollection.sprites.Length == 0) return;
        
        currentSpriteIndex = (currentSpriteIndex + 1) % activeCollection.sprites.Length;
        UpdatePreview();
        SaveCurrentSelection();
        PlaySoundEffect();
    }
    
    // Navigate to the previous sprite in the current collection
    public void PreviousSprite()
    {
        if (splashCollections.Count == 0 || inCustom) return;
        
        SplashCollection activeCollection = splashCollections[currentCollectionIndex];
        if (activeCollection.sprites.Length == 0) return;
        
        currentSpriteIndex = (currentSpriteIndex - 1 + activeCollection.sprites.Length) % activeCollection.sprites.Length;
        UpdatePreview();
        SaveCurrentSelection();
        PlaySoundEffect();
    }

    // Set the active collection by index
    public void SetActiveCollection(int collectionIndex)
    {
        if (collectionIndex < 0 || collectionIndex >= splashCollections.Count)
        {
            Debug.LogError($"Collection index {collectionIndex} is out of range!");
            return;
        }

        currentCollectionIndex = collectionIndex;
        currentSpriteIndex = 0; // Reset to first sprite in the new collection
        UpdatePreview();
        SaveCurrentSelection();

        inCustom = false;
    }

    public void SetRandomSprite()
    {
        currentCollectionIndex = 0;
        currentSpriteIndex = 0;

        SplashCollection activeCollection = randomCollections[currentCollectionIndex];
        mainPreview.sprite = activeCollection.sprites[currentSpriteIndex];
        
        inCustom = true;
    }

    public void SetCustomSprite()
    {
        currentCollectionIndex = 0;
        currentSpriteIndex = 0;

        SplashCollection activeCollection = customCollections[currentCollectionIndex];
        mainPreview.sprite = activeCollection.sprites[currentSpriteIndex];
        
        inCustom = true;
    }
    // Set the active collection by name
    public void SetActiveCollection(string collectionName)
    {
        for (int i = 0; i < splashCollections.Count; i++)
        {
            if (splashCollections[i].name.Equals(collectionName, StringComparison.OrdinalIgnoreCase))
            {
                SetActiveCollection(i);
                return;
            }
        }

        Debug.LogWarning($"Collection '{collectionName}' not found!");
    }

    private void PlaySoundEffect()
    {
        float pitch = UnityEngine.Random.Range(0.98f,1.1f);
        UISoundManager.Instance.PlaySoundEffect(6,pitch,UISoundManager.MEDIUM_VOLUME);
    }

    public int GetCurerntCollectionIndex()
    {
        return PlayerPrefs.GetInt(PREFS_COLLECTION_KEY, 0);
    }
    
    /*
    public Sprite[] GetCurrentCollectionSprites()
    {
        if (splashCollections.Count == 0) return new Sprite[0];
        return splashCollections[currentCollectionIndex].sprites;
    }
    
    public Sprite GetCurrentSprite()
    {
        if (splashCollections.Count == 0 || 
            splashCollections[currentCollectionIndex].sprites.Length == 0) 
            return null;
            
        return splashCollections[currentCollectionIndex].sprites[currentSpriteIndex];
    }
    
    public string GetCurrentCollectionName()
    {
        if (splashCollections.Count == 0) return string.Empty;
        return splashCollections[currentCollectionIndex].name;
    }
    */
}