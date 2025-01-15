using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public float regrabDistance = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isLadder;
    private bool isClimbing;
    private float vertical;
    private float ladderCenterX;
    private float ladderTopY;
    private bool canRegrabLadder = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || isClimbing)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (isClimbing)
                {
                    isClimbing = false;
                    canRegrabLadder = false;
                }
            }
        }

        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f && canRegrabLadder && Mathf.Abs(transform.position.x - ladderCenterX) <= regrabDistance)
        {
            if (vertical > 0f && transform.position.y >= ladderTopY)
            {
                vertical = 0f;
            }
            else
            {
                isClimbing = true;
            }
        }

        if (rb.linearVelocity.y < 0f)
        {
            canRegrabLadder = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, vertical * maxSpeed);

            rb.position = new Vector2(ladderCenterX, rb.position.y);
        }
        else
        {
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(horizontalInput * maxSpeed, rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("Rope"))
        {
            isLadder = true;
            ladderCenterX = collision.gameObject.transform.position.x;

            Collider2D ladderCollider = collision.GetComponent<Collider2D>();
            ladderTopY = ladderCollider.bounds.max.y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("Rope"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
