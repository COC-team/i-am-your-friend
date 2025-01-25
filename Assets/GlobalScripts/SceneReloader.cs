using UnityEngine;
using UnityEngine.SceneManagement;  // Подключаем для работы с сценами

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        // Получаем текущую сцену и перезагружаем ее
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}