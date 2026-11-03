using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class OfflineAI : MonoBehaviour
{
    [SerializeField] private float rewiringDuration = 3f;
    [SerializeField] private Color completedColor = Color.green;

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private bool playerInRange;
    private bool completed;
    private float rewiringProgress;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (completed || !playerInRange || gameManager == null)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.eKey.isPressed)
        {
            rewiringProgress += Time.deltaTime;

            float percent = Mathf.Clamp01(
                rewiringProgress / rewiringDuration) * 100f;

            gameManager.SetRewiringStatus(
                $"REWIRING... {percent:0}% - Hold E",
                true);

            if (rewiringProgress >= rewiringDuration)
            {
                CompleteRewiring();
            }
        }
        else
        {
            rewiringProgress = 0f;

            gameManager.SetRewiringStatus(
                "Hold E to reprogram the AI.",
                true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            rewiringProgress = 0f;

            if (gameManager != null)
            {
                gameManager.SetRewiringStatus("", false);
            }
        }
    }

    private void CompleteRewiring()
    {
        completed = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = completedColor;
        }

        GetComponent<Collider2D>().enabled = false;

        gameManager.SetRewiringStatus("", false);
        gameManager.CompleteBeta();
    }
}