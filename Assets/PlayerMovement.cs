using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb; 
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;

    void Update()
    {
        // Flip player direction based on movement
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {        
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, 0f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {           
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0f);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {            
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
        }
    }

    private bool IsGrounded()
    {        
        return Physics.OverlapSphere(groundCheck.position, 0.2f, groundLayer).Length > 0;
    }

    private void Flip()
    {        
        isFacingRight = !isFacingRight;
        
        transform.Rotate(0f, 180f, 0f);
    }

    public void Move(InputAction.CallbackContext context)
    {        
        horizontal = context.ReadValue<Vector2>().x;
    }
}
