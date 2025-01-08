using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 2f;
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
    }
}
