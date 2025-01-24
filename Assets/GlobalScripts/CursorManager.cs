using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Image cursorImage;   // UI Image для курсора
    public float cursorScale = 1.5f;  // Масштаб курсора
    public Canvas canvas;  // Ссылка на Canvas, где находится курсор

    private RectTransform canvasRect; // RectTransform Canvas

    void Start()
    {
        // Отключаем стандартный курсор
        Cursor.visible = false;

        // Если canvas не задан, находим его в сцене с использованием FindFirstObjectByType
        if (canvas == null)
        {
            canvas = Object.FindFirstObjectByType<Canvas>(); // заменили на новый метод
        }

        // Получаем RectTransform от Canvas
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Получаем текущую позицию мыши на экране
        Vector2 cursorPos = Input.mousePosition;

        // Переводим экранные координаты мыши в локальные координаты Canvas
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, cursorPos, canvas.worldCamera, out localPos);

        // Ограничиваем позицию в пределах Canvas
        Vector2 clampedPos = ClampToCanvas(localPos);

        // Добавляем офсет (например, 10 пикселей вправо и вниз)
        Vector2 offset = new Vector2(65f, -65f); // Положительное значение по X — вправо, отрицательное по Y — вниз
        clampedPos += offset;

        // Перемещаем курсор (UI Image) на ограниченную позицию с учётом офсета
        cursorImage.rectTransform.localPosition = clampedPos;

        // Масштабируем курсор (опционально)
        cursorImage.rectTransform.localScale = new Vector3(cursorScale, cursorScale, 1f);
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        // Получаем размеры Canvas
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Ограничиваем позицию по X и Y в пределах Canvas
        float clampedX = Mathf.Clamp(position.x, -canvasSize.x / 2, canvasSize.x / 2);
        float clampedY = Mathf.Clamp(position.y, -canvasSize.y / 2, canvasSize.y / 2);

        return new Vector2(clampedX, clampedY);
    }
}