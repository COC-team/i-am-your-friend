using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private float delay = 5f;  // Time (in seconds) before the scene changes
    [SerializeField] private string sceneName = "NextScene";  // Name of the scene to load

    private float timer;  // Internal timer

    void Start()
    {
        timer = delay;  // Initialize the timer with the delay
    }

    void Update()
    {
        // Reduce the timer by the elapsed time since the last frame
        timer -= Time.deltaTime;

        // Check if the timer has reached 0
        if (timer <= 0f)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        // Log a message (optional) for debugging
        Debug.Log($"Changing to scene: {sceneName}");
        
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}