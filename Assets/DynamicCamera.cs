using UnityEngine;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f; // How quickly the camera adjusts zoom
    [SerializeField] private float minFOV = 30f; // Minimum field of view
    [SerializeField] private float maxFOV = 60f; // Maximum field of view
    [SerializeField] private float extraPadding = 5f; // Extra padding for the zoom
    [SerializeField] private float refreshRate = 0.5f; // How often to refresh tracked objects
    [SerializeField] private LayerMask playerLayer; // Layer mask for "Player" objects

    private List<Transform> trackedObjects = new List<Transform>();
    private float cameraZOffset = -10f;

    private void Start()
    {
        // Refresh the list of tracked objects periodically
        InvokeRepeating(nameof(UpdateTrackedObjects), 0f, refreshRate);
    }

    private void Update()
    {
        if (trackedObjects.Count > 0)
        {
            // Calculate the midpoint of all tracked objects
            Vector3 midpoint = CalculateMidpoint();
            Debug.Log($"Midpoint calculated: {midpoint}");

            transform.position = new Vector3(midpoint.x, midpoint.y, cameraZOffset);

            // Calculate the maximum distance between tracked objects
            float maxDistance = CalculateMaxDistance(midpoint);
            Debug.Log($"Maximum distance from midpoint: {maxDistance}");

            // Adjust the field of view based on the maximum distance
            float desiredFOV = Mathf.Clamp(maxDistance + extraPadding, minFOV, maxFOV);
            Debug.Log($"Desired FOV: {desiredFOV}");

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            Debug.LogWarning("No tracked objects found! The camera cannot update.");
        }
    }

    private void UpdateTrackedObjects()
    {
        trackedObjects.Clear();

        // Find all objects in the "Player" layer
        Collider[] playerColliders = Physics.OverlapSphere(Vector3.zero, Mathf.Infinity, playerLayer);

        foreach (Collider col in playerColliders)
        {
            trackedObjects.Add(col.transform);
            Debug.Log($"Added {col.name} to tracked objects (Layer: Player).");
        }

        Debug.Log($"Total tracked objects: {trackedObjects.Count}");
    }

    private Vector3 CalculateMidpoint()
    {
        if (trackedObjects.Count == 0) return Vector3.zero;

        Vector3 totalPosition = Vector3.zero;
        foreach (Transform obj in trackedObjects)
        {
            totalPosition += obj.position;
        }
        return totalPosition / trackedObjects.Count;
    }

    private float CalculateMaxDistance(Vector3 midpoint)
    {
        float maxDistance = 0f;
        foreach (Transform obj in trackedObjects)
        {
            float distance = Vector3.Distance(midpoint, obj.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance;
    }
}




