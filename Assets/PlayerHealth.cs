using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    Animator animator;

    [SerializeField] public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private string teamTag = "Team 1";

    [SerializeField] private Slider healthSlider; // Reference to the Slider component

    private GameObject winCanvasTeam1;
    private GameObject winCanvasTeam2;

    private void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        winCanvasTeam1 = GameObject.FindWithTag("Team1WinCanvas");
        winCanvasTeam2 = GameObject.FindWithTag("Team2WinCanvas");

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Set slider max value
            healthSlider.value = currentHealth; // Initialize slider value
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string ballTag = collision.gameObject.tag;
        if ((teamTag == "Team 1" && ballTag == "Team 2") || (teamTag == "Team 2" && ballTag == "Team 1"))
        {
            TakeDamage(10);
            animator.SetTrigger("HitTrigger");

        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        UpdateHealthSlider(); // Update the slider whenever health changes

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Sync the slider value with current health
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





