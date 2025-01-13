using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float vertical;
    private float ladderCenterX;
    private bool isLadder;
    private bool isClimbing;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //this works. need to handle breaking away from ladder center when jumping
        if (Input.GetButtonDown("Jump") && (isGrounded || isClimbing))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * maxSpeed, rb.linearVelocity.y);

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * maxSpeed);
            rb.transform.position = new Vector2(ladderCenterX, rb.transform.position.y);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder") || collision.CompareTag("Rope"))
        {
            isLadder = true;
            ladderCenterX = collision.gameObject.transform.position.x;
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
}
