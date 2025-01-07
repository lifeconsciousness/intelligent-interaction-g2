using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private CameraController cameraController;
    private Vector3 initialPosition;

    public GameObject[] bossVariants;

    public float spawnDistance = 40f;
    public Quaternion spawnRotation = Quaternion.identity;



    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        // asymFrustum = FindObjectOfType<AsymFrustum>();

        // if (cameraController == null || asymFrustum == null)
        if (cameraController == null)
        {
            Debug.LogError("Cannot find camera controller or asymmetrical frustum.");
        }

        initialPosition = cameraController.initialPosition;
    }

    void SpawnWall()
    {
        // Define wall spawn position similar to objects or in a specific pattern
        float randomX = initialPosition.x; // Centered on X; adjust as needed
        float randomY = initialPosition.y; // Centered on Y; adjust as needed
        float spawnZ = initialPosition.z + spawnDistance;

        Vector3 spawnPosition = new Vector3(randomX, randomY, spawnZ);

        GameObject selectedBoss = bossVariants[0];

        // Instantiate the wall
        GameObject boss = Instantiate(selectedBoss, spawnPosition, spawnRotation);

        // Configure the WallOfDeath script
        // WallOfDeath wallScript = wall.GetComponent<WallOfDeath>();
        // if (wallScript != null)
        // {
        //     wallScript.moveSpeed = wallSpeed;
        //     wallScript.destructionDistance = wallDestructionDistance; // Or use a separate value if needed
        //     // You can set additional properties here if desired
        // }
        // else
        // {
        //     Debug.LogError("Wall prefab does not have a WallOfDeath script attached.");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
