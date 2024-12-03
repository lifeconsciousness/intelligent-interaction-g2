using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the wall moves towards the player
    public float destructionDistance = -5f; // Z-axis position at which the wall is destroyed
    private ObjectSpawner spawner;

    void Start()
    {
        // Find the ObjectSpawner to modify the score or handle game logic
        spawner = FindObjectOfType<ObjectSpawner>();
        if (spawner == null)
        {
            Debug.LogError("ObjectSpawner not found in the scene.");
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        // Move the wall towards the player along the Z-axis
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;

        // Destroy the wall if it passes the destruction distance
        if (transform.position.z < destructionDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle collision with the player (e.g., decrement score or health)
            if (spawner != null)
            {
                spawner.DecrementScore(); // Or another method to handle damage
            }

            // Destroy the wall on collision
            Destroy(gameObject);
        }
    }
}
