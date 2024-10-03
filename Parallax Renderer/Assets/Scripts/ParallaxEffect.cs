using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.XR;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class ParallaxEffect : MonoBehaviour
{
    private Camera parallaxCamera;  // Reference to the parallax camera
    public float parallaxFactor = 0.5f;

    private Vector3 initialMainCameraPos;
    private Vector3 initialParallaxCameraPos;
    private Vector3 lastPosition;
    private TcpListener listener;
    private bool isRunning = false;

    public int socketPort = 2000;

    void Start()
    {
        parallaxCamera = GetComponent<Camera>();

        if (parallaxCamera == null)
        {
            Debug.LogError("Parallax Camera not found!");
            return;
        }

        StartServer();
        initialMainCameraPos = new Vector3(0, 0, 0);
        initialParallaxCameraPos = parallaxCamera.transform.position;
    }

    private void StartServer()
    {
        listener = new TcpListener(IPAddress.Parse("127.0.0.1"), socketPort);
        listener.Start();
        isRunning = true;

        Thread serverThread = new Thread(ListenForClients);
        serverThread.Start();
    }


    private void ListenForClients()
    {
        while (isRunning)
        {
            TcpClient client = listener.AcceptTcpClient();
            // Pass the client to the handler method
            HandleClientComm(client);
        }
    }

    void Update()
    {
        // Update the parallax effect based on the main camera's movement
        UpdateParallaxEffect();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetOffset();
        }
    }

    void UpdateParallaxEffect()
    {
        // Calculate the parallax effect position
        Vector3 delta = lastPosition - initialMainCameraPos;
        parallaxCamera.transform.position = initialParallaxCameraPos + delta * parallaxFactor;
    }

    void ResetOffset()
    {
        // Reset the initial positions of both cameras to their current positions
        parallaxCamera.transform.position = initialParallaxCameraPos;
        initialMainCameraPos = lastPosition;
    }

    private void HandleClientComm(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received: " + receivedData);

                // Split the received data
                var values = receivedData.Split(',');
                if (values.Length == 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);

                    lastPosition = new Vector3(x, y, z);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        listener.Stop();
    }
}
