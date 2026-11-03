using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryDrainPerSecond = 1.5f;
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private TMP_Text statusText;

    private float battery;
    private bool detected;
    private bool gameOver;

    private void Awake()
    {
        battery = maxBattery;
        RefreshUI();
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        battery = Mathf.Max(
            0f,
            battery - batteryDrainPerSecond * Time.deltaTime);

        if (battery <= 0f)
        {
            gameOver = true;
        }

        RefreshUI();
    }

    public void SetDetected(bool value)
    {
        detected = value;
        RefreshUI();
    }

    public void Recharge(float amount)
    {
        battery = Mathf.Min(maxBattery, battery + amount);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (batteryText != null)
        {
            batteryText.text = $"BATTERY: {battery:0}%";
        }

        if (statusText == null)
        {
            return;
        }

        if (gameOver)
        {
            statusText.text = "SYSTEM SHUTDOWN - Battery depleted";
        }
        else if (detected)
        {
            statusText.text = "DETECTED! Leave the red vision area.";
        }
        else
        {
            statusText.text = "Objective: Reach the offline AI.";
        }
    }
}