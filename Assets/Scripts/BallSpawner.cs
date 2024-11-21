using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    private int maxBalls = 5;
    [SerializeField] float holdTimeThreshold = 0.5f;  
    private float holdTime = 0f;             
    private bool isHoldingButton = false;    

    public void OnMakeBall(InputAction.CallbackContext context)
    {
        if (context.started)
        {            
            isHoldingButton = true;            
        }

        if (context.canceled)
        {            
            isHoldingButton = false;
            holdTime = 0f; 
        }
    }

    private void Update()
    {        
        if (isHoldingButton)
        {
            holdTime += Time.deltaTime;

            
            if (holdTime >= holdTimeThreshold && transform.childCount < maxBalls)
            {
                SpawnBall();
                holdTime = 0f; 
                isHoldingButton = false; 
            }
        }
    }

    private void SpawnBall()
    {        
        GameObject spawnedBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedBall.transform.parent = transform;

        spawnedBall.tag = this.tag;

        spawnedBall.SetActive(false);
    }
}



