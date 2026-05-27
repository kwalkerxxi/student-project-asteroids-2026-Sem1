using UnityEditor;
using UnityEngine;

public class RemoveNonMeshComponents : EditorWindow
{
    private static Object[] selectedObjects;
    private static bool keepParticleSystems = false;

    [MenuItem("Tools/Remove Non-Mesh Components")]
    public static void ShowWindow()
    {
        selectedObjects = Selection.objects;

        if(selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select at least one GameObject.", "OK");
            return;
        }

        string message = "You are about to remove ALL components except:\n" +
                         "- Transform\n" +
                         "- MeshFilter\n" +
                         "- MeshRenderer\n\n" +
                         "Selected objects:\n";

        foreach(Object selected in selectedObjects)
        {
            message += "- " + selected.name + "\n";
        }

        bool confirm = EditorUtility.DisplayDialog(
            "Confirm Component Removal",
            message,
            "Continue",
            "Cancel"
        );

        if(!confirm)
        {
            return;
        }

        // Ask whether to keep ParticleSystems
        keepParticleSystems = EditorUtility.DisplayDialog(
            "Particle Systems",
            "Do you want to KEEP ParticleSystem and ParticleSystemRenderer components?",
            "Keep Particle Systems",
            "Remove Particle Systems"
        );

        ProcessSelection();
    }

    private static void ProcessSelection()
    {
        foreach(Object selected in selectedObjects)
        {
            if(selected is GameObject selectedGameObject)
            {
                if(PrefabUtility.IsPartOfAnyPrefab(selectedGameObject))
                {
                    PrefabUtility.UnpackPrefabInstance(
                        selectedGameObject,
                        PrefabUnpackMode.Completely,
                        InteractionMode.AutomatedAction
                    );
                }

                ProcessGameObjectRecursive(selectedGameObject);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void ProcessGameObjectRecursive(GameObject gameObject)
    {
        RemoveComponents(gameObject);

        foreach(Transform childTransform in gameObject.transform)
        {
            ProcessGameObjectRecursive(childTransform.gameObject);
        }
    }

    private static void RemoveComponents(GameObject gameObject)
    {
        Component[] allComponents = gameObject.GetComponents<Component>();

        // First pass: remove all MonoBehaviours
        foreach(Component component in allComponents)
        {
            if(component is MonoBehaviour)
            {
                Undo.DestroyObjectImmediate(component);
            }
        }

        // Refresh list after script removal
        allComponents = gameObject.GetComponents<Component>();

        // Second pass: remove everything except allowed components
        foreach(Component component in allComponents)
        {
            if(component is Transform)
            {
                continue;
            }

            if(component is MeshFilter)
            {
                continue;
            }

            if(component is MeshRenderer)
            {
                continue;
            }

            if(keepParticleSystems)
            {
                if(component is ParticleSystem)
                {
                    continue;
                }

                if(component is ParticleSystemRenderer)
                {
                    continue;
                }
            }

            Undo.DestroyObjectImmediate(component);
        }
    }
}
