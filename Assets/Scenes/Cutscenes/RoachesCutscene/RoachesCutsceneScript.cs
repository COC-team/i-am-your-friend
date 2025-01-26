using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class RoachesCutsceneScript : MonoBehaviour
{
    [SerializeField] private float cutsceneDuration = 15.5f; // Duration of the cutscene in seconds
    [SerializeField] private string nextSceneName; // The name of the scene to load after the cutscene

    void Start()
    {
        // Automatically end the cutscene after the specified duration
        Invoke("EndCutscene", cutsceneDuration);
    }

    private void EndCutscene()
    {
        // Check if a scene name is provided
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No scene name provided! Add the next scene name in the Inspector.");
        }
    }
}