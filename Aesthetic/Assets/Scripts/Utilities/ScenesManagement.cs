using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
    public string sceneName;
    public void LoadSceneName()
    {
        SceneManager.LoadScene(sceneName);
    }
}
