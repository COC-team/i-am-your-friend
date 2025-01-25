using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public SpriteRenderer cursorSpriteRenderer;   // SpriteRenderer для кастомного курсора
    public Canvas canvas;                         // Ссылка на Canvas в World Space

    private RectTransform canvasRect; // RectTransform Canvas

    // Параметры смещения курсора (в пикселях)
    public Vector2 cursorOffset = new Vector2(20f, -20f); 

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

        // Если не назначен SpriteRenderer, выводим ошибку
        if (cursorSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer for the cursor is not assigned.");
        }
    }

    void Update()
    {
        if (canvas == null || cursorSpriteRenderer == null) return;

        // Получаем текущую позицию мыши
        Vector2 cursorPos = Input.mousePosition;

        // Переводим экранные координаты мыши в мировые координаты
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(cursorPos);

        // Добавляем смещение (для 2D)
        worldPos.x += cursorOffset.x / canvas.scaleFactor; // Учтём масштаб Canvas
        worldPos.y += cursorOffset.y / canvas.scaleFactor; // Учтём масштаб Canvas

        // Ограничиваем позицию курсора в пределах Canvas, учитывая смещение и размер курсора
        worldPos = ClampToCanvas(worldPos);

        // Устанавливаем позицию курсора с SpriteRenderer
        cursorSpriteRenderer.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);  // Z=0 для 2D
    }

    private Vector3 ClampToCanvas(Vector3 position)
    {
        // Получаем границы Canvas в мировых координатах
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        // Левая, правая, нижняя и верхняя границы
        float minX = corners[0].x + cursorOffset.x;
        float maxX = corners[2].x - cursorOffset.x;
        float minY = corners[0].y - cursorOffset.y;
        float maxY = corners[1].y + cursorOffset.y;

        // Ограничиваем позицию курсора с учетом смещения и размера
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector3(clampedX, clampedY, position.z);  // Возвращаем в мировых координатах
    }
}
