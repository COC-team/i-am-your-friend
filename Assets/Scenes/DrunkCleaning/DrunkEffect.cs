using UnityEngine; // Добавьте эту строку

public class DrunkEffect : MonoBehaviour
{
    public float drunkLevel = 0f;  // Уровень пьянства
    public float drunkIncreaseSpeed = 10000f; // Скорость увеличения пьянства с каждым уничтожением
    public float maxDrunkLevel = 10000000000f; // Максимальный уровень пьянства

    private Vector2 drunkOffset; // Смещение, которое применяется к движению шарика
    private float shakeTimer = 0f; // Таймер для плавности тряски

    // Метод для увеличения уровня пьянства
    public void IncreaseDrunkLevel()
    {
        drunkLevel = Mathf.Min(drunkLevel + drunkIncreaseSpeed, maxDrunkLevel);
    }

    // Метод для получения смещения в зависимости от уровня пьянства
    public Vector2 GetDrunkOffset()
    {
        // Чем выше уровень пьянства, тем сильнее будет смещение
        float offsetMagnitude = Mathf.Lerp(0f, 200f, drunkLevel / maxDrunkLevel); // Увеличиваем амплитуду

        // Плавная тряска
        shakeTimer += Time.deltaTime * Mathf.Lerp(1f, 5f, drunkLevel / maxDrunkLevel); // Увеличиваем скорость изменения
        drunkOffset = new Vector2(
            Mathf.Sin(shakeTimer) * offsetMagnitude, // Синус для плавного колебания
            Mathf.Cos(shakeTimer) * offsetMagnitude  // Косинус для плавного колебания
        );

        return drunkOffset;
    }

    // Сброс пьянства
    public void ResetDrunkLevel()
    {
        drunkLevel = 0f;
        shakeTimer = 0f; // Сбросим таймер для тряски
    }
}