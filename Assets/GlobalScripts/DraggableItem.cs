using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera uiCamera;
    private RectTransform canvasRect; // RectTransform Canvas
    private Vector2 offset;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();  // Получаем родительский Canvas
        canvasRect = canvas.GetComponent<RectTransform>(); // RectTransform Canvas

        // Если камера не указана, находим её автоматически
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
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

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, uiCamera, out Vector2 localPointerPos))
        {
            offset = localPointerPos - (Vector2)rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, uiCamera, out Vector2 localPointerPos))
        {
            // Новая позиция с учётом смещения
            Vector2 newPosition = localPointerPos - offset;

            // Ограничиваем объект в пределах Canvas
            newPosition = ClampToCanvas(newPosition);

            // Применяем ограниченную позицию
            rectTransform.localPosition = newPosition;

        }
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        // Получаем размеры Canvas
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Получаем фактические размеры объекта
        Vector2 objectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);


        // Вычисляем границы с учётом размеров объекта
        float minX = -canvasSize.x / 2 + objectSize.x / 2; // Левая граница
        float maxX = canvasSize.x / 2 - objectSize.x / 2;  // Правая граница
        float minY = -canvasSize.y / 2 + objectSize.y / 2; // Нижняя граница
        float maxY = canvasSize.y / 2 - objectSize.y / 2;  // Верхняя граница


        // Ограничиваем позицию объекта в пределах Canvas
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }

}
