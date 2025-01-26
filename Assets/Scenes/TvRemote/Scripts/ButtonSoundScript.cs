using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundScript : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip buttonSound;  // Reference to the sound effect

    void Start()
    {
        // Add a listener to the button to trigger the sound when clicked
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    public void PlaySound()
    {
        // Play the sound
        if (audioSource != null && buttonSound != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }
    }
}