using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
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
            }
        }

        if (context.canceled)
        {
            isHolding = false;
            LaunchBall();
                        
            arrow.gameObject.SetActive(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {        
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

    private void UpdateArrowScale()
    {
        // Calculate the hold time (which correlates with the launch force)
        float clampedHoldTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);

        // Calculate the launch force
        float launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, clampedHoldTime / maxHoldTime);

        // Map the launch force to scale between 10 and 15
        float scale = Mathf.Lerp(10f, 15f, (launchForce - minLaunchForce) / (maxLaunchForce - minLaunchForce));

        // Apply the new scale to the arrow
        arrow.localScale = new Vector3(scale, scale, 1f); // Assuming the arrow is pointing along the Z-axis
    }

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
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








