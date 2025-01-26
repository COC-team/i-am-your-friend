using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlePlayButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("hi");
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
