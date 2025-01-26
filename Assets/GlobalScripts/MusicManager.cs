using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgroundMusic;  // Аудиофайл музыки
    private AudioSource audioSource;   // Компонент AudioSource

    void Start()
    {
        // Получаем компонент AudioSource, если он еще не был получен
        audioSource = GetComponent<AudioSource>();

        // Проверяем, если у нас есть музыка, то воспроизводим ее
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;  // Устанавливаем музыкальный клип
            audioSource.loop = true;              // Включаем зацикливание музыки
            audioSource.Play();                   // Запускаем воспроизведение музыки
        }
    }

    void Update()
    {
        // Проверяем, если музыка закончилась, и проигрываем ее снова
        if (!audioSource.isPlaying)
        {
            audioSource.Play();  // Если музыка не играет, начинаем воспроизведение заново
        }
    }
}