using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float jump;
    Transform self;
    Rigidbody2D rb;

    void Start()
    {
        self = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var horizontalMagnitude = Input.GetAxis("Horizontal");

        rb.MovePosition(new Vector2(self.position.x, self.position.y) + (new Vector2(horizontalMagnitude, 0) * maxSpeed));
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jump));
        }
    }
}
