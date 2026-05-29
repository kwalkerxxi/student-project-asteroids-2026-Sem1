using UnityEngine;
using UnityEditor;

public class HierarchyContextMenu : EditorWindow
{
    [MenuItem("GameObject/Utilities Custom/Create Empty Parent Holder", false, 0)]
    static void CreateEmptyParentHolder()
    {
        // Get the selected object in the hierarchy
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            // Create an empty GameObject with the name of the selected object + "Holder"
            GameObject parentHolder = new GameObject(selectedObject.name + " Holder");

            // Set the parent of the selected object to be the new empty parent holder
            Undo.RegisterCreatedObjectUndo(parentHolder, "Create Parent Holder");
            selectedObject.transform.SetParent(parentHolder.transform);

            // Optionally select the new parent object in the hierarchy
            Selection.activeGameObject = parentHolder;
        }
        else
        {
            Debug.LogWarning("No object selected in the hierarchy!");
        }
    }
}
