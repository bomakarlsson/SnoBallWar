using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private string teamTag = "Team 1";

    private GameObject winCanvasTeam1;
    private GameObject winCanvasTeam2;

    private void Start()
    {
        currentHealth = maxHealth;

        winCanvasTeam1 = GameObject.FindWithTag("Team1WinCanvas");
        winCanvasTeam2 = GameObject.FindWithTag("Team2WinCanvas");

        /*
        if (winCanvasTeam1 != null)
        {
            ToggleCanvasChildren(winCanvasTeam1, false); // Ensure children are deactivated
            Debug.Log("Team1WinCanvas found and its children deactivated!");
        }
        else
        {
            Debug.LogError("Team1WinCanvas not found!");
        }

        if (winCanvasTeam2 != null)
        {
            ToggleCanvasChildren(winCanvasTeam2, false); // Ensure children are deactivated
            Debug.Log("Team2WinCanvas found and its children deactivated!");
        }
        else
        {
            Debug.LogError("Team2WinCanvas not found!");
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string ballTag = collision.gameObject.tag;
        if ((teamTag == "Team 1" && ballTag == "Team 2") || (teamTag == "Team 2" && ballTag == "Team 1"))
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        gameObject.SetActive(false);

        // Check if there are any remaining teammates
        bool hasTeammates = false;
        foreach (GameObject teammate in GameObject.FindGameObjectsWithTag(teamTag))
        {
            if (teammate.activeSelf && teammate != this.gameObject)
            {
                hasTeammates = true;
                break;
            }
        }

        // Activate the children of the win canvas for the opposing team if no teammates remain
        if (!hasTeammates)
        {
            if (teamTag == "Team 1" && winCanvasTeam2 != null)
            {
                ToggleCanvasChildren(winCanvasTeam2, true);
                Debug.Log("Team 2 wins! Activating Team2WinCanvas children.");
            }
            else if (teamTag == "Team 2" && winCanvasTeam1 != null)
            {
                ToggleCanvasChildren(winCanvasTeam1, true);
                Debug.Log("Team 1 wins! Activating Team1WinCanvas children.");
            }
        }
    }

    private void ToggleCanvasChildren(GameObject canvas, bool activate)
    {
        // Iterate through all children and toggle their active state
        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(activate);
        }
    }
}




