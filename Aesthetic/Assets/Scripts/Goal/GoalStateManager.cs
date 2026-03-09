using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoalStateManager : MonoBehaviour
{
    public enum States
    {
        Ciruclar,
        Copperhead,
        EmptyCircular,
        EmptySquare,
        Freycinet,
        GriftRoll,
        Hexavector,
        LeatherWood,
        Line,
        Oleo,
        Outline,
        Quoll,
        Quadrilateral,
        Triangular,
        WaterColor,
        Coal,
        Fresco,
        OldBeach,
        Tarraleah
    }

    private States currentState;
    private int numberOfStates;
    private const string PREFS_COLLECTION_KEY = "ActiveSplashCollection";


    // Custom palette system
    private List<int> validCustomStates;
    private int customPaletteIndex = 0;
    private const string CUSTOM_ART_1 = "CustomArt1";
    private const string CUSTOM_ART_2 = "CustomArt2";
    private const string CUSTOM_ART_3 = "CustomArt3";
    private const string CUSTOM_ART_4 = "CustomArt4";

    private void Awake()
    {
        numberOfStates = Enum.GetValues(typeof(States)).Length;
        SetCurrentState(PlayerPrefs.GetInt(PREFS_COLLECTION_KEY, 0));
    }

    public States GetCurrentState()
    {
        return currentState;
    }

    private void SetCurrentState(int index)
    {
        switch (index)
        {
            case -2:
                currentState = (States)Random.Range(0, numberOfStates);
                break;
            case -1:
                CustomArt();
                break;
            default:
                currentState = (States)index;
                break;
        }

    }

  private void CustomArt()
    {
        if (validCustomStates == null)
        {
            BuildValidCustomStatesList();
        }

        // If no valid states found, fallback to random
        if (validCustomStates.Count == 0)
        {
            Debug.LogWarning("No valid custom art states found. Using random state instead.");
            currentState = (States)Random.Range(0, numberOfStates);
            return;
        }

        // Get the current state from the cycling list
        int stateValue = validCustomStates[customPaletteIndex];
        currentState = (States)stateValue;

        // Move to next index, cycling back to 0 when we reach the end
        customPaletteIndex = (customPaletteIndex + 1) % validCustomStates.Count;
    }

    private void BuildValidCustomStatesList()
    {
        validCustomStates = new List<int>();
        
        // Check all custom art PlayerPrefs
        string[] customArtKeys = { CUSTOM_ART_1, CUSTOM_ART_2, CUSTOM_ART_3, CUSTOM_ART_4 };
        
        foreach (string key in customArtKeys)
        {
            int value = PlayerPrefs.GetInt(key, -1); // Default to -1 if not found
            
            // Only add values that are >= 0 and within valid state range
            if (value >= 0 && value < numberOfStates)
            {
                validCustomStates.Add(value);
            }
            else if (value >= 0)
            {
                Debug.LogWarning($"Custom art value {value} in {key} is out of range (max: {numberOfStates - 1})");
            }
        }

        Debug.Log($"Built valid custom states list with {validCustomStates.Count} entries");
    }

    // Public method to trigger the next custom art state (called from GoalBehavior)
    public void NextCustomArtState()
    {
        if (PlayerPrefs.GetInt(PREFS_COLLECTION_KEY, 0) == -1) // Only work in custom mode
        {
            CustomArt();
        }
    }
    public bool IsInCustomMode()
    {
        return PlayerPrefs.GetInt(PREFS_COLLECTION_KEY, 0) == -1;
    }

    
}

