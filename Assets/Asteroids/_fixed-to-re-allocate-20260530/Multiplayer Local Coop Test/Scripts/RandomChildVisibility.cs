using UnityEngine;

public class RandomChildVisibility : MonoBehaviour
{
    // Serialized fields for inspector assignment
    [SerializeField] private GameObject parentObject;
    [SerializeField] private string childName;

    void Start()
    {
        // Search the entire hierarchy of the parentObject for the child by name
        Transform childTransform = FindChildRecursive(parentObject.transform, childName);


        if (childTransform != null)
        {
            // Get all the child objects under this found child
            GameObject[] childObjects = childTransform.GetComponentsInChildren<GameObject>();

            // Filter out the parent object itself
            var filteredChildObjects = new System.Collections.Generic.List<GameObject>();
            foreach (var obj in childObjects)
            {
                if (obj != parentObject) filteredChildObjects.Add(obj);
            }

            // Randomly keep one of them active and deactivate the others
            if (filteredChildObjects.Count > 0)
            {
                // Turn off all children first
                foreach (GameObject child in filteredChildObjects)
                {
                    child.SetActive(false);
                }

                // Randomly choose one child to remain active
                int randomIndex = Random.Range(0, filteredChildObjects.Count);
                filteredChildObjects[randomIndex].SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Child object with the name " + childName + " not found.");
        }
    }

    // Recursive method to search the hierarchy for a child by name
    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            Transform found = FindChildRecursive(child, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
}
