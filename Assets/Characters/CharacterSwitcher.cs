using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    int index = 0; // Tracks which prefab to spawn next
    [SerializeField] List<GameObject> fighters = new List<GameObject>(); 
    [SerializeField] List<Transform> prefab1SpawnPoints = new List<Transform>(); 
    [SerializeField] List<Transform> prefab2SpawnPoints = new List<Transform>(); 

    PlayerInputManager manager;

    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        if (fighters.Count > 0)
        {
            manager.playerPrefab = fighters[index];
        }
    }

    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        if (fighters.Count > 0)
        {            
            index = (index + 1) % fighters.Count;
            manager.playerPrefab = fighters[index];
                        
            Transform spawnPoint = GetSpawnPointForPrefab(fighters[index]);
            if (spawnPoint != null)
            {
                input.transform.position = spawnPoint.position;
            }
        }
    }

    private Transform GetSpawnPointForPrefab(GameObject prefab)
    {
        if (prefab == fighters[0] && prefab1SpawnPoints.Count > 0)
        {
            // Get a random or sequential spawn point for prefab 1
            return prefab1SpawnPoints[Random.Range(0, prefab1SpawnPoints.Count)];
        }
        else if (prefab == fighters[1] && prefab2SpawnPoints.Count > 0)
        {
            // Get a random or sequential spawn point for prefab 2
            return prefab2SpawnPoints[Random.Range(0, prefab2SpawnPoints.Count)];
        }
        return null; // Fallback if no spawn points are available
    }
}
