using UnityEngine;

public class RechargeStation : MonoBehaviour
{
    [SerializeField] private float rechargePerSecond = 25f;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || gameManager == null)
        {
            return;
        }

        gameManager.SetRecharging(true);
        gameManager.Recharge(rechargePerSecond * Time.deltaTime);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetRecharging(false);
        }
    }
}