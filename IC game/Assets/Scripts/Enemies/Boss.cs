using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float xBoundary = 10f; // Maximum distance the object can move on the x-axis
    public float yBoundary = 10f; // Maximum distance the object can move on the y-axis
    // public float speed = 2f; // Speed of movement

    public bool isRotating = true; // Enable/disable rotation
    public bool isHovering = true; // Enable/disable rotation
    public float rotationSpeed = 1f; // Speed of rotation
    public float rotationRadius = 2f; // Radius of circular rotation

    public float hoverSpeed = 1f; // Speed of hovering
    public float hoverAmplitude = 0.5f; // Amplitude of hovering

    private Vector2 targetPosition;
    private float rotationAngle = 0f;
    private float hoverTimer = 0f;

    // private Vector3 originalPosition;

    // void Start()
    // {
    //     originalPosition = transform.position;

    //         // speed = 2f;
    // }

    // void Update()
    // {

    //     // RandomMove();
    // }

    // doesn't work in combination with rotation or hover
    public void RandomMove()
    {
        if (targetPosition == Vector2.zero || Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            float randomX = Random.Range(-xBoundary, xBoundary);
            float randomY = Random.Range(-yBoundary, yBoundary);
            targetPosition = new Vector2(randomX, randomY);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void RotateCircular()
    {
        if (!isRotating)
        {
            return; 
        }

        rotationAngle += Time.deltaTime * rotationSpeed;

        // Calculate new X and Z positions using a circular path
        float offsetX = Mathf.Cos(rotationAngle) * rotationRadius;
        float offsetZ = Mathf.Sin(rotationAngle) * rotationRadius;

        transform.position = new Vector3(originalPosition.x + offsetX, transform.position.y, originalPosition.z + offsetZ);
    }

    public void Hover()
    {
        if (!isHovering)
        {
            return; // Exit the function if Z movement is disabled
        }

        hoverTimer += Time.deltaTime * hoverSpeed;

        // Calculate new Y position using sine wave
        float newY = originalPosition.y + Mathf.Sin(hoverTimer) * hoverAmplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void MoveObject(string axis, float direction, float speed, float timeout)
    {
        StartCoroutine(MoveCoroutine(axis, direction, speed, timeout));
    }

    private IEnumerator MoveCoroutine(string axis, float direction, float speed, float timeout)
    {
        float elapsedTime = 0f;

        while (elapsedTime < timeout)
        {
            Vector3 movement = Vector3.zero;

            if (axis.ToLower() == "x")
            {
                movement = new Vector3(direction * speed * Time.deltaTime, 0, 0);
            }
            else if (axis.ToLower() == "y")
            {
                movement = new Vector3(0, direction * speed * Time.deltaTime, 0);
            }
            else if (axis.ToLower() == "z")
            {
                movement = new Vector3(0, 0, direction * speed * Time.deltaTime);
            }

            transform.position += movement;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    protected override void Move()
    {
        RotateCircular();
        Hover();
    }
}
