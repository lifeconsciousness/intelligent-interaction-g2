using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class FaceTrackerReceiver : MonoBehaviour
{
    private static FaceTrackerReceiver instance;
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    [System.Serializable]
    public class FaceCoordinates
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class FaceCoordinatesMinMax
    {
        public float minX = float.MaxValue;
        public float maxX = float.MinValue;
        public float minY = float.MaxValue;
        public float maxY = float.MinValue;
        public float minZ = float.MaxValue;
        public float maxZ = float.MinValue;

        public void Reset()
        {
            minX = float.MaxValue;
            maxX = float.MinValue;
            minY = float.MaxValue;
            maxY = float.MinValue;
            minZ = float.MaxValue;
            maxZ = float.MinValue;
        }
    }

    public FaceCoordinates coordinates;
    public FaceCoordinatesMinMax minMaxValues; // Instance to hold min/max values

    // Offset to apply when resetting position
    private Vector3 resetOffset;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps the object between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    void Start()
    {
        // Existing start code
        udpClient = new UdpClient(5005);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        minMaxValues.Reset(); // Initialize the min/max values on start
    }

    void Update()
    {
        if (udpClient.Available > 0)
        {
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            string json = System.Text.Encoding.UTF8.GetString(data);
            FaceCoordinates receivedCoordinates = JsonUtility.FromJson<FaceCoordinates>(json);

            // Update coordinates relative to the current position
            coordinates.x = -(receivedCoordinates.x - resetOffset.x);
            coordinates.y = -(receivedCoordinates.y - resetOffset.y);
            coordinates.z = receivedCoordinates.z - resetOffset.z;

            // Update min/max values based on the received coordinates
            UpdateMinMaxValues();
        }

        // Check for 'R' key press to reset position
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    private void UpdateMinMaxValues()
    {
        // Update min/max for x
        if (coordinates.x < minMaxValues.minX) minMaxValues.minX = coordinates.x;
        if (coordinates.x > minMaxValues.maxX) minMaxValues.maxX = coordinates.x;

        // Update min/max for y
        if (coordinates.y < minMaxValues.minY) minMaxValues.minY = coordinates.y;
        if (coordinates.y > minMaxValues.maxY) minMaxValues.maxY = coordinates.y;

        // Update min/max for z
        if (coordinates.z < minMaxValues.minZ) minMaxValues.minZ = coordinates.z;
        if (coordinates.z > minMaxValues.maxZ) minMaxValues.maxZ = coordinates.z;
    }

    public void ResetPosition()
    {
        // Save the current coordinates as the offset
        resetOffset = new Vector3(coordinates.x, coordinates.y, coordinates.z);

        // Reset min/max values
        minMaxValues.Reset();

        // The coordinates will be updated next frame when new data is received
        coordinates.x = 0;
        coordinates.y = 0;
        coordinates.z = 0;

        Debug.Log($"Position and offsets have been reset.");
    }

    public static FaceTrackerReceiver Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("FaceTrackerReceiver is not initialized");
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        udpClient.Close();
    }
}
