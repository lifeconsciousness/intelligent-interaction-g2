using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Global variables
    public float GameTime { get; private set; } = 0f; // Total game time passed
    public int PlayerHealth { get; private set; } = 100; // Player health

    // UI elements
    public TMPro.TextMeshProUGUI HealthText;
    public GameObject damageIndicator;

    // Damage cooldown
    public float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (HealthText == null) {
            Debug.LogError("HealthText is not set in the GameManager!");
        }
        HealthText.SetText(PlayerHealth + "♥︎");
    }

    private void Update()
    {
        GameTime += Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        damageIndicator.SetActive(true);
        Invoke(nameof(HideDamageIndicator), damageCooldown);

        PlayerHealth -= damage;
        if (PlayerHealth <= 0) {
            PlayerHealth = 0;
            // Handle player death
            SceneManager.LoadScene("Menu");
        }

        HealthText.SetText(PlayerHealth + "♥︎");
    }

    private void HideDamageIndicator()
    {
        damageIndicator.SetActive(false);
    }

    public void Heal(int amount)
    {
        PlayerHealth += amount;
        if (PlayerHealth > 100)
            PlayerHealth = 100;

        HealthText.SetText(PlayerHealth + "♥︎");
    }
}