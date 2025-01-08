using UnityEngine;

[System.Serializable]
public class SpawnEvent
{
    public float spawnTime; // Time in seconds when this event should trigger
    public GameObject enemyPrefab; // Enemy prefab to spawn

    public bool useCoordinates = false; // If true, use x, y, z coordinates instead of a Transform

    public Transform spawnLocation; // Exact location to spawn the enemy (used if useCoordinates is false)
    public Vector3 spawnCoordinates; // Coordinates to spawn the enemy (used if useCoordinates is true)

    public int quantity = 1; // Number of enemies to spawn

    public bool useRandomPosition = false; // For random positions within an area
    public Vector3 areaSize; // Size of the area to randomly spawn enemies

    public float spawnInterval = 0f; // Interval between spawns in the wave

    public bool visualize = false; // Whether to show spawn location gizmos for this event
}