using UnityEngine;

public class MainManager : MonoBehaviour
{
    void Start()
    {
        // Завершаем активные мини-игры
        Debug.Log($"Завершаем активные мини-игры.");
        GameStateManager.Instance.CompleteActiveGames();

        // Выводим информацию о статусе мини-игр
        for (int i = 0; i < GameStateManager.Instance.miniGameStarted.Length; i++)
        {
            if (GameStateManager.Instance.miniGameStarted[i])
            {
                if (GameStateManager.Instance.miniGameCompleted[i])
                {
                    Debug.Log($"Мини-игра {i} была пройдена.");
                }
                else
                {
                    Debug.Log($"Мини-игра {i} была начата, но не завершена.");
                }
            }
        }
    }
}
