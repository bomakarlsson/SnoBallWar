using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BallLauncher : MonoBehaviour
{
    Animator animator;
    public GameObject player;

    public Transform spawnPoint; 
    public Transform arrow;
    [SerializeField] float minLaunchForce = 750f;
    [SerializeField] float maxLaunchForce = 1500f;
    [SerializeField] float maxHoldTime = 1f; 

    private float holdTime = 0f; 
    private bool isHolding = false; 
    private Vector2 aimDirection = Vector2.up; 

    private void Start()
    {       
        arrow.gameObject.SetActive(false);
        animator = player.GetComponent<Animator>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (HasAvailableBalls())
            {
                isHolding = true;
                holdTime = 0f;

                arrow.gameObject.SetActive(true);

                // Start the scaling coroutine
                StartCoroutine(ScaleArrow());
            }
        }

        if (context.canceled)
        {
            isHolding = false;
            LaunchBall();

            arrow.gameObject.SetActive(false);
        }
    }


    // Coroutine to scale the arrow
    private IEnumerator ScaleArrow()
    {
        // Set the initial scale of the arrow to 10
        Vector3 initialScale = Vector3.one * 10f;
        Vector3 targetScale = Vector3.one * 20f;
        float scaleDuration = 1f;

        arrow.transform.localScale = initialScale;

        float elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / scaleDuration);

            // Smoothly interpolate the scale from initial to target
            arrow.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        arrow.transform.localScale = targetScale;
    }


    public void OnAim(InputAction.CallbackContext context)
    {        
        if (context.control.device.name == "Mouse")
        {
            Vector3 mousePosition = context.ReadValue<Vector2>();
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, 
                                                                                    mousePosition.y, 
                                                                                    Camera.main.transform.position.z * -1));
            aimDirection = (worldMousePosition - transform.position).normalized;
        }
        else
            aimDirection = context.ReadValue<Vector2>().normalized;
               
        if (aimDirection == Vector2.zero)
        {
            aimDirection = Vector2.up;
        }
        
        

       

        UpdateArrowRotation();
    }

    private void UpdateArrowRotation()
    {        
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90f;
                      
        arrow.rotation = Quaternion.Euler(0f, 0f, angle);
    }

   /* private void UpdateArrowScale()
    {
        // Calculate the hold time (which correlates with the launch force)
        float clampedHoldTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);

        // Calculate the launch force
        float launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, clampedHoldTime / maxHoldTime);

        // Map the launch force to scale between 10 and 15
        float scale = Mathf.Lerp(10f, 15f, (launchForce - minLaunchForce) / (maxLaunchForce - minLaunchForce));

        // Apply the new scale to the arrow
        arrow.localScale = new Vector3(scale, scale, 1f); // Assuming the arrow is pointing along the Z-axis
    } */

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;

            animator.SetBool("Aiming", true);
            print("AIMING");
        }
        else
        {
            animator.SetBool("Aiming", false);
        }
        
    }

    private void LaunchBall()
    {
        // Clamp the hold time to the maximum hold duration
        float clampedHoldTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
              
        float launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, clampedHoldTime / maxHoldTime);
                
        foreach (Transform child in spawnPoint)
        {
            if (!child.gameObject.activeInHierarchy)
            {                
                child.gameObject.SetActive(true);
                                
                child.parent = null;
                                
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                                        
                    rb.AddForce(new Vector3(aimDirection.x, aimDirection.y, 0f) * launchForce);
                    animator.SetTrigger("FireTrigger");
                }

                break;
            }
        }
    }

    private bool HasAvailableBalls()
    {
        // Check if there are any inactive balls in the spawnPoint
        foreach (Transform child in spawnPoint)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                return true; // Found at least one inactive ball
            }
        }
        return false; // No inactive balls available
    }
}








