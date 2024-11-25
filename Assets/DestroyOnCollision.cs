using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject particleEffectPrefab; // Assign your particle effect prefab in the inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the particleEffectPrefab is assigned
        if (particleEffectPrefab != null)
        {
            // Instantiate the particle effect at the collision point
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the object
        Destroy(gameObject);
    }
}


