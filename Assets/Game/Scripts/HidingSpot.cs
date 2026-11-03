using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetHidden(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetHidden(false);
        }
    }
}