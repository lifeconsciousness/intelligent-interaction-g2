using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Global variables
    public float GameTime { get; private set; } = 0f; // Total game time passed
    public int PlayerHealth { get; private set; } = 100; // Player health

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        GameTime += Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
        if (PlayerHealth < 0)
            PlayerHealth = 0;

        if (PlayerHealth == 0)
        {
            // Handle player death
            Debug.Log("Player has died!");
        }

        Debug.Log("Player health: " + PlayerHealth);
    }

    public void Heal(int amount)
    {
        PlayerHealth += amount;
        if (PlayerHealth > 100)
            PlayerHealth = 100;
    }
}