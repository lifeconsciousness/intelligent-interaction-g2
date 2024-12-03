using UnityEngine;
using UnityEngine.UI; // Import the UI namespace

public class CameraController : MonoBehaviour
{
    private AsymFrustum asymFrustum; // Reference to the AsymFrustum script
    private FaceTrackerReceiver faceTracker;
    public float cameraDistance;

    // Smoothing parameters
    public float smoothTime = 0.3f; // Time to smooth to the target position
    private Vector3 targetPosition; // The target position the camera should move towards
    private Vector3 currentVelocity = Vector3.zero; // Current velocity for smooth dampening

    public Vector3 initialPosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        initialPosition = mainCamera.transform.position; // Store the initial position of the camera
        cameraDistance = mainCamera.transform.position.z; // Get the initial camera distance

        // print out camera distance
        Debug.Log("Camera distance: " + cameraDistance);


        asymFrustum = FindObjectOfType<AsymFrustum>();
        if (asymFrustum == null)
        {
            Debug.LogError("AsymFrustum reference is not set. Please assign it in the Inspector.");
        }

        faceTracker = FaceTrackerReceiver.Instance;
        if (faceTracker == null)
        {
            Debug.LogError("FaceTrackerReceiver instance is not set. Please ensure it exists in the scene.");
        }
    }

    void Update()
    {
        if (asymFrustum == null || faceTracker == null)
        {
            return;
        }

        MapAndSetCameraPosition();
    }

    private void MapAndSetCameraPosition()
    {
        // Real-world boundaries
        float realMinX = faceTracker.minMaxValues.minX; // Get these from FaceTrackerReceiver
        float realMaxX = faceTracker.minMaxValues.maxX;
        float realMinY = faceTracker.minMaxValues.minY;
        float realMaxY = faceTracker.minMaxValues.maxY;

        // Virtual world boundaries
        float virtualMinX = -asymFrustum.width / 2;
        float virtualMaxX = asymFrustum.width / 2;
        float virtualMinY = -asymFrustum.height / 2;
        float virtualMaxY = asymFrustum.height / 2;

        // Map real-world coordinates to virtual coordinates
        float mappedX = MapValue(faceTracker.coordinates.x, realMinX, realMaxX, virtualMinX, virtualMaxX);
        float mappedY = MapValue(faceTracker.coordinates.y, realMinY, realMaxY, virtualMinY, virtualMaxY);

        // Calculate the target position in world space
        targetPosition = initialPosition + new Vector3(mappedX, mappedY, cameraDistance);

        targetPosition.z = cameraDistance; // Ensure the camera distance is maintained

        // Smoothly move to the target position
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

    private float MapValue(float value, float realMin, float realMax, float virtualMin, float virtualMax)
    {
        // Handle cases where realMax equals realMin to avoid division by zero
        if (realMax == realMin)
        {
            return virtualMin; // Or handle it in a way that suits your application
        }

        // Normalize the value to the range [0, 1]
        float normalizedValue = (value - realMin) / (realMax - realMin);

        // Scale to the virtual range
        return normalizedValue * (virtualMax - virtualMin) + virtualMin;
    }
}
