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
}


