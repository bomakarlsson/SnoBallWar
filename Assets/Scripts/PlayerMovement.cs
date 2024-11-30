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

    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask obstacleLayer; // Define which layers count as obstacles
    [SerializeField] private float detectionDistance = 0.5f; // Raycast distance to detect obstacles
    [SerializeField] private float stepUpForce = 5f; // Force applied to step over obstacles
    [SerializeField] private float stepUpHeight = 0.3f; // Height above ground to check for obstacles

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
        if (CanMove)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            // Check and handle small obstacles
            DetectAndStepOverObstacle();
        }
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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

    private void DetectAndStepOverObstacle()
    {
        // Only perform the raycast if the player is moving
        if (Mathf.Abs(horizontal) > 0.01f) // Check if there is significant horizontal input
        {
            // Determine raycast origin and direction based on facing direction
            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + stepUpHeight);
            Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;

            // Perform raycast to detect obstacles
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionDistance, obstacleLayer);

            if (hit.collider != null)
            {
                Debug.Log($"Obstacle detected: {hit.collider.name}");

                // Apply upward force to step over the obstacle
                rb.velocity = new Vector2(rb.velocity.x, stepUpForce);
            }

            // Debug visualization in Scene view
            Debug.DrawRay(rayOrigin, rayDirection * detectionDistance, Color.red);
        }
    }

}


