using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public Vector3 playerPosition;
    public bool[] miniGameStarted = new bool[3];
    public bool[] miniGameCompleted = new bool[3];

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
        }
    }

    // Методы для обновления статуса мини-игр
    public void StartMiniGame(int miniGameIndex)
    {
        if (miniGameIndex >= 0 && miniGameIndex < miniGameStarted.Length)
        {
            miniGameStarted[miniGameIndex] = true;
        }
    }

    public void CompleteMiniGame(int miniGameIndex)
    {
        if (miniGameIndex >= 0 && miniGameIndex < miniGameCompleted.Length)
        {
            miniGameCompleted[miniGameIndex] = true;
        }
    }
    
    public void CompleteActiveGames()
    {
        for (int i = 0; i < miniGameStarted.Length; i++)
        {
            if (miniGameStarted[i] && !miniGameCompleted[i])
            {
                miniGameCompleted[i] = true;
                miniGameStarted[i] = false;
                Debug.Log($"Мини-игра {i} была активной и теперь отмечена как пройденная.");
            }
        }
    }
}