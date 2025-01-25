using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public float RemainingTime; // Сколько времени осталось до уничтожения
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        RemainingTime = 2f; // Устанавливаем стартовое время уничтожения
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("DestroyableObject: Нет SpriteRenderer на объекте.");
        }
    }

    // Устанавливаем прозрачность объекта
    public void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}