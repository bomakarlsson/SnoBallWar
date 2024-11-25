using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    int index = 0;
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
        // Determine the index of the prefab in the `fighters` list
        int prefabIndex = fighters.IndexOf(prefab);

        // If the index is invalid, return null as a fallback
        if (prefabIndex == -1) return null;

        // Use odd/even logic to select spawn points
        if (prefabIndex % 2 == 0 && prefab2SpawnPoints.Count > 0)
        {
            // Even index -> Use prefab2SpawnPoints
            return prefab2SpawnPoints[Random.Range(0, prefab2SpawnPoints.Count)];
        }
        else if (prefabIndex % 2 != 0 && prefab1SpawnPoints.Count > 0)
        {
            // Odd index -> Use prefab1SpawnPoints
            return prefab1SpawnPoints[Random.Range(0, prefab1SpawnPoints.Count)];
        }

        // Fallback if no spawn points are available
        return null;
    }

}
