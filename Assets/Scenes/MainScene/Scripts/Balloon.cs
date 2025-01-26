using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float moveSpeed = 300f;         // Скорость полета к игроку
    public float swingSpeed = 2f;          // Скорость качания вверх и вниз
    public float swingAmount = 2;          // Амплитуда качания
    public Transform player;               // Ссылка на игрока (с тегом "Player")
    
    public float swingDistance = 3f;       // Максимальное расстояние, на котором шарик может качаться
    public float startMoveThreshold = 150f;  // Порог расстояния, при котором шарик начнёт двигаться к игроку
    public float playerRadius = 100f;        // Радиус игрока (например, радиус коллайдера игрока)

    private bool isSwinging = false;       // Флаг, который указывает, качается ли шарик
    private Vector3 swingPosition;         // Позиция, на которой будет качаться шарик

    private float stopThreshold = 0.1f;    // Порог расстояния для перехода в режим качания

    void Start()
    {
        // Находим игрока по тегу, если не передан в инспектор
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Запоминаем начальную позицию качания
        swingPosition = transform.position;
    }

    void Update()
    {
        // Вычисляем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > startMoveThreshold)
        {
            // Если шарик слишком далеко, он начнёт двигаться к игроку
            if (isSwinging)
            {
                // Если шарик в режиме качания, выходим из него
                isSwinging = false;
            }

            // Летит к игроку
            MoveToPlayer();
        }
        else if (distanceToPlayer <= swingDistance)
        {
            // Если шарик достаточно близко, он начинает качаться
            if (!isSwinging)
            {
                // Включаем качание, когда шарик достаточно близко
                isSwinging = true;
                swingPosition = transform.position;  // Сохраняем текущую позицию как начальную для качания
            }

            // Качается, но остается на месте
            SwingAndStayInPlace();
        }
    }

    void MoveToPlayer()
    {
        // Вычисляем случайную позицию на радиусе вокруг игрока
        Vector3 targetPosition = player.position + (Random.insideUnitSphere * playerRadius);
        
        // Убедитесь, что цель находится на плоскости с игроком (если нужно)
        targetPosition.y = player.position.y;

        // Летит к цели
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void SwingAndStayInPlace()
    {
        // Вычисляем колебания с помощью синуса
        float swingCycle = Mathf.Sin(Time.time * swingSpeed); // Синус от времени создает колебания от -1 до 1

        // Увеличиваем амплитуду качания по Y
        float yOffset = swingCycle * swingAmount;  // Умножаем на нужную амплитуду

        // Обновляем вертикальную позицию, не меняем X и Z (оставляем X и Z как у игрока)
        Vector3 newPosition = new Vector3(player.position.x, player.position.y + yOffset, player.position.z);

        // Обновляем позицию шарика
        transform.position = newPosition;
    }
}
