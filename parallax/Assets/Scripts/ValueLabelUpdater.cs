using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUpdater : MonoBehaviour
{
    private Slider parentSlider;
    private TMP_Text text;

    void Start()
    {
        // Get the Text component on this GameObject
        text = GetComponent<TMP_Text>();

        // Get the Slider component from the parent
        parentSlider = GetComponentInParent<Slider>();

        if (parentSlider != null)
        {
            // Subscribe to the Slider's value changed event
            parentSlider.onValueChanged.AddListener(UpdateText);

            // Initial update of the text based on the starting value of the slider
            UpdateText(parentSlider.value);
        }
        else
        {
            Debug.LogError("No Slider component found in parent.");
        }
    }

    void UpdateText(float value)
    {
        // Update the Text component based on the slider's value
        text.text = value.ToString("0.00");
    }

    void OnDestroy()
    {
        if (parentSlider != null)
        {
            // Unsubscribe from the event to avoid memory leaks when this object is destroyed
            parentSlider.onValueChanged.RemoveListener(UpdateText);
        }
    }
}