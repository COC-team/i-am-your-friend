using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVRemoteButton : MonoBehaviour
{
    [SerializeField] private TMP_Text channelDisplay;  // Displays the current channel
    [SerializeField] private TMP_Text inputDisplay;    // Displays the entered digits
    [SerializeField] private Animator animator;        // Handles animations
    [SerializeField] private Image displayImage;       // Image to display after each trigger
    [SerializeField] private AudioSource audioSource;  // Audio source for playing sounds
    [SerializeField] private AudioClip step0Sound;     // Sound for the initial step
    [SerializeField] private AudioClip step1Sound;     // Sound for step 1
    [SerializeField] private AudioClip step2Sound;     // Sound for step 2
    [SerializeField] private AudioClip step3Sound;     // Sound for step 3

    private static string channelInput = "";          // Stores the user's input
    public static int currentChannel = 0;             // The current channel
    private static float inputTimer = 0f;             // Timer for resetting input
    private static float maxInputDelay = 15f;         // Time allowed between inputs

    // Step triggers
    private bool step0Triggered = false;
    private bool step1Triggered = false;
    private bool step2Triggered = false;

    // Images for steps
    [SerializeField] private Sprite step0Image;        // Image for the initial step
    [SerializeField] private Sprite step1Image;        // Image for step 1
    [SerializeField] private Sprite step2Image;        // Image for step 2
    [SerializeField] private Sprite step3Image;        // Image for step 3

    void Update()
    {
        // Reset input if too much time passes
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

        // Add button value to input and reset timer
        channelInput += buttonValue.ToString();
        inputTimer = maxInputDelay;

        Debug.Log($"Updated Channel Input: {channelInput}");

        // If two digits are entered, update the channel
        if (channelInput.Length == 2)
        {
            currentChannel = Mathf.Clamp(int.Parse(channelInput), 0, 99); // Ensure channel is valid
            HandleChannelChange(currentChannel);
            channelInput = ""; // Reset input after updating channel
            UpdateChannelDisplay();
        }

        UpdateInputDisplay(); // Update the displayed input
    }

    private void HandleChannelChange(int channel)
    {
        Debug.Log($"Channel Updated to: {channel}");

        if (animator != null)
        {
            // Set the channel parameter for the animator
            animator.SetInteger("channel", channel);

            // Handle the sequence of steps

            TriggerStep(step0Image, step0Sound, "Dad says: Start with channel 45.");
            step0Triggered = true;
            else if (!step1Triggered && channel == 52)
            {
                TriggerStep(step1Image, step1Sound, "Dad says: Good, now switch to channel 90.");
                step1Triggered = true;
            }
            else if (step1Triggered && !step2Triggered && channel == 90)
            {
                TriggerStep(step2Image, step2Sound, "Dad says: Great, now switch to channel 16 for football.");
                step2Triggered = true;
            }
            else if (step1Triggered && step2Triggered && channel == 16)
            {
                TriggerStep(step3Image, step3Sound, "Football animation triggered!");
                animator.SetTrigger("FootballAnimationTrigger");
            }
        }
    }

    private void TriggerStep(Sprite stepImage, AudioClip stepSound, string logMessage)
    {
        // Update the image display
        if (displayImage != null && stepImage != null)
        {
            displayImage.sprite = stepImage;
            displayImage.enabled = true; // Ensure the image is visible
        }

        // Play the sound
        if (audioSource != null && stepSound != null)
        {
            audioSource.PlayOneShot(stepSound);
        }

        // Log the message
        Debug.Log(logMessage);
    }

    private void UpdateInputDisplay()
    {
        if (inputDisplay != null)
        {
            inputDisplay.text = !string.IsNullOrEmpty(channelInput) ? channelInput : "--"; // Default to '--' if empty
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
            channelDisplay.text = currentChannel.ToString("D2"); // Format as two digits
        }
        else
        {
            Debug.LogWarning("Channel Display is not assigned!");
        }
    }
}
