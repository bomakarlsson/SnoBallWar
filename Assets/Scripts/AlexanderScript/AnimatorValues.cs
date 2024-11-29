using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorValues : MonoBehaviour
{
    Animator animator; // Reference to the Animator component
    Rigidbody2D rb;      // Reference to Rigidbody for velocity (optional)
    // Start is called before the first frame update

    public Transform groundCheck;
    public LayerMask groundLayer;


    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();

        // (Optional) Get the Rigidbody component if using physics-based movement
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Link player's speed to the "Speed" parameter
        float speed = rb.velocity.magnitude; // Calculate the player's speed (if using Rigidbody)
        

        // Update the Animator parameter
        animator.SetFloat("Speed", speed);

        // Example for boolean: Is the player grounded?
    

        // Update VerticalVelocity (e.g., to check jump/fall state)
        animator.SetFloat("VerticalVelocity", rb.velocity.y);

        animator.SetBool("IsGrounded", IsGrounded());
        

        
        
        
}

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


}
