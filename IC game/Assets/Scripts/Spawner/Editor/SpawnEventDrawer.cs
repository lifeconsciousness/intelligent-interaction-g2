using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnEvent))]
public class SpawnEventDrawer : PropertyDrawer
{
    // Constants for layout
    private const float fieldSpacing = 2f;
    private const float lineHeight = 18f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Start with default line count
        int lineCount = 7; // Added one more for 'visualize'

        // Get references to properties
        SerializedProperty useCoordinatesProp = property.FindPropertyRelative("useCoordinates");
        SerializedProperty useRandomPositionProp = property.FindPropertyRelative("useRandomPosition");

        // Add line for location field
        lineCount++; // Either spawnLocation or spawnCoordinates

        // Add line for areaSize if useRandomPosition is true
        if (useRandomPositionProp.boolValue)
        {
            lineCount++; // areaSize
        }

        return lineHeight * lineCount + fieldSpacing * (lineCount - 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin property drawing
        EditorGUI.BeginProperty(position, label, property);

        // Increase indent for nested fields
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = indent + 1;

        // Calculate rect for the first property
        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // Get references to properties
        SerializedProperty spawnTimeProp = property.FindPropertyRelative("spawnTime");
        SerializedProperty enemyPrefabProp = property.FindPropertyRelative("enemyPrefab");
        SerializedProperty useCoordinatesProp = property.FindPropertyRelative("useCoordinates");
        SerializedProperty spawnLocationProp = property.FindPropertyRelative("spawnLocation");
        SerializedProperty spawnCoordinatesProp = property.FindPropertyRelative("spawnCoordinates");
        SerializedProperty quantityProp = property.FindPropertyRelative("quantity");
        SerializedProperty useRandomPositionProp = property.FindPropertyRelative("useRandomPosition");
        SerializedProperty areaSizeProp = property.FindPropertyRelative("areaSize");
        SerializedProperty spawnIntervalProp = property.FindPropertyRelative("spawnInterval");
        SerializedProperty visualizeProp = property.FindPropertyRelative("visualize");
        SerializedProperty disableProp = property.FindPropertyRelative("disable");

        // Draw Disable
        EditorGUI.PropertyField(currentRect, disableProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Spawn Time
        EditorGUI.PropertyField(currentRect, spawnTimeProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Enemy Prefab
        EditorGUI.PropertyField(currentRect, enemyPrefabProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Use Coordinates
        EditorGUI.PropertyField(currentRect, useCoordinatesProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Spawn Location or Spawn Coordinates based on useCoordinates
        if (useCoordinatesProp.boolValue)
        {
            EditorGUI.PropertyField(currentRect, spawnCoordinatesProp, new GUIContent("Spawn Coordinates"));
        }
        else
        {
            EditorGUI.PropertyField(currentRect, spawnLocationProp, new GUIContent("Spawn Location"));
        }
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Quantity
        EditorGUI.PropertyField(currentRect, quantityProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Use Random Position
        EditorGUI.PropertyField(currentRect, useRandomPositionProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Area Size if Use Random Position is true
        if (useRandomPositionProp.boolValue)
        {
            EditorGUI.PropertyField(currentRect, areaSizeProp);
            currentRect.y += lineHeight + fieldSpacing;
        }

        // Draw Spawn Interval
        EditorGUI.PropertyField(currentRect, spawnIntervalProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Draw Visualize
        EditorGUI.PropertyField(currentRect, visualizeProp);
        currentRect.y += lineHeight + fieldSpacing;

        // Reset indent level
        EditorGUI.indentLevel = indent;

        // End property drawing
        EditorGUI.EndProperty();
    }
}
