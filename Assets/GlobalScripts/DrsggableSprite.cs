using UnityEngine;

public class DraggableSprite : MonoBehaviour
{
    private Vector3 offset; // Смещение между позицией мыши и центром объекта
    private Camera mainCamera; // Основная камера для определения мировых координат
    private bool isDragging = false;

    void Start()
    {
        // Находим основную камеру
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        // Определяем смещение между позицией мыши и центром объекта
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        offset = transform.position - mouseWorldPosition;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // Перемещаем объект к позиции мыши с учетом смещения
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            transform.position = mouseWorldPosition + offset;
        }
    }

    void OnMouseUp()
    {
        // Перетаскивание прекращается, когда отпускается кнопка мыши
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Получаем текущую позицию мыши в мировых координатах
        Vector3 mouseScreenPosition = Input.mousePosition; // Позиция мыши в экранных координатах
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z); // Глубина объекта
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition); // Преобразуем в мировые координаты
    }
}