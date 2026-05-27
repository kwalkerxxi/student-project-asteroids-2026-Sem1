using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the attached object toward a specified target position.
/// </summary>
public class MoveRandomly : MonoBehaviour
{
    private Vector3 targetPosition;
    public float moveSpeed = 3f;

    public void Initialize(Vector3 centerPosition)
    {
        targetPosition = GetRandomPoint();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            //Destroy(gameObject);

            targetPosition = GetRandomPoint();
        }
    }


    Vector3 GetRandomPoint()
    {
        float x = Random.Range(0f, Screen.width);
        float y = Random.Range(0f, Screen.height);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if(plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}