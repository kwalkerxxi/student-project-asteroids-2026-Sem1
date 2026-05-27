using UnityEngine;

/// <summary>
/// This script allows the TrailRenderer to wrap around the screen better.
/// Instead of leaving behind the second postion which causes a poor screen artifact
/// it will clear it a temporarily stop emitting
/// </summary>
public class TrailRendererWrapper : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] trails;

    private int resumeFrame = -1;
    [SerializeField]
    private int frameBuffer = 2;

    void Awake()
    {
        // trail = GetComponent<TrailRenderer>();

        if (trails[0] == null)
        {
            Debug.LogError("TrailRenderer not found. Script Removed in play mode", gameObject);
            Destroy(this);
        }
    }

    /// <summary>
    /// This method temporarily resets the TrailRenderer and moves it to a new postion
    /// </summary>
    /// <param name="newPosition"></param>
    public void Warp(Vector3 newPosition)
    {
        foreach (TrailRenderer trail in trails)
        {
            trail.emitting = false;
            trail.Clear();

            transform.position = newPosition;

            resumeFrame = Time.frameCount + frameBuffer;
        }
    }

    /// <summary>
    /// This method temporarily resets the TrailRenderer
    /// This version is used in UnityEvents
    /// </summary>
    public void Warp()
    {
        foreach (TrailRenderer trail in trails)
        {
            trail.emitting = false;
            trail.Clear();
            resumeFrame = Time.frameCount + frameBuffer;
        }
    }

    public void CheckAndSetActivation()
    {
              
        if (resumeFrame != -1 && Time.frameCount >= resumeFrame)
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = true;
                resumeFrame = -1;
            }
        }
        
    }

    void Update()
    {
        CheckAndSetActivation();
    }
}
