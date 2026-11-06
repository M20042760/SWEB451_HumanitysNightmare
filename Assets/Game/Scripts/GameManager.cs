using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Battery")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryDrainPerSecond = 1.5f;

    [Header("Capture")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float captureDelay = 3f;
    [SerializeField] private float captureMessageDuration = 1.5f;

    [Header("HUD")]
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private TMP_Text statusText;

    private float battery;
    private float detectionTimer;
    private float captureMessageTimer;

    private bool detected;
    private bool recharging;
    private bool hidden;
    private bool rewiring;
    private bool gameOver;
    private bool betaCompleted;

    private string rewiringMessage;

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
            if (Keyboard.current != null &&
                Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().name);
            }

            return;
        }

        if (betaCompleted)
        {
            return;
        }

        battery = Mathf.Max(
            0f,
            battery - batteryDrainPerSecond * Time.deltaTime);

        if (battery <= 0f)
        {
            gameOver = true;
            RefreshUI();
            return;
        }

        if (detected)
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= captureDelay)
            {
                CapturePlayer();
                return;
            }
        }
        else
        {
            detectionTimer = 0f;
        }

        if (captureMessageTimer > 0f)
        {
            captureMessageTimer -= Time.deltaTime;
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

    public void SetRewiringStatus(string message, bool active)
    {
        rewiringMessage = message;
        rewiring = active;
        RefreshUI();
    }

    public void CompleteBeta()
    {
        betaCompleted = true;
        RefreshUI();
    }

    public void Recharge(float amount)
    {
        battery = Mathf.Min(maxBattery, battery + amount);
        RefreshUI();
    }

    private void CapturePlayer()
    {
        detected = false;
        hidden = false;
        recharging = false;
        rewiring = false;
        detectionTimer = 0f;
        captureMessageTimer = captureMessageDuration;

        if (player != null && spawnPoint != null)
        {
            Rigidbody2D playerBody =
                player.GetComponent<Rigidbody2D>();

            if (playerBody != null)
            {
                playerBody.linearVelocity = Vector2.zero;
                playerBody.angularVelocity = 0f;
            }

            player.position = spawnPoint.position;
        }

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
            statusText.text =
                "SYSTEM SHUTDOWN - Press R to restart.";
        }
        else if (betaCompleted)
        {
            statusText.text =
                "BETA COMPLETE - AI reprogrammed!";
        }
        else if (captureMessageTimer > 0f)
        {
            statusText.text =
                "CAPTURED - Returned to checkpoint.";
        }
        else if (detected)
        {
            float remaining =
                Mathf.Max(0f, captureDelay - detectionTimer);

            statusText.text =
                $"DETECTED! Escape or hide: {remaining:0.0}s";
        }
        else if (hidden)
        {
            statusText.text =
                "HIDDEN - Agent cannot detect you.";
        }
        else if (recharging)
        {
            statusText.text = "RECHARGING...";
        }
        else if (rewiring)
        {
            statusText.text = rewiringMessage;
        }
        else
        {
            statusText.text =
                "Objective: Reach the offline AI.";
        }
    }
}