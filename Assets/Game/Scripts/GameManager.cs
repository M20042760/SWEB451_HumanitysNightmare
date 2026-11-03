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
    private bool recharging;
    private bool hidden;
    private bool gameOver;

    public bool IsHidden => hidden;

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
        detected = hidden ? false : value;
        RefreshUI();
    }

    public void SetRecharging(bool value)
    {
        recharging = value;
        RefreshUI();
    }

    public void SetHidden(bool value)
    {
        hidden = value;

        if (hidden)
        {
            detected = false;
        }

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
        else if (hidden)
        {
            statusText.text = "HIDDEN - Agent cannot detect you.";
        }
        else if (recharging)
        {
            statusText.text = "RECHARGING...";
        }
        else
        {
            statusText.text = "Objective: Reach the offline AI.";
        }
    }
}