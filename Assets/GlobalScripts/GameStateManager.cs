using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public Vector3 playerPosition;
    public bool[] miniGameStarted = { false, false, false };
    public bool[] miniGameCompleted = { false, false, false };

    public AudioClip completionSound; // Звук завершения игры
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Добавляем AudioSource, если его нет
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
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
            if (!miniGameCompleted[miniGameIndex]) // Если игра еще не завершена
            {
                miniGameCompleted[miniGameIndex] = true;
                PlayCompletionSound(); // Воспроизводим звук завершения
                Debug.Log($"Мини-игра {miniGameIndex} завершена!");
            }
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
                PlayCompletionSound(); // Воспроизводим звук завершения
                Debug.Log($"Мини-игра {i} была активной и теперь отмечена как пройденная.");
            }
        }
    }

    private void PlayCompletionSound()
    {
        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound); // Проигрываем звук
        }
        else
        {
            Debug.LogWarning("AudioSource или AudioClip не назначены!");
        }
    }
}
