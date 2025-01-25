using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVRemoteButton : MonoBehaviour
{
    // Значение, которое задается кнопке (0-9)
    // [SerializeField] private int buttonValue;

    // Ссылка на текстовый объект, который отображает текущий канал
    [SerializeField] private TMP_Text channelDisplay;

    // Ссылка на текстовый объект, который отображает вводимые цифры
    [SerializeField] private TMP_Text inputDisplay;

    // Временная переменная для хранения ввода
    private static string channelInput = "";

    // Статическая переменная для хранения текущего канала (0-99)
    public static int currentChannel = 0;

    // Reference to the Animator for handling animations
    [SerializeField] private Animator animator; // Drag the Animator component here in Inspector

    // Таймер для ввода (в секундах)
    private static float inputTimer = 0f;
    
    // Увеличим максимальное время между нажатиями (например, 3 секунды)
    private static float maxInputDelay = 15f; // Increased to 3 seconds

    void Start()
    {
        // Убедимся, что значение кнопки находится в диапазоне 0-9
        // buttonValue = Mathf.Clamp(buttonValue, 0, 9);

        // Проверим, активен ли компонент Button
        // Button button = GetComponent<Button>();
        // if (button != null)
        // {
        //     button.onClick.AddListener(OnButtonPressed);
        // }
        // else
        // {
        //     Debug.LogError("Кнопка не найдена на объекте " + gameObject.name);
        // }
        // GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        // foreach (var button in buttons)
        // {
        //     var realBtn = button.GetComponent<Button>();
        //     button.onClick.AddListener(OnButtonPressed);
        // }
    }

    void Update()
    {
        // Сбрасываем ввод, если прошло слишком много времени
        if (inputTimer > 0f)
        {
            inputTimer -= Time.deltaTime;
            if (inputTimer <= 0f)
            {
                channelInput = "";
                UpdateInputDisplay();
            }
        }
    }

    public void OnButtonPressed(int buttonValue)
    {
        Debug.Log($"Button Pressed: {buttonValue}");

        // Добавляем значение кнопки к временной строке
        channelInput += buttonValue.ToString();
        inputTimer = maxInputDelay;  // Reset the timer to limit input delay

        Debug.Log($"Updated Channel Input: {channelInput}");

        // Если ввели одну или две цифры, обновляем текущий канал
        if (channelInput.Length == 2)
        {
            currentChannel = Mathf.Clamp(int.Parse(channelInput), 0, 99);  // Ensure the channel stays within 0-99

            if (animator != null)
            {
                // Update the animator with the current channel number
                animator.SetInteger("channel", currentChannel);
                
                // If the channel is 16, trigger the Football Animation
                if (currentChannel == 16)
                {
                    animator.SetTrigger("FootballAnimationTrigger");
                    animator.SetInteger("channel", currentChannel);
                    Debug.Log("Football animation triggered!");
                }
            }
                
            Debug.Log($"Channel Updated to: {currentChannel}");

            channelInput = "";  // Сбрасываем ввод после обновления канала
            UpdateChannelDisplay();  // Обновляем дисплей канала
        }

        // Обновляем текстовые поля
        UpdateInputDisplay();
    }

    private void UpdateInputDisplay()
    {
        if (inputDisplay != null)
        {
            inputDisplay.text = !string.IsNullOrEmpty(channelInput) ? channelInput : "--";  // Default to '--' if empty
        }
        else
        {
            Debug.LogWarning("Input Display is not assigned!");
        }
    }

    private void UpdateChannelDisplay()
    {
        if (channelDisplay != null)
        {
            channelDisplay.text = currentChannel.ToString("D2");  // Format as two digits (e.g., 01, 12)
        }
        else
        {
            Debug.LogWarning("Channel Display is not assigned!");
        }
    }
}
