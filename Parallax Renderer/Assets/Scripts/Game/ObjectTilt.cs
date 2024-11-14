using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPositionRotation : MonoBehaviour
{
    public GameObject objectPrefab;  // The object to rotate (e.g., monkey)
    public float rotateSpeed = 100f; // Speed at which the object rotates (degrees per second)
    public float maxRotation = 45f; // Maximum rotation angle from the default position
    public float returnSpeed = 5f;  // Speed at which the object returns to its neutral rotation

    private float currentRotation = 0f; // Current rotation of the object relative to the default position
    private float targetRotation = 0f;  // Target rotation based on user input

    private Quaternion initialRotation;  // Stores the object's initial (default) rotation

    void Start()
    {
        // Store the initial rotation of the object when the game starts
        initialRotation = objectPrefab.transform.rotation;
    }

    void Update()
    {
        // Get horizontal input for rotating the object (e.g., A/D or arrow keys)
        targetRotation = Input.GetAxis("Horizontal") * maxRotation;

        // Smoothly interpolate the current rotation towards the target rotation
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * rotateSpeed);

        // Apply the new rotation to the objectPrefab based on the initial rotation (default position)
        objectPrefab.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, -currentRotation);

        // Smoothly return the object to its initial rotation when no input is detected
        if (Mathf.Abs(targetRotation) < 0.1f)
        {
            currentRotation = Mathf.Lerp(currentRotation, 0f, Time.deltaTime * returnSpeed);
            objectPrefab.transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, currentRotation);
        }
    }
}
