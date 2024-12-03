using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private Transform player;
    private float speed;
    private float destructionDistance;
    private ObjectSpawner spawner;  // Reference to the spawner to update score

    // Initialize the object with the player's reference, speed, destruction distance, and spawner reference
    public void Initialize(float moveSpeed, float destroyDistance, ObjectSpawner objectSpawner)
    {
        speed = moveSpeed;
        destructionDistance = destroyDistance;
        spawner = objectSpawner;
        player = FindObjectOfType<CameraController>().transform;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }


        // Capture the current position
        Vector3 currentPosition = transform.position;

        // Create a new position for the next step along the Z-axis towards the player
        Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y, player.position.z - destructionDistance - 1f);

        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // Destroy the object if it moves behind the player (past the camera)
        if (transform.position.z < player.position.z - destructionDistance)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
        }
    }

    // Handle collision with the player
    void OnTriggerEnter(Collider other)
    {
        // Check if the object collides with the player
        if (other.CompareTag("Player"))
        {
            // Decrement the score through the spawner
            spawner.DecrementScore();

            // Destroy the object
            Destroy(gameObject);
        }
    }
}
