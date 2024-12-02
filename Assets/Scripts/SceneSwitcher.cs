using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{   
    [SerializeField]
    private string targetSceneName;   
    public void SwitchScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target scene name is not set in the Inspector!");
        }
    }
}

