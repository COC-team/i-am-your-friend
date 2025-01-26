using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTrigger : MonoBehaviour
{
    public TextMeshPro interactText;
    public Transform player;
    public float triggerRadius = 30f;
    public Vector2 textOffset = new Vector2(0, -1f);
    public string loadSceneName;
    public int miniGameIndex;
    public GameObject exclamationMark; // UI-иконка восклицательного знака

    public Camera mainCamera;

    private bool isGameCompleted;

    void Start()
    {
        if (loadSceneName == null)
        {
            throw new Exception("Specify scene name");
        }

        if (miniGameIndex >= 0 && miniGameIndex < GameStateManager.Instance.miniGameCompleted.Length)
        {
            // Проверяем статус мини-игры
            isGameCompleted = GameStateManager.Instance.miniGameCompleted[miniGameIndex];
            exclamationMark.SetActive(!isGameCompleted); // Отображаем восклицательный знак только если игра не пройдена
        }
        else
        {
            Debug.LogError($"Некорректный индекс мини-игры: {miniGameIndex}");
        }

        // Скрыть текст взаимодействия при старте
        interactText.transform.position = new Vector3(-10, -10, -10);
    }

    public int numSegments = 100;  // Number of segments for the circle (higher = smoother)

    private void OnDrawGizmos()
    {
        // Set Gizmos color
        Gizmos.color = Color.red;

        // Draw the circle
        DrawCircleAroundPoint(transform.position, triggerRadius, numSegments);
    }

    private void DrawCircleAroundPoint(Vector3 center, float radius, int segments)
    {
        float angleStep = Mathf.Deg2Rad * (360f / segments); // Angle between each segment
        Vector3 previousPoint = center + new Vector3(radius, 0, 0); // Starting point (radius away from center on the X axis)

        for (int i = 1; i <= segments; i++)
        {
            Vector2 vectorVA = previousPoint - center;

            // Поворот вектора на угол
            Vector2 rotatedVector = new Vector2(
                vectorVA.x * Mathf.Cos(angleStep) - vectorVA.y * Mathf.Sin(angleStep),
                vectorVA.x * Mathf.Sin(angleStep) + vectorVA.y * Mathf.Cos(angleStep)
            );

            // Координаты второго угла при основании
            Vector2 newPoint = new Vector2(center.x + rotatedVector.x, center.y + rotatedVector.y);
            // Draw a line between the previous point and the new point
            Gizmos.DrawLine(previousPoint, newPoint);

            previousPoint = newPoint;  // Set the previous point for the next line
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameCompleted)
        {
            // Если игра пройдена, не показываем текст и не обрабатываем нажатие
            interactText.transform.position = new Vector3(-10, -10, -10);
            return;
        }

        var distance = Vector2.Distance(new Vector2(player.position.x, player.position.y),
            new Vector2(transform.position.x, transform.position.y));

        if (distance <= triggerRadius)
        {
            ShowText();
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameStateManager.Instance.playerPosition = GameObject.FindWithTag("Player").transform.position;
                GameStateManager.Instance.StartMiniGame(miniGameIndex);
                SceneManager.LoadScene(loadSceneName);
            }
        }
        else
        {
            // Скрываем текст, если игрок вышел за пределы радиуса
            interactText.transform.position = new Vector3(-10, -10, -10);
        }
    }

    void ShowText()
    {
        Vector2 bottomBorder = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y - mainCamera.orthographicSize);
        interactText.transform.position = new Vector2(bottomBorder.x + textOffset.x, bottomBorder.y + textOffset.y);
    }
}
