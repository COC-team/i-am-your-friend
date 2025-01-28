using UnityEngine;

public class FinalCutsceneScript : MonoBehaviour
{
    [SerializeField] private float cutsceneDuration = 40.5f; // Duration before quitting the game

    void Start()
    {
        // Schedule the game to quit after the cutscene duration
        Invoke("QuitGame", cutsceneDuration);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

        // If running in the Unity Editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}