using UnityEngine;
using System;
using NetMQ;
using NetMQ.Sockets;
using System.Collections;
using UnityEngine.Assertions;

public class ParallaxEffect : MonoBehaviour
{
    private Camera parallaxCamera;  // Reference to the parallax camera
    public float parallaxFactor = 0.5f;

    private Vector3 initialMainCameraPos;
    private Vector3 initialParallaxCameraPos;
    private Vector3 lastPosition;
    private volatile bool isRunning = false;
    public int _port = 2000;

    void Start()
    {
        parallaxCamera = GetComponent<Camera>();

        if (parallaxCamera == null)
        {
            Debug.LogError("Parallax Camera not found!");
            return;
        }

        initialMainCameraPos = new Vector3(0, 0, 0);
        initialParallaxCameraPos = parallaxCamera.transform.position;

        AsyncIO.ForceDotNet.Force();
        NetMQConfig.Linger = new TimeSpan(0, 0, 1);

        _server = new PullSocket();
        _server.Options.Linger = new TimeSpan(0, 0, 1);
        _server.Bind($"tcp://*:{_port}");
        print($"server on {_port}");

        Assert.IsNotNull(_server);

        StartCoroutine(_CoWorker());
    }

    public PullSocket _server;
    void OnDisable()
    {
        _server?.Dispose();
        NetMQConfig.Cleanup(false);
    }

    IEnumerator _CoWorker()
    {
        while (true)
        {
            // Try receiving with a timeout or block until a message is available.
            if (_server.TryReceiveFrameString(out string recv))
            {
                //Debug.Log($"Received: {recv}");

                string[] pos = recv.Split(',');
                lastPosition = new Vector3(-float.Parse(pos[0]), -float.Parse(pos[1]), -float.Parse(pos[2]));
            }
            yield return null; // Yield every frame, effectively making this non-blocking.
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

    private void OnApplicationQuit()
    {
        isRunning = false;
    }
}
