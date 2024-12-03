using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The object to spawn
    public float spawnRate = 1f;     // How often to spawn objects
    public float objectSpeedMin = 5f;   // Speed of objects towards the player (on the z-axis)
    public float objectSpeedMax = 10f;  // Speed of objects towards the player (on the z-axis)
    public float destructionDistance = -5f; // Distance at which the object gets destroyed
    public int score = 0;            // The player's score

    public float wallSpawnRate = 10f; // How often to spawn walls
    public float wallSpeed = 15f;     // Speed at which the wall moves towards the player
    public float wallDestructionDistance = -5f; // Distance at which the wall gets destroyed
    public Quaternion spawnRotation = Quaternion.identity;
    private float nextSpawnTime = 0f;
    private float nextWallSpawnTime = 0f;
    private CameraController cameraController;
    private AsymFrustum asymFrustum;

    private Vector3 initialPosition;
    public float spawnDistance = 100f;

    // list of all wall variants
    public GameObject[] wallVariants;
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        asymFrustum = FindObjectOfType<AsymFrustum>();

        if (cameraController == null || asymFrustum == null)
        {
            Debug.LogError("Cannot find camera controller or asymmetrical frustum.");
        }

        initialPosition = cameraController.initialPosition;

        // Initialize next wall spawn time
        nextWallSpawnTime = Time.time + wallSpawnRate;
    }

    void Update()
    {
        // Check if it's time to spawn a new object
        if (Time.time >= nextSpawnTime && !GameManager.Instance.isGameOver)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }

        // Check if it's time to spawn a new wall
        if (Time.time >= nextWallSpawnTime && !GameManager.Instance.isGameOver)
        {
            SpawnWall();
            nextWallSpawnTime = Time.time + wallSpawnRate;
        }
    }

    void SpawnEnemy()
    {
        // Randomize the spawn position around the spawner's position
        float randomX = initialPosition.x + Random.Range(-asymFrustum.width / 2, asymFrustum.width / 2);
        float randomY = initialPosition.y + Random.Range(-asymFrustum.height / 2, asymFrustum.height / 2);
        float spawnZ = initialPosition.z + spawnDistance;

        Vector3 spawnPosition = new Vector3(randomX, randomY, spawnZ);

        // Instantiate the object
        GameObject obj = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
        obj.tag = "Enemy";

        // Add a script to move the object and destroy it when it reaches the player
        obj.AddComponent<ObjectMover>().Initialize(Random.Range(objectSpeedMin, objectSpeedMax), destructionDistance, this);
    }

    void SpawnWall()
    {
        // Define wall spawn position similar to objects or in a specific pattern
        float randomX = initialPosition.x; // Centered on X; adjust as needed
        float randomY = initialPosition.y; // Centered on Y; adjust as needed
        float spawnZ = initialPosition.z + spawnDistance;

        Vector3 spawnPosition = new Vector3(randomX, randomY, spawnZ);

        GameObject randomWall = wallVariants[Random.Range(0, wallVariants.Length)];

        // Instantiate the wall
        GameObject wall = Instantiate(randomWall, spawnPosition, spawnRotation);

        // Configure the WallOfDeath script
        WallOfDeath wallScript = wall.GetComponent<WallOfDeath>();
        if (wallScript != null)
        {
            wallScript.moveSpeed = wallSpeed;
            wallScript.destructionDistance = wallDestructionDistance; // Or use a separate value if needed
            // You can set additional properties here if desired
        }
        else
        {
            Debug.LogError("Wall prefab does not have a WallOfDeath script attached.");
        }
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
