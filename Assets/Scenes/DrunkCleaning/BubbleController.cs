using UnityEngine;

public class BallController : MonoBehaviour
{
    public float destroyTime = 1f; // Время удержания для уничтожения объекта
    private GameObject targetObject; // Объект, над которым находится шарик
    private float timeOverTarget; // Таймер нахождения на объекте
    public int destroyedCount = 0; // Счетчик уничтоженных объектов

    private Vector3 offset; // Смещение курсора относительно шарика
    private bool isDragging = false; // Флаг перетаскивания

    private DestroyableObject targetDestroyable; // Ссылка на скрипт уничтожаемого объекта

    void Update()
    {
        HandleDrag();

        if (targetDestroyable != null)
        {
            // Увеличиваем время удержания
            targetDestroyable.RemainingTime -= Time.deltaTime;

            // Рассчитываем новую прозрачность
            float alpha = Mathf.Lerp(1f, 0f, (destroyTime - targetDestroyable.RemainingTime) / destroyTime);
            targetDestroyable.SetAlpha(alpha);

            // Если время истекло, уничтожаем объект
            if (targetDestroyable.RemainingTime <= 0)
            {
                Destroy(targetDestroyable.gameObject); // Уничтожаем объект
                destroyedCount++; // Увеличиваем счетчик
                Debug.Log("Уничтожено объектов: " + destroyedCount);

                targetDestroyable = null; // Сбрасываем цель
            }
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ нажата
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Если попали по этому объекту
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mousePos;
                offset.z = 0; // Игнорируем смещение по Z
            }
        }

        if (Input.GetMouseButtonUp(0)) // ЛКМ отпущена
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Для 2D игры игнорируем Z
            transform.position = mousePos + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если шарик находится над объектом с компонентом DestroyableObject
        if (collision.CompareTag("Destroyable"))
        {
            targetDestroyable = collision.GetComponent<DestroyableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если шарик уходит с объекта, сбрасываем цель
        if (collision.gameObject == targetDestroyable?.gameObject)
        {
            targetDestroyable = null;
        }
    }
}
