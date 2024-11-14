using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Player health
    public int playerHealth = 100;

    void Awake()
    {
        // Implement Singleton pattern to ensure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Preserve this object across scenes
    }

    // Method to reduce player health
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
            Debug.Log("Player has died.");
            // You could add additional logic for game over or player death here
        }
        Debug.Log("Player Health: " + playerHealth);
    }

    // Method to increase player health
    public void HealPlayer(int healAmount)
    {
        playerHealth += healAmount;
        Debug.Log("Player Health: " + playerHealth);
    }
}
