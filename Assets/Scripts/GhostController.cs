using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float speed = 5f; // Movement speed of the ghost
    public float verticalFollowSpeed = 2f; // Vertical movement speed of the ghost

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable gravity
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the horizontal direction towards the player
            Vector2 horizontalDirection = new Vector2(player.position.x - transform.position.x, 0).normalized;

            // Move the ghost horizontally towards the player
            rb.linearVelocity = new Vector2(horizontalDirection.x * speed, rb.linearVelocity.y);

            // Smoothly adjust the vertical position without directly setting velocity
            float newY = Mathf.Lerp(transform.position.y, player.position.y, Time.deltaTime * verticalFollowSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop movement when no player
        }
    }
}
