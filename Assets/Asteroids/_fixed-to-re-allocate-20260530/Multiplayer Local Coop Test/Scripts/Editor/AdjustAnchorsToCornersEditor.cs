using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdjustAnchorsToCorners))]
public class AdjustAnchorsToCornersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Display the default inspector
        DrawDefaultInspector();

        AdjustAnchorsToCorners myScript = (AdjustAnchorsToCorners)target;

        // Add a button to adjust the anchors
        if (GUILayout.Button("Adjust Anchors to Corners"))
        {
            // Call the AdjustAnchors method
            myScript.AdjustAnchors();
        }
    }
}
