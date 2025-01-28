using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Scenes.KillRoaches.Scripts;

public class BubbleInteraction : MonoBehaviour
{
    public GameObject bubble;
    public GameObject acid;
    public Animator cockroachAnimator;
    
    private SpriteRenderer bubbleRenderer;
    private Texture2D bubbleTexture;
    private Color originalColor;

    private bool _isBubbleAcid = false;
    private bool _isSuckingAcid = false;

    private float _rChange;
    private float _gChange;
    private float _bChange;

    private GameObject[] cockroachesInBubble = new GameObject[8];

    public int acidDelay = 2;
    public int killDelay = 1;
    private CancellationTokenSource acidCts = new CancellationTokenSource();
    private CancellationTokenSource killCts = new CancellationTokenSource();

    private int killedAmount = 0;
    private MinigameManager minigameManager;
    
    private float currentRChange;
    private float currentGChange;
    private float currentBChange;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        minigameManager = FindObjectOfType<MinigameManager>();

        // Убедитесь, что MinigameManager найден
        if (minigameManager == null)
        {
            Debug.LogError("MinigameManager не найден! Убедитесь, что он есть в сцене.");
        }
        Array.Fill(cockroachesInBubble, null);
        bubble = GameObject.Find("Bubble");
        acid = GameObject.Find("Acid");
        bubbleRenderer = bubble.GetComponent<SpriteRenderer>();
        originalColor = bubbleRenderer.color;
        Debug.Log($"Bubble renderer: {bubbleRenderer.color}");
        
        _rChange = (Color.magenta.r - bubbleRenderer.color.r) / 2;
        _gChange = (Color.magenta.g - bubbleRenderer.color.g) / 2;
        _bChange = (Color.magenta.b - bubbleRenderer.color.b) / 2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GameObject().name == "Acid")
        {
            paintBubbleTimeout();
            
        } else if (other.gameObject.CompareTag("Cockroaches") && _isBubbleAcid)
        {
            int id = other.gameObject.GetInstanceID();
            int freeIndex = -1;
            for (int i = 0; i < cockroachesInBubble.Length; i++)
            {
                if (cockroachesInBubble[i] == null)
                {
                    freeIndex = i;
                } else if (cockroachesInBubble[i].GetInstanceID() == id)
                {
                    return;
                }
            }
            cockroachesInBubble[freeIndex] = other.gameObject;
            killCockroachTimeout(id);
        }
    }

    async void paintBubbleTimeout()
    {
        acidCts = new CancellationTokenSource();
        Task timeoutTask = Task.Delay(acidDelay * 1000, acidCts.Token);
        try
        {
            _isSuckingAcid = true;
            float startTime = Time.time;
            while (Time.time - startTime < acidDelay)
            {
                float elapsed = Time.time - startTime;
                float lerpFactor = elapsed / acidDelay;
            
                currentRChange = Mathf.Lerp(0, _rChange, lerpFactor);
                currentGChange = Mathf.Lerp(0, _gChange, lerpFactor);
                currentBChange = Mathf.Lerp(0, _bChange, lerpFactor);

                bubbleRenderer.color = new Color(bubbleRenderer.color.r + Time.deltaTime * currentRChange,
                    bubbleRenderer.color.g + Time.deltaTime * currentGChange,
                    bubbleRenderer.color.b + Time.deltaTime * currentBChange);

                await Task.Yield();
            }

            MakeAcid();
            _isSuckingAcid = false;
        }
        catch (OperationCanceledException)
        {
            _isSuckingAcid = false;
            ReturnToOriginalColor();
        }
    }


    private void FixedUpdate()
    {
        if (_isSuckingAcid)
        {
            bubbleRenderer.color = new Color(bubbleRenderer.color.r + Time.deltaTime * currentRChange,
                bubbleRenderer.color.g + Time.deltaTime * currentGChange,
                bubbleRenderer.color.b + Time.deltaTime * currentBChange);
        }
    }


    async void killCockroachTimeout(int id)
    {
        killCts = new CancellationTokenSource();
        Task timeoutTask = Task.Delay(killDelay * 1000, killCts.Token);
        try
        {
            await Task.WhenAll(new Task[] { timeoutTask });
            for (int i = 0; i < cockroachesInBubble.Length; i++)
            {
                if (cockroachesInBubble[i] != null && cockroachesInBubble[i].GetInstanceID() == id)
                {
                    killCockroach(cockroachesInBubble[i]);
                    cockroachesInBubble[i] = null;
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            
        }
    }

    private void killCockroach(GameObject cockroach)
    {
        Animator animator = cockroach.GetComponent<Animator>();
        animator.SetTrigger("death");
        for (int i = 0; i < cockroachesInBubble.Length; i++)
        {
            if (cockroachesInBubble[i] != null && cockroachesInBubble[i].GetInstanceID() == cockroach.GetInstanceID())
            {
                cockroachesInBubble[i] = null;
                killedAmount++;

                if (killedAmount == 7)
                {
                    minigameManager.WinGame();
                }
                break;
            }
        }
        ReturnToOriginalColor();
    }
    
    private void MakeAcid()
    {
        // Apply the new color to the sprite
        bubbleRenderer.color = Color.magenta;
        _isBubbleAcid = true;
    }

    private void ReturnToOriginalColor()
    {
        bubbleRenderer.color = originalColor;
        _isBubbleAcid = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GameObject().name == "Acid")
        {
            acidCts.Cancel();
        } else if (other.gameObject.CompareTag("Cockroaches"))
        {
            int id = other.gameObject.GetInstanceID();
            for (int i = 0; i < cockroachesInBubble.Length; i++)
            {
                if (cockroachesInBubble[i] != null && cockroachesInBubble[i].GetInstanceID() == id)
                {
                    cockroachesInBubble[i] = null;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
