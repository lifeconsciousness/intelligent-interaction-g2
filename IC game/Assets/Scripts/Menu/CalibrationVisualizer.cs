using UnityEngine;
using UnityEngine.UI;

public class CalibrationVisualizer : MonoBehaviour
{
    public RectTransform dot; // The dot that moves
    public RectTransform box; // The box in which the dot moves

    private FaceTrackerReceiver faceTracker;

    void Start()
    {
        faceTracker = FaceTrackerReceiver.Instance;
        if (faceTracker == null)
        {
            Debug.LogError("FaceTrackerReceiver instance not found.");
        }
    }

    void Update()
    {
        if (faceTracker == null)
            return;

        // Get the current coordinates
        float x = faceTracker.coordinates.x;
        float y = faceTracker.coordinates.y;

        // Get min/max values (they are being updated during calibration)
        float minX = faceTracker.minMaxValues.minX;
        float maxX = faceTracker.minMaxValues.maxX;
        float minY = faceTracker.minMaxValues.minY;
        float maxY = faceTracker.minMaxValues.maxY;

        // Handle cases where min and max are equal to avoid division by zero
        if (Mathf.Approximately(maxX, minX) || Mathf.Approximately(maxY, minY))
            return;

        // Normalize x and y to range [-0.5, 0.5]
        float normalizedX = (x - (minX + maxX) / 2) / (maxX - minX);
        float normalizedY = (y - (minY + maxY) / 2) / (maxY - minY);

        // Ensure normalized values are within [-0.5, 0.5]
        normalizedX = Mathf.Clamp(normalizedX, -0.5f, 0.5f);
        normalizedY = Mathf.Clamp(normalizedY, -0.5f, 0.5f);

        // Calculate the movement ranges accounting for the dot's size
        float movementRangeX = box.rect.width - dot.rect.width;
        float movementRangeY = box.rect.height - dot.rect.height;

        // Calculate the anchored position of the dot
        Vector2 anchoredPosition = new Vector2(
            normalizedX * movementRangeX,
            normalizedY * movementRangeY
        );

        dot.anchoredPosition = anchoredPosition;
    }
}