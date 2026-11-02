using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;

    private Rigidbody2D body;
    private Vector2 moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            moveInput.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            moveInput.y -= 1f;
        }

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput.x -= 1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput.x += 1f;
        }

        moveInput = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        bool isRunning = Keyboard.current != null &&
                         Keyboard.current.leftShiftKey.isPressed;

        float activeSpeed = isRunning ? runSpeed : walkSpeed;
        body.linearVelocity = moveInput * activeSpeed;
    }
}