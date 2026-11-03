using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AgentPatrol : MonoBehaviour
{
    [SerializeField] private float leftX = 2f;
    [SerializeField] private float rightX = 6f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Transform visionArea;

    private Rigidbody2D body;
    private bool movingRight = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float targetX = movingRight ? rightX : leftX;

        float nextX = Mathf.MoveTowards(
            body.position.x,
            targetX,
            speed * Time.fixedDeltaTime);

        body.MovePosition(new Vector2(nextX, body.position.y));

        if (Mathf.Abs(nextX - targetX) < 0.01f)
        {
            movingRight = !movingRight;
        }

        if (visionArea != null)
        {
            float direction = movingRight ? 1.7f : -1.7f;
            visionArea.localPosition = new Vector3(
                direction, 0f, 0f);
        }
    }
}