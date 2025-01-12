using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    public float detectionDistance = 0.5f;
    private Vector2 direction = Vector2.right;
    private bool isTurning = false;
    public float turnCooldown = 0.5f; // Cooldown before allowing another turn

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Move the snake
        rb.linearVelocity = direction * speed;

        // If already turning, don't check for new turns yet
        if (isTurning) return;

        // Check for an obstacle in front
        RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, direction, detectionDistance);
        if (obstacleHit.collider != null)
        {
            StartCoroutine(TurnAround());
            return;
        }

        // Check if there's ground beneath the snake
        Vector2 downDirection = Vector2.down;
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position + (Vector3)direction * 0.5f, downDirection, detectionDistance);
        if (groundHit.collider == null)
        {
            StartCoroutine(TurnAround());
        }
    }

    private System.Collections.IEnumerator TurnAround()
    {
        isTurning = true;
        direction = -direction; // Reverse direction
        yield return new WaitForSeconds(turnCooldown); // Wait before allowing another turn
        isTurning = false;
    }

    void OnDrawGizmos()
    {
        // Visualize the raycasts in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * detectionDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + (Vector3)direction * 0.5f, transform.position + (Vector3)direction * 0.5f + Vector3.down * detectionDistance);
    }
}
