using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;  // The object to spawn
    public float spawnRate = 1f;     // How often to spawn objects
    public float objectSpeedMin = 5f;   // Speed of objects towards the player (on the z-axis)
    public float objectSpeedMax = 10f;  // Speed of objects towards the player (on the z-axis)
    public Transform player;         // Reference to the player (camera)
    public float destructionDistance = -5f; // Distance at which the object gets destroyed
    public int score = 0;            // The player's score

    public Quaternion spawnRotation = Quaternion.identity;
    private float nextSpawnTime = 0f;
    private CameraController cameraController;
    private AsymFrustum asymFrustum;

    private Vector3 initialPosition;
    public float spawnDistance = 100f;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        asymFrustum = FindObjectOfType<AsymFrustum>();

        if (cameraController == null || asymFrustum == null)
        {
            Debug.LogError("Cannot find camera controller or asymmetrical frustum.");
        }

        initialPosition = cameraController.initialPosition;


    }

    void Update()
    {
        // Update spawner's position to match player's x and y, but keep its z unchanged
        Vector3 playerPosition = player.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

        // Check if it's time to spawn a new object
        if (Time.time >= nextSpawnTime && !GameManager.Instance.isGameOver)
        {
            SpawnObject();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }

    }

    void SpawnObject()
    {
        // Randomize the spawn position (this can be adjusted as needed)
        // Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 20f);

        // Randomize the spawn position around the spawner's position
        float randomX = initialPosition.x + Random.Range(-asymFrustum.width / 2, asymFrustum.width / 2);
        float randomY = initialPosition.y + Random.Range(-asymFrustum.height / 2, asymFrustum.height / 2);
        float spawnZ = initialPosition.z + spawnDistance;

        Vector3 spawnPosition = new Vector3(randomX, randomY, spawnZ);

        // Instantiate the object
        GameObject obj = Instantiate(objectPrefab, spawnPosition, spawnRotation);
        obj.tag = "Enemy0";

        // Add a script to move the object and destroy it when it reaches the player
        obj.AddComponent<ObjectMover>().Initialize(player, Random.Range(objectSpeedMin, objectSpeedMax), destructionDistance, this);
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
    public GameObject gameState;

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
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        // Capture the current position
        Vector3 currentPosition = transform.position;

        // Create a new position for the next step along the Z-axis towards the player
        Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y, player.position.z - 10f);

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
