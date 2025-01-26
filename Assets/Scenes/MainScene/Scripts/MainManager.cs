using UnityEngine;
using UnityEngine.SceneManagement; // Для работы с сценами

public class MainManager : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // Имя следующей сцены

    void Start()
    {
        // Завершаем активные мини-игры
        Debug.Log($"Завершаем активные мини-игры.");
        GameStateManager.Instance.CompleteActiveGames();

        // Проверяем статус мини-игр
        bool allGamesCompleted = true; // Флаг для проверки всех игр

        for (int i = 0; i < GameStateManager.Instance.miniGameCompleted.Length; i++)
        {

                if (GameStateManager.Instance.miniGameCompleted[i])
                {
                    Debug.Log($"");
                }
                else
                {
                    Debug.Log($"Мини-игра {i} была начата, но не завершена.");
                    allGamesCompleted = false; // Если хотя бы одна игра не завершена
                }
        }

        // Если все мини-игры пройдены, запускаем следующую сцену
        if (allGamesCompleted)
        {
            Debug.Log("Все мини-игры пройдены! Загружаем следующую сцену...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}