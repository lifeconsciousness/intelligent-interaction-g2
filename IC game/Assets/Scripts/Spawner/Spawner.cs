using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public List<SpawnEvent> spawnEvents = new List<SpawnEvent>();
    private float gameTimer = 0f;
    private int nextEventIndex = 0;

    void Start()
    {
        spawnEvents.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));
    }

    void Update()
    {
        gameTimer += Time.deltaTime;

        while (nextEventIndex < spawnEvents.Count && gameTimer >= spawnEvents[nextEventIndex].spawnTime)
        {
            StartCoroutine(SpawnEnemies(spawnEvents[nextEventIndex]));
            nextEventIndex++;
        }
    }

    private IEnumerator<UnityEngine.WaitForSeconds> SpawnEnemies(SpawnEvent spawnEvent)
    {
        for (int i = 0; i < spawnEvent.quantity; i++)
        {
            Vector3 spawnPosition;
            Vector3 centerPosition;

            if (spawnEvent.useCoordinates)
            {
                centerPosition = spawnEvent.spawnCoordinates;
            }
            else if (spawnEvent.spawnLocation != null)
            {
                centerPosition = spawnEvent.spawnLocation.position;
            }
            else
            {
                Debug.LogWarning("Spawn location is not set for a spawn event!");
                yield break;
            }

            if (spawnEvent.useRandomPosition)
            {
                spawnPosition = GetRandomPositionInArea(centerPosition, spawnEvent.areaSize);
            }
            else
            {
                spawnPosition = centerPosition;
            }

            Instantiate(spawnEvent.enemyPrefab, spawnPosition, Quaternion.identity);

            // Wait for the specified interval before spawning the next enemy
            if (spawnEvent.spawnInterval > 0f && i < spawnEvent.quantity - 1)
            {
                yield return new WaitForSeconds(spawnEvent.spawnInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector3 GetRandomPositionInArea(Vector3 center, Vector3 size)
    {
        Vector3 randomPos = new Vector3(
            Random.Range(center.x - size.x / 2f, center.x + size.x / 2f),
            center.y,
            Random.Range(center.z - size.z / 2f, center.z + size.z / 2f)
        );
        return randomPos;
    }

    void OnDrawGizmos()
    {
        foreach (var spawnEvent in spawnEvents)
        {
            if (!spawnEvent.visualize)
                continue;

            Vector3 centerPosition;

            if (spawnEvent.useCoordinates)
            {
                centerPosition = spawnEvent.spawnCoordinates;
            }
            else if (spawnEvent.spawnLocation != null)
            {
                centerPosition = spawnEvent.spawnLocation.position;
            }
            else
            {
                continue;
            }

            if (spawnEvent.useRandomPosition)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(centerPosition, spawnEvent.areaSize);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(centerPosition, 0.5f);
            }
        }
    }
}