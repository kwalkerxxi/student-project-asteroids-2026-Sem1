using UnityEditor;
using UnityEngine;

public static class CreateEmptyAtOriginEditor
{
    [MenuItem("GameObject/Create Empty at Origin", false, 0)]
    private static void CreateEmptyGameObjectAtOrigin()
    {
        //string input = EditorInputDialog.Show("Enter Empty GameObject Name", "Please enter the name", "");

        GameObject go = new GameObject("GameObject");
        //if(input != null || input == "")
        //{
        //    go.name = input;
        //}
        go.transform.position = Vector3.zero;

        // If there's a selected transform, parent under it
        if(Selection.activeTransform != null)
        {
            go.transform.SetParent(Selection.activeTransform);
        }

        // Register the creation in undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Empty at Origin");
        Selection.activeGameObject = go;

        //Selection.activeGameObject = null;
    }
}
