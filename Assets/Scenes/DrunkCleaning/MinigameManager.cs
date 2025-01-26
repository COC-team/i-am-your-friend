using UnityEngine;
using UnityEngine.SceneManagement; // Для возможности перезагрузки сцены

public class GameManager : MonoBehaviour
{
    public float gameDuration = 30f;        // Время игры в секундах
    private float timer;                    // Таймер игры
    private bool isGameRunning;             // Статус игры

    public RectTransform maskRectTransform; // Маска для обрезки спрайта (RectTransform)
    public RectTransform spriteRectTransform; // Спрайт, который мы будем обрезать
    public SceneReloader sceneReloader;

    void Start()
    {
        // Игра не начата, таймер равен 0
        timer = 0f;
        isGameRunning = false;
        StartGame();

        // Убедитесь, что маска и спрайт привязаны в инспекторе
        if (maskRectTransform == null || spriteRectTransform == null)
        {
            Debug.LogError("Не привязаны маска или спрайт в инспекторе.");
        }
    }

    void Update()
    {

        // Если игра идет, отсчитываем таймер
        if (isGameRunning)
        {
            timer -= Time.deltaTime; // Уменьшаем таймер по мере времени

            // Если время истекло, заканчиваем игру
            if (timer <= 0)
            {
                EndGame();
            }

            // Обновляем позицию маски в зависимости от времени
            UpdateMaskPosition();
        }
    }

    void StartGame()
    {
        // Запускаем игру, устанавливаем таймер
        isGameRunning = true;
        timer = gameDuration;
        Debug.Log("Игра началась! Время: " + gameDuration + " секунд.");
    }

    void EndGame()
    {
        // Завершаем игру
        isGameRunning = false;
        Debug.Log("Игра окончена! Время истекло.");
        sceneReloader.ReloadScene();
    }
    
    public void WinGame()
    {
        // Завершаем игру
        isGameRunning = false;
        Debug.Log("Игра окончена! Вы победили.");
        SceneManager.LoadScene("MainScene");
    }

    void UpdateMaskPosition()
    {
        if (maskRectTransform != null)
        {
            // Рассчитываем процент прошедшего времени
            float timePercent = timer / gameDuration;

            // Получаем ширину экрана в мировых координатах
            float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;

            // Рассчитываем смещение маски влево
            float shiftAmount = (screenWidth / 3f) * (1 - timePercent) * 7f; // Умножаем на 10 для усиления эффекта

            // Обновляем позицию маски: смещаем её влево
            maskRectTransform.localPosition = new Vector3(-shiftAmount, maskRectTransform.localPosition.y, maskRectTransform.localPosition.z);
        }
    }
}
