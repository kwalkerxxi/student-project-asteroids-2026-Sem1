#if UNITY_EDITOR
using com.cyborgAssets.inspectorButtonPro;
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Splines;

[ExecuteAlways]
[RequireComponent(typeof(SplineContainer))]
public class SplineModifier : MonoBehaviour
{
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 positionScale = Vector3.one;

    [Tooltip("Apply on start in Editor (not at runtime)")]
    public bool applyOnStart = false;

    private SplineContainer splineContainer;

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && applyOnStart)
        {
            ApplyOffsetAndScale();
        }
#endif
    }

    [ContextMenu("Apply Offset and Scale")]
    [ProButton]
    public void ApplyOffsetAndScale()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer not found.");
            return;
        }

        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            BezierKnot knot = splineContainer.Spline[i];

            Vector3 newPos = Vector3.Scale(knot.Position, positionScale) + positionOffset;
            knot.Position = newPos;

            splineContainer.Spline.SetKnot(i, knot);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(splineContainer);
#endif

        Debug.Log("Spline modified with offset and scale.");
    }
}
