using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlePlayButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("hi");
        SceneManager.LoadScene("IntroCutscene");
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
