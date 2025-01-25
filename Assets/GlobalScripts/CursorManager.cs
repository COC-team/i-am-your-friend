using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Image cursorImage;   // UI Image для кастомного курсора
    public Canvas canvas;       // Ссылка на Canvas в World Space

    private RectTransform canvasRect; // RectTransform Canvas

    void Start()
    {
        // Отключаем стандартный курсор
        Cursor.visible = false;

        // Если Canvas не указан, находим его
        if (canvas == null)
        {
            canvas = Object.FindFirstObjectByType<Canvas>(); // Автоматический поиск Canvas
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        // Получаем RectTransform от Canvas
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (canvas == null) return;

        // Получаем текущую позицию мыши
        Vector2 cursorPos = Input.mousePosition;

        // Переводим экранные координаты мыши в мировые координаты
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRect, cursorPos, canvas.worldCamera, out Vector3 worldPos);

        // Ограничиваем позицию курсора в пределах Canvas
        Vector2 clampedPos = ClampToCanvas(worldPos);

        // Устанавливаем позицию кастомного курсора
        cursorImage.rectTransform.position = clampedPos;
    }

    private Vector2 ClampToCanvas(Vector3 position)
    {
        // Получаем границы Canvas в мировых координатах
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        // Левая, правая, нижняя и верхняя границы
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[1].y;

        // Ограничиваем позицию курсора
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }
}
