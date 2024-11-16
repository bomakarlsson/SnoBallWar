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
    private Vector2 aimDirection = Vector2.up; // Default launch direction is "up"

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

    public void OnAim(InputAction.CallbackContext context)
    {       
        aimDirection = context.ReadValue<Vector2>().normalized;

        // Default to "up" if the stick is in a neutral position
        if (aimDirection == Vector2.zero)
        {
            aimDirection = Vector2.up;
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
        // Clamp the hold time to the maximum hold duration
        float clampedHoldTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);

        // Calculate the force based on the hold time
        float launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, clampedHoldTime / maxHoldTime);

        // Find the first inactive ball under the spawn point
        foreach (Transform child in spawnPoint)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                // Activate the ball and apply the calculated force
                child.gameObject.SetActive(true);

                // Remove the ball from being a child of the spawner
                child.parent = null;

                // Ensure the ball has a Rigidbody component to apply force
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;

                    // Apply force in the aim direction
                    rb.AddForce(new Vector3(aimDirection.x, aimDirection.y, 0f) * launchForce);
                }

                break;
            }
        }
    }
}





