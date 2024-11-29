using UnityEngine;
using System.Collections.Generic;

public class CenteringCamera : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer; // Assign the "Player" layer in the Inspector
    [SerializeField] private float followSpeed = 2f; // Speed at which the camera follows
    [SerializeField] private float heightOffset = -10f; // Fixed Z-offset for the camera

    [Header("Zoom Settings")]
    [SerializeField] private float minFOV = 30f; // Minimum Field of View (zoomed in)
    [SerializeField] private float maxFOV = 60f; // Maximum Field of View (zoomed out)
    [SerializeField] private float maxDistance = 20f; // Maximum distance for FOV adjustment
    [SerializeField] private float zoomSpeed = 2f; // Speed of FOV adjustment

    private List<Transform> playerTransforms = new List<Transform>();

    private void Update()
    {
        UpdatePlayerList();
        UpdateCameraPosition();
        UpdateCameraZoom();
    }

    private void UpdatePlayerList()
    {
        playerTransforms.Clear();

        // Find all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.activeInHierarchy && obj.layer == LayerMask.NameToLayer("Player"))
            {
                playerTransforms.Add(obj.transform);
            }
        }

        if (playerTransforms.Count == 0)
        {
            Debug.LogWarning("No players found! The camera will remain static.");
        }
    }

    private void UpdateCameraPosition()
    {
        if (playerTransforms.Count == 0) return;

        // Calculate the average X and Y positions of all players
        Vector3 averagePosition = Vector3.zero;
        foreach (Transform player in playerTransforms)
        {
            averagePosition += player.position;
        }
        averagePosition /= playerTransforms.Count;

        // Desired camera position (fixed Z-offset)
        Vector3 desiredPosition = new Vector3(averagePosition.x, averagePosition.y, heightOffset);

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    private void UpdateCameraZoom()
    {
        if (playerTransforms.Count == 0) return;

        // Find the maximum distance between any two players
        float maxDistanceBetweenPlayers = 0f;
        for (int i = 0; i < playerTransforms.Count; i++)
        {
            for (int j = i + 1; j < playerTransforms.Count; j++)
            {
                float distance = Vector3.Distance(playerTransforms[i].position, playerTransforms[j].position);
                if (distance > maxDistanceBetweenPlayers)
                {
                    maxDistanceBetweenPlayers = distance;
                }
            }
        }

        // Calculate the desired FOV based on the maximum distance
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, maxDistanceBetweenPlayers / maxDistance);

        // Smoothly adjust the camera's FOV
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}







