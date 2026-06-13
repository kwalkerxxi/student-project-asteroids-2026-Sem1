using UnityEngine;

public static class TransformUtils
{
    public static void DeleteChildren(Transform parent, bool deleteParentToo = false)
    {
        if(parent == null)
        {
            return;
        }

        // Delete all children
        for(int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                Object.DestroyImmediate(child.gameObject);
            }
            else
#endif
                Object.Destroy(child.gameObject);
        }

        // Optionally delete parent
        if(deleteParentToo)
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                Object.DestroyImmediate(parent.gameObject);
            }
            else
#endif
                Object.Destroy(parent.gameObject);
        }
    }
}