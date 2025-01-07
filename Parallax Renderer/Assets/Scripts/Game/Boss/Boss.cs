using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Hover settings
    public float hoverAmplitude = 0.5f;
    public float hoverSpeed = 2f;

    // Rotation settings
    public float rotationRadius = 1f;
    public float rotationSpeed = 2f;
    private Vector3 originalPosition;

    private float hoverTimer = 0f;
    private float rotationAngle = 0f;

    // Z-axis movement settings
    public float zMoveDistance = 2f; // How far it moves back and forth
    public float zMoveSpeed = 1f;

    // Random movement settings
    public float randomMoveSpeed = 1f;
    public float randomMoveDuration = 2f;
    private Vector3 randomTarget; // Target position for random movement

    // Boundaries
    public float xBoundary;
    public float yBoundary;

    // Switching behavior settings
    public float behaviorSwitchInterval = 5f;

    private int currentBehavior = 0;
    private bool isRotating = true;


    void Start()
    {
        originalPosition = transform.position;

        // StartCoroutine(SwitchBehaviors());
        GenerateRandomTarget();
    }


    void Update()
    {
        Hover();

        if (isRotating)
        {
            RotateCircular();
        }

        MoveZAxis(1.5f);

        // switch (currentBehavior)
        // {
        //     case 1:
        //         MoveZAxis();
        //         break;
        //     case 2:
        //         RandomMovement();
        //         break;
        //     case 3:
        //         MoveZAxis();
        //         RandomMovement();
        //         break;
        // }
    }

    // void MoveZAxis()
    // {
    //     float newZ = Mathf.Sin(Time.time * zMoveSpeed) * zMoveDistance;
    //     transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    // }

    void MoveZAxis(float pauseDuration)
    {
        // Calculate position based on time
        float cycleTime = Time.time % (zMoveSpeed * 2 + pauseDuration * 2); // Full cycle: forward, pause, backward, pause
        if (cycleTime < zMoveSpeed) // Moving forward
        {
            float newZ = Mathf.Lerp(-zMoveDistance, zMoveDistance, cycleTime / zMoveSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        }
        else if (cycleTime < zMoveSpeed + pauseDuration) // Pause after moving forward
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zMoveDistance);
        }
        else if (cycleTime < zMoveSpeed * 2 + pauseDuration) // Moving backward
        {
            float newZ = Mathf.Lerp(zMoveDistance, -zMoveDistance, (cycleTime - zMoveSpeed - pauseDuration) / zMoveSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        }
        else // Pause after moving backward
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zMoveDistance);
        }
    }


    void RandomMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, randomTarget, randomMoveSpeed * Time.deltaTime);

        // Generate a new random target if the current one is reached
        if (Vector3.Distance(transform.position, randomTarget) < 0.1f)
        {
            GenerateRandomTarget();
        }
    }

    void GenerateRandomTarget()
    {
        float randomX = Random.Range(-xBoundary, xBoundary);
        float randomY = Random.Range(-yBoundary, yBoundary);
        randomTarget = new Vector3(randomX, randomY, transform.position.z);
    }

    IEnumerator SwitchBehaviors()
    {
        while (true)
        {
            currentBehavior = (currentBehavior + 1) % 4; // Cycle through behaviors
            yield return new WaitForSeconds(behaviorSwitchInterval);
        }
    }

    void Hover()
    {
        hoverTimer += Time.deltaTime * hoverSpeed;

        // Calculate new Y position using sine wave
        float newY = originalPosition.y + Mathf.Sin(hoverTimer) * hoverAmplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void RotateCircular()
    {
        rotationAngle += Time.deltaTime * rotationSpeed;

        // Calculate new X and Z positions using a circular path
        float offsetX = Mathf.Cos(rotationAngle) * rotationRadius;
        float offsetZ = Mathf.Sin(rotationAngle) * rotationRadius;

        transform.position = new Vector3(originalPosition.x + offsetX, transform.position.y, originalPosition.z + offsetZ);
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
        // Reset rotation angle to avoid jumping when restarting
        rotationAngle = 0f;
    }
}
