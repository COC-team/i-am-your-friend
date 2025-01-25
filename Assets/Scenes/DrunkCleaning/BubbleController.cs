using UnityEngine;

public class BallController : MonoBehaviour
{
    public float destroyTime = 1f;
    public int destroyedCount = 0;

    private DestroyableObject targetDestroyable;
    private DraggableItem draggableItem;
    private SpriteRenderer spriteRenderer; // Компонент SpriteRenderer для изменения свечения
    private Material defaultMaterial; // Исходный материал объекта

    // Ссылка на MinigameManager для вызова метода WinGame
    public GameManager minigameManager;

    void Start()
    {
        draggableItem = GetComponent<DraggableItem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Не найден SpriteRenderer у объекта с BallController.");
        }
        else
        {
            defaultMaterial = spriteRenderer.material;
        }

        UpdateGlowEffect();
    }

    void Update()
    {
        if (targetDestroyable != null)
        {
            targetDestroyable.RemainingTime -= Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, (destroyTime - targetDestroyable.RemainingTime) / destroyTime);
            targetDestroyable.SetAlpha(alpha);

            if (targetDestroyable.RemainingTime <= 0)
            {
                Destroy(targetDestroyable.gameObject);
                destroyedCount++;

                // Обновляем отзывчивость в зависимости от количества уничтоженных объектов
                draggableItem.UpdateDragSpeed(destroyedCount);

                // Если уничтожено 9 объектов, вызываем WinGame в MinigameManager
                if (destroyedCount == 9 && minigameManager != null)
                {
                    minigameManager.WinGame();
                }

                targetDestroyable = null;
                UpdateGlowEffect(); // Обновляем эффект свечения
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable"))
        {
            targetDestroyable = collision.GetComponent<DestroyableObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetDestroyable?.gameObject)
        {
            targetDestroyable = null;
        }
    }

    private void UpdateGlowEffect()
    {
        if (spriteRenderer != null)
        {
            // Рассчитываем интенсивность свечения (максимум 100%)
            float glowIntensity = Mathf.Min(0.1f * destroyedCount, 1f);

            // Устанавливаем зелёный цвет с нужной интенсивностью
            spriteRenderer.material.SetColor("_Color", new Color(1f - glowIntensity, 1f, 1f - glowIntensity, 1f));
        }
    }
}
