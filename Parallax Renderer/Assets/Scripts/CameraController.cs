using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera parallaxCamera;
    public Transform target; // The target that the camera will look at
    public float parallaxFactor = 0.02f;
    public float smoothing = 0.1f; // Adjust this value to increase or decrease smoothing
    public float autoResetDelay = 5f; // Time in seconds before automatic reset
    public float wasdMoveSpeed = 10f;
    public float autoResetDelay = 1f; // Time in seconds before automatic reset
    public float depthMultiplier = 3f; // Adjust this value to increase or decrease depth

    private Vector3 initialMainCameraPos;
    private Vector3 initialParallaxCameraPos;
    private Vector3 lastPosition;

    void Start()
    {
        parallaxCamera = GetComponent<Camera>();

        if (parallaxCamera == null)
        {
            Debug.LogError("Camera not found, make sure this script is attached to an object with a Camera component!");
            return;
        }

        initialMainCameraPos = Vector3.zero;
        initialParallaxCameraPos = parallaxCamera.transform.position;
        lastPosition = initialMainCameraPos;

        // Start the coroutine for automatic reset
        StartCoroutine(AutoResetParallaxCamera());
    }

    void Update()
    {
        if (FaceTrackerReceiver.Instance != null)
        {
            var coords = FaceTrackerReceiver.Instance.coordinates;
            Vector3 vectorCoords = new Vector3(-coords.x, -coords.y, -coords.z * depthMultiplier);

            // Smooth the coordinates using Lerp
            Vector3 smoothedPosition = Vector3.Lerp(lastPosition, vectorCoords, smoothing);
            Vector3 delta = smoothedPosition - initialMainCameraPos;

            // Apply the parallax effect
            parallaxCamera.transform.position = initialParallaxCameraPos + delta * parallaxFactor;

            lastPosition = smoothedPosition; // Update the last position to the smoothed position
            // Make the camera look at the target
            if (target != null)
            {
                parallaxCamera.transform.LookAt(target);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetParallaxCamera();
            }
            lastPosition = smoothedPosition; // Update the last position to the smoothed position
        } 

    }

    private void ResetParallaxCamera()
    {
        parallaxCamera.transform.position = initialParallaxCameraPos;
        initialMainCameraPos = lastPosition; // Update the initial main camera position to the last position

        if (target != null)
        {
            parallaxCamera.transform.LookAt(target);
        }
    }

    private IEnumerator AutoResetParallaxCamera()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(autoResetDelay);
        ResetParallaxCamera();
    }
}
