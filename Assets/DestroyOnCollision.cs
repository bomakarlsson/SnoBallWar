using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject particleEffectPrefab; 

    private void OnCollisionEnter2D(Collision2D collision)
    {       
        if (particleEffectPrefab != null)
        {           
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }
                
        Destroy(gameObject);
    }
}


