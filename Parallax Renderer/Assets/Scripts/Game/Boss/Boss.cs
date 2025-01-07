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

        MoveZAxis(10f, 1f, 4f);

        RandomMovement();

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


    private float initialZ; // Stores the starting Z position
    private float movementStartTime; 

    void MoveZAxis(float zMoveDistance, float zMoveSpeed, float pauseDuration)
    {
        // Initialize starting position and time if needed
        if (movementStartTime == 0f)
        {
            initialZ = transform.position.z; // Store the current Z position
            movementStartTime = Time.time;  // Start time of the movement
        }

        // Calculate the total cycle duration
        float cycleDuration = 2 * zMoveSpeed + 2 * pauseDuration;

        // Calculate elapsed time since the movement started
        float elapsedTime = (Time.time - movementStartTime) % cycleDuration;

        float forwardZ = initialZ + zMoveDistance;
        float backwardZ = initialZ - zMoveDistance;

        // Determine phase of movement within the cycle
        if (elapsedTime < zMoveSpeed) // Moving forward
        {
            float t = elapsedTime / zMoveSpeed;
            float newZ = Mathf.Lerp(backwardZ, forwardZ, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        }
        else if (elapsedTime < zMoveSpeed + pauseDuration) // Pausing at forward end
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, forwardZ);
        }
        else if (elapsedTime < zMoveSpeed * 2 + pauseDuration) // Moving backward
        {
            float t = (elapsedTime - zMoveSpeed - pauseDuration) / zMoveSpeed;
            float newZ = Mathf.Lerp(forwardZ, backwardZ, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        }
        else // Pausing at backward end
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, backwardZ);
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
