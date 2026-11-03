using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UpdateDetection(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        UpdateDetection(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetDetected(false);
        }
    }

    private void UpdateDetection(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetDetected(!gameManager.IsHidden);
        }
    }
}