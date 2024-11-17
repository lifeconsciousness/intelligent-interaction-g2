using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private AsymFrustum asymFrustum;
    public Slider distanceSlider;
    public Button resetButton;
    private CameraController cameraController;
    private FaceTrackerReceiver faceTracker;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GetComponent<CameraController>();
        faceTracker = FaceTrackerReceiver.Instance;
        float cameraDistance = cameraController.cameraDistance;
        asymFrustum = FindObjectOfType<AsymFrustum>();

        if (cameraController == null)
        {
            Debug.LogError("Could not find CameraController.");
        }

        if (asymFrustum == null)
        {
            Debug.LogError("Could not find AsymFrustum.");
        }

        // Initialize the slider value and add a listener for value changes
        if (distanceSlider != null)
        {
            distanceSlider.value = -cameraDistance; // Set the slider to the current camera distance
            distanceSlider.onValueChanged.AddListener(OnSliderValueChanged);
            distanceSlider.minValue = -asymFrustum.virtualWindow.transform.position.z + FindObjectOfType<Camera>().transform.position.z + 0.3f;
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(faceTracker.ResetPosition);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        cameraController.cameraDistance = -value; // Update the camera distance based on slider value
    }
}
