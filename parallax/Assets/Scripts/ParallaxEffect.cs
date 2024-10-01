using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components
using UnityEngine.XR;

public class ParallaxEffect : MonoBehaviour
{
    public Camera mainCamera;      // Reference to the main camera
    public Camera parallaxCamera;  // Reference to the parallax camera
    public Slider parallaxSlider;  // Reference to the UI Slider
    public float parallaxFactor = 0.5f; // Initial value for the parallax effect

    private Vector3 initialMainCameraPos;
    private Vector3 initialParallaxCameraPos;

    void Start()
    {
        // Store the initial positions of both cameras
        initialMainCameraPos = mainCamera.transform.position;
        initialParallaxCameraPos = parallaxCamera.transform.position;

        if (parallaxSlider != null)
        {
            // Set the slider's initial value and add a listener for value changes
            parallaxSlider.value = parallaxFactor;
            parallaxSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Update()
    {
        // Update the parallax effect based on the main camera's movement
        UpdateParallaxEffect();

        // Check if the trigger button on the right hand controller is pressed
        CheckRightHandTrigger();
    }

    void UpdateParallaxEffect()
    {
        // Calculate the parallax effect position
        Vector3 delta = mainCamera.transform.position - initialMainCameraPos;
        parallaxCamera.transform.position = initialParallaxCameraPos + delta * parallaxFactor;
    }

    void CheckRightHandTrigger()
    {
        List<InputDevice> rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        if (rightHandDevices.Count == 1)
        {
            InputDevice device = rightHandDevices[0];
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
            {
                //Debug.Log("Trigger button is pressed.");
                ResetOffset();
            }
        }
        else if (rightHandDevices.Count > 1)
        {
            Debug.Log("Found more than one right hand device!");
        }
    }

    void ResetOffset()
    {
        // Reset the initial positions of both cameras to their current positions
        parallaxCamera.transform.position = initialParallaxCameraPos;
        initialMainCameraPos = mainCamera.transform.position;
        //Debug.Log("Offset has been reset.");
    }

    // Method to handle slider value changes
    public void OnSliderValueChanged(float value)
    {
        parallaxFactor = value;
        Debug.Log("Parallax factor updated to: " + parallaxFactor);
    }
}
