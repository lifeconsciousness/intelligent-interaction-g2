using System.Net;
using System.Net.Sockets;
using UnityEngine;

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

    public FaceCoordinates coordinates;

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
        udpClient = new UdpClient(5005);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update()
    {
        if (udpClient.Available > 0)
        {
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            string json = System.Text.Encoding.UTF8.GetString(data);
            coordinates = JsonUtility.FromJson<FaceCoordinates>(json);
        }
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
