using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    public Transform spawnPoint; // Reference to the spawn point (where balls are located)
    public float launchForce = 500f; // Force to apply to launch the ball

    public void OnFire(InputAction.CallbackContext context)
    {
        // Check if the button/trigger was released
        if (context.canceled)
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        // Find the first inactive ball under the spawn point
        foreach (Transform child in spawnPoint)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                // Activate the ball and apply an upward force
                child.gameObject.SetActive(true);

                // Remove the ball from being a child of the spawner
                child.parent = null;

                // Ensure the ball has a Rigidbody component to apply force
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // Reset velocity
                    rb.AddForce(Vector3.up * launchForce); // Apply upward force
                }

                break; // Exit the loop after launching one ball
            }
        }
    }
}


