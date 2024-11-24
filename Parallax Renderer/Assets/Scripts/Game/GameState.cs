using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Player health
    public int playerHealth = 100;

    public bool isGameOver = false;
    public GameObject gameOverScreen;
    public TMP_Text healthText;

    void Start()
    {
        gameOverScreen.SetActive(false);
        healthText.text = "HP: " + playerHealth;
    }

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            restartGame();
        }
    }

    // Method to reduce player health
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("Player has died.");
            isGameOver = true;
            gameOverScreen.SetActive(true);
            // You could add additional logic for game over or player death here
        }
        Debug.Log("Player Health: " + playerHealth);
        healthText.text = "HP: " + playerHealth;
    }

    // Method to increase player health
    public void HealPlayer(int healAmount)
    {
        playerHealth += healAmount;
        Debug.Log("Player Health: " + playerHealth);
    }

    public void restartGame()
    {
        playerHealth = 100;
        isGameOver = false;
        gameOverScreen.SetActive(false);
        healthText.text = "HP: " + playerHealth;
    }
}
