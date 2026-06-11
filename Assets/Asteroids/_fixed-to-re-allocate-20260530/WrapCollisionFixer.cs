
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WrapCollisionFixer : MonoBehaviour
{
    [SerializeField] float timeScaleForChecking = 1f;
    public static List<Transform> LargeAsteroids = new List<Transform>();
    [SerializeField] private Color colorToUse = Color.rebeccaPurple;
    [SerializeField] private float radiusToCheck = 0.7f;

    [SerializeField] private Color normalColor = Color.magenta;
    [SerializeField] private Color overlapColor = Color.red;

    HashSet<Transform> overlapping = new();

    void Start()
    {
        Time.timeScale = timeScaleForChecking;
    }

    private void FixedUpdate()
    {
        overlapping.Clear();

        for(int i = 0; i < LargeAsteroids.Count; i++)
        {
            for(int j = i + 1; j < LargeAsteroids.Count; j++)
            {
                if(LargeAsteroids[i] == null || LargeAsteroids[j] == null)
                {
                    continue;
                }

                if(Vector3.Distance(
                        LargeAsteroids[i].position,
                        LargeAsteroids[j].position)
                    < radiusToCheck * 2f)
                {
                    overlapping.Add(LargeAsteroids[i]);
                    overlapping.Add(LargeAsteroids[j]);
                }
            }
        }

        foreach(var asteroid in overlapping)
        {
            if(overlapping.Contains(asteroid))
            {
                asteroid.AddComponent<TemporaryIFrame>();
                //asteroid.GetComponent<Rigidbody>().AddForce(asteroid.forward * 1f, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach(var asteroid in overlapping)
        {
            if(asteroid == null)
            {
                continue;
            }

            Gizmos.color = overlapping.Contains(asteroid)
                ? overlapColor
                : normalColor;

            Gizmos.DrawSphere(asteroid.position, radiusToCheck);
        }
    }
}
