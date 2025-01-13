using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    private Vector2 direction = Vector2.right;
    public float groundDetectionDistance = 0.6f;
    public float forwardOffset = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;

        Vector2 raycastOrigin = (Vector2)transform.position + direction * forwardOffset;
        RaycastHit2D groundHit = Physics2D.Raycast(raycastOrigin, Vector2.down, groundDetectionDistance);

        if (groundHit.collider == null)
        {
            direction = -direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = -direction;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 raycastOrigin = (Vector2)transform.position + direction * forwardOffset;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.down * groundDetectionDistance);
    }
}