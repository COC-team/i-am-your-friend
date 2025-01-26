using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform; // RectTransform объекта
    private Canvas canvas;               // Родительский Canvas
    private Camera uiCamera;             // Камера для World Space Canvas
    private RectTransform canvasRect;    // RectTransform Canvas
    private Vector2 offset;              // Смещение относительно точки нажатия

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();  // Находим родительский Canvas
        canvasRect = canvas.GetComponent<RectTransform>();

        // Если Canvas в режиме ScreenSpace - Camera или WorldSpace, указываем камеру
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas not found in parent.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRect, eventData.position, uiCamera, out Vector3 worldPointerPos))
        {
            offset = (Vector2)worldPointerPos - (Vector2)rectTransform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRect, eventData.position, uiCamera, out Vector3 worldPointerPos))
        {
            // Новая позиция с учетом смещения
            Vector2 newPosition = (Vector2) worldPointerPos - offset;

            // Ограничиваем объект в пределах Canvas
            newPosition = ClampToCanvas(newPosition);

            // Устанавливаем позицию объекта
            rectTransform.position = newPosition;
        }
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        // Получаем границы Canvas в мировых координатах
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        // Левая, правая, нижняя и верхняя границы Canvas
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[1].y;

        // Получаем фактические размеры объекта с учетом масштаба
        Vector2 objectSize = new Vector2(
            rectTransform.rect.width * rectTransform.lossyScale.x,
            rectTransform.rect.height * rectTransform.lossyScale.y
        );

        // Вычисляем границы объекта с учетом его размеров
        float objectHalfWidth = objectSize.x / 2;
        float objectHalfHeight = objectSize.y / 2;

        // Ограничиваем позицию объекта так, чтобы он не выходил за границы Canvas
        float clampedX = Mathf.Clamp(position.x, minX + objectHalfWidth, maxX - objectHalfWidth);
        float clampedY = Mathf.Clamp(position.y, minY + objectHalfHeight, maxY - objectHalfHeight);

        return new Vector2(clampedX, clampedY);
    }

}
