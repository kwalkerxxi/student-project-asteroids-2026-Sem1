using UnityEngine;

public class AutoSeparator : MonoBehaviour
{
    public Rigidbody cachedRigidbody;
    public Collider cachedCollider;
    public LayerMask separationMask;
    public float maxSeparationDistance = 0.2f;

    void Awake()
    {
        if(cachedRigidbody == null)
        {
            cachedRigidbody = GetComponent<Rigidbody>();
        }

        if(cachedCollider == null)
        {
            cachedCollider = GetComponent<Collider>();
        }
    }

    //void FixedUpdate()
    //{
    //    SeparateFromOverlaps();
    //}

    public void RandomCheckAndSepartate()
    {
        Invoke(nameof(SeparateFromOverlaps), Random.Range(0.1f, 0.2f));
    }


    public void SeparateFromOverlaps()
    {
        // Broad-phase check
        Collider[] hits = Physics.OverlapBox(
            cachedCollider.bounds.center,
            cachedCollider.bounds.extents,
            transform.rotation,
            separationMask
        );

        foreach(var hit in hits)
        {
            if(hit == cachedCollider)
            {
                continue;
            }

            Vector3 direction;
            float distance;

            // Compute exact penetration
            bool overlapped = Physics.ComputePenetration(
                cachedCollider, transform.position, transform.rotation,
                hit, hit.transform.position, hit.transform.rotation,
                out direction, out distance
            );

            if(overlapped && distance > 0f)
            {
                // Clamp distance to avoid explosive separation
                float push = Mathf.Min(distance, maxSeparationDistance);

                // Move using physics-safe method
                cachedRigidbody.MovePosition(cachedRigidbody.position + direction * push);
            }
        }
    }
}
