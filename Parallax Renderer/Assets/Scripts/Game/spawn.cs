using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;  // The object to spawn
    public float spawnRate = 1f;     // How often to spawn objects
    public float objectSpeed = 5f;   // Speed of objects towards the player (on the z-axis)
    public Transform player;         // Reference to the player (camera)
    public float destructionDistance = 1f; // Distance at which the object gets destroyed
    public int score = 0;            // The player's score

    public Quaternion spawnRotation = Quaternion.identity;
    private float nextSpawnTime = 0f;

    void Update()
    {
        // Update spawner's position to match player's x and y, but keep its z unchanged
        Vector3 playerPosition = player.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

        // Check if it's time to spawn a new object
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }

    }

    void SpawnObject()
    {
        // Randomize the spawn position (this can be adjusted as needed)
        // Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 20f);

        // Get the spawner's current position
        Vector3 spawnerPosition = transform.position;

        // Randomize the spawn position around the spawner's position
        float randomX = spawnerPosition.x + Random.Range(-10f, 10f);
        float randomY = spawnerPosition.y + Random.Range(-10f, 10f);
        float spawnZ = spawnerPosition.z + 20f;

        Vector3 spawnPosition = new Vector3(randomX, randomY, spawnZ);

        // Instantiate the object
        GameObject obj = Instantiate(objectPrefab, spawnPosition, spawnRotation);
        obj.tag = "Enemy0";

        // Add a script to move the object and destroy it when it reaches the player
        obj.AddComponent<ObjectMover>().Initialize(player, objectSpeed, destructionDistance, this);
    }

    // Function to increment the score
    public void IncrementScore()
    {
        score++;
        Debug.Log("Score: " + score);
    }

    // Function to decrement the score
    public void DecrementScore()
    {
        score--;
        Debug.Log("Score: " + score);
    }
}

public class ObjectMover : MonoBehaviour
{
    private Transform player;
    private float speed;
    private float destructionDistance;
    private ObjectSpawner spawner;  // Reference to the spawner to update score

    // Initialize the object with the player's reference, speed, destruction distance, and spawner reference
    public void Initialize(Transform playerTransform, float moveSpeed, float destroyDistance, ObjectSpawner objectSpawner)
    {
        player = playerTransform;
        speed = moveSpeed;
        destructionDistance = destroyDistance;
        spawner = objectSpawner;
    }

    void Update()
    {
        // Move the object towards the player's z-axis, but move further after passing the player's z-axis
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, player.position.y, player.position.z - 10f), speed * Time.deltaTime);

        // Destroy the object if it moves behind the player (past the camera)
        if (transform.position.z < player.position.z - destructionDistance)
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
