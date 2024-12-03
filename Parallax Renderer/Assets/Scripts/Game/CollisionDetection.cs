using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the other object is one of the spawned objects by tag or component
        if (other.CompareTag("Enemy"))  // Or use a specific component check
        {
            // Handle collision logic, e.g., destroy object, increase score, etc.
            Destroy(other.gameObject);
            // Debug.Log("Collision detected with spawned object!");

            // Reduces player health by 10
            GameManager.Instance.TakeDamage(10);
        }

        if (other.CompareTag("Wall"))
        {
            // Handle collision with the wall, e.g., destroy wall, decrease score, etc.
            Destroy(other.gameObject);
            // Debug.Log("Collision detected with wall!");

            // Decrement the score through the spawner
            GameManager.Instance.TakeDamage(1000);
        }
    }
}
