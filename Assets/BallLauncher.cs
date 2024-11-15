using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    public Transform spawnPoint; // Reference to the spawn point (where balls are located)
    public float minLaunchForce = 750f; // Minimum force
    public float maxLaunchForce = 1500f; // Maximum force
    public float maxHoldTime = 1f; // Maximum hold time for full force

    private float holdTime = 0f; // Tracks how long the button is held
    private bool isHolding = false; // Tracks whether the button is being held

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {            
            isHolding = true;
            holdTime = 0f;
        }

        if (context.canceled)
        {            
            isHolding = false;
            LaunchBall();
        }
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
        float clampedHoldTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
                
        float launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, clampedHoldTime / maxHoldTime);
               
        foreach (Transform child in spawnPoint)
        {
            if (!child.gameObject.activeInHierarchy)
            {                
                child.gameObject.SetActive(true);
                                
                child.parent = null;

                Rigidbody rb = child.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero; 
                rb.AddForce(Vector3.up * launchForce); 

                break; 
            }
        }
    }
}




