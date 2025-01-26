using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class EndCutscene : MonoBehaviour
{
    [SerializeField] private float cutsceneDuration = 9.5f; // Duration of the cutscene in seconds
    [SerializeField] private string nextSceneName; // Name of the next scene to load (set in Inspector)

    void Start()
    {
        // Start the coroutine to wait for the cutscene to end
        StartCoroutine(EndAfterDelay());
    }

    private System.Collections.IEnumerator EndAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(cutsceneDuration);

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