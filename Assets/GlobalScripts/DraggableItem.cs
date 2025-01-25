using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera uiCamera;
    private RectTransform canvasRect;
    private Vector2 offset;

    public float maxDragSpeed = 20f;  // Увеличиваем отзывчивость в 10 раз
    public float minDragSpeed = 2f; // Минимальная скорость (оставим большую отзывчивость на последнем объекте)
    public int maxDestroyedCount = 8; // Количество уничтоженных объектов, после которых отзывчивость минимальна

    private float dragSpeed;

    // Метод для обновления скорости
    public void UpdateDragSpeed(int destroyedCount)
    {
        // Применяем более мягкое уменьшение скорости
        float t = Mathf.InverseLerp(0, maxDestroyedCount, destroyedCount);

        // Меньше замедление и плавное замедление, сохраняем высокую отзывчивость в конце
        dragSpeed = Mathf.Lerp(maxDragSpeed, minDragSpeed, Mathf.Pow(t, 0.2f)); // Чем меньше степень, тем менее выражено замедление
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas not found in parent.");
        }

        dragSpeed = maxDragSpeed; // Изначально отзывчивость максимально высокая
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
            Vector2 newPosition = (Vector2)worldPointerPos - offset;

            // Ограничиваем объект в пределах Canvas
            newPosition = ClampToCanvas(newPosition);

            // Применяем уменьшенную скорость
            newPosition = Vector2.Lerp(rectTransform.position, newPosition, dragSpeed * Time.deltaTime);

            rectTransform.position = newPosition;
        }
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        // Ограничиваем объект в пределах Canvas
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[1].y;

        Vector2 objectSize = new Vector2(
            rectTransform.rect.width * rectTransform.lossyScale.x,
            rectTransform.rect.height * rectTransform.lossyScale.y
        );

        float objectHalfWidth = objectSize.x / 2;
        float objectHalfHeight = objectSize.y / 2;

        float clampedX = Mathf.Clamp(position.x, minX + objectHalfWidth, maxX - objectHalfWidth);
        float clampedY = Mathf.Clamp(position.y, minY + objectHalfHeight, maxY - objectHalfHeight);

        return new Vector2(clampedX, clampedY);
    }
}
