using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    private int maxBalls = 5;
    [SerializeField] float holdTimeThreshold = 0.5f; // Time required to hold before spawning a ball
    private float holdTime = 0f;
    private bool isHoldingButton = false;
    private int lastBallCount = 0;

    [SerializeField] private GameObject[] ballUIImages; // Array to link UI image objects in inspector

    public void OnMakeBall(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHoldingButton = true; // Start tracking button hold
        }

        if (context.canceled)
        {
            isHoldingButton = false; // Stop tracking button hold
            holdTime = 0f; // Reset hold time
        }
    }

    private void Update()
    {
        if (isHoldingButton)
        {
            holdTime += Time.deltaTime;

            // Check if hold time exceeds the threshold and there is room for more balls
            if (holdTime >= holdTimeThreshold && transform.childCount < maxBalls)
            {
                SpawnBall();
                holdTime = 0f; // Reset hold time to allow for continuous spawning
            }
        }

        // Monitor and log ball count changes
        if (transform.childCount != lastBallCount)
        {
            lastBallCount = transform.childCount;
            Debug.Log($"Current ball count: {lastBallCount}");
            UpdateBallUI(); // Update the UI when ball count changes
        }
    }

    private void SpawnBall()
    {
        GameObject spawnedBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedBall.transform.parent = transform;

        spawnedBall.tag = this.tag;

        spawnedBall.SetActive(false); // Ball is initially inactive until used

        Debug.Log($"Current ball count: {transform.childCount}");
    }

    private void UpdateBallUI()
    {
        // Ensure no errors when ballUIImages is not properly assigned
        if (ballUIImages == null || ballUIImages.Length == 0) return;

        // Activate or deactivate UI Images based on current ball count
        for (int i = 0; i < ballUIImages.Length; i++)
        {
            if (ballUIImages[i] != null)
            {
                // Activate the UI image if the index is less than the current ball count
                ballUIImages[i].SetActive(i < transform.childCount);
            }
        }
    }
}













