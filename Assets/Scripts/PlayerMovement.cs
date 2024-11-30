using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;


    private float horizontal;
    [SerializeField] float speed = 8f;
    [SerializeField] float jumpingPower = 16f;
    private bool isFacingRight = true;

    private Vector2 aimDirection = Vector2.zero;


    void Update()
    {
        UpdateFacingDirection();

        if (IsGrounded())
        {
            rb.AddForce(Vector2.down * 5f);
        }        
    }   

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        // The size of the box for the ground check
        Vector2 boxSize = new Vector2(0.8f, 0.4f); // Adjust this based on the player's dimensions

        // Perform the box cast (casting the box downward)
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, Vector2.down, 0f, groundLayer);

        // If the box cast hits something on the ground layer, the player is grounded
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        // Check if the player is grounded
        bool grounded = IsGrounded();

        // Set the Gizmos color based on whether the player is grounded
        if (grounded)
        {
            Gizmos.color = Color.green;  // Green when grounded
        }
        else
        {
            Gizmos.color = Color.red;    // Red when not grounded
        }

        // Draw the box in the scene view for debugging
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(0.8f, 0.4f, 1f)); // Visualize the box cast
    }



    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }


    public void Aim(InputAction.CallbackContext context)
    {
        if (context.control.device.name == "Mouse")
        {
            Vector3 mousePosition = context.ReadValue<Vector2>();
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,
                                                                                    mousePosition.y,
                                                                                    Camera.main.transform.position.z * -1));
            aimDirection = worldMousePosition - transform.position;
        }
        else
            aimDirection = context.ReadValue<Vector2>();
    }

    private void UpdateFacingDirection()
    {
        if (aimDirection.x > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (aimDirection.x < 0f && isFacingRight)
        {
            Flip();
        }
    }
}



