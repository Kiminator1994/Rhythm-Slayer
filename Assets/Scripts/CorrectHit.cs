using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectHit : MonoBehaviour
{
    [Range(0, 90)] public float toleranceAngle = 60f;
    public int coneSegments = 12;
    public bool correctHit = false;
    private Vector3 previousPosition;


    private void FixedUpdate()
    {
        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Actual hit direction (sword's movement)
        Vector3 actualHitDirection = (transform.position - previousPosition).normalized;

        // Expected direction (custom logic for your game)
        Vector3 expectedHitDirection = (transform.TransformDirection(Vector3.down) + transform.TransformDirection(Vector3.back)).normalized;

        // Validate swing direction
        float angle = Vector3.Angle(expectedHitDirection, actualHitDirection);

        //// Debug draw the directions
        Debug.DrawLine(transform.position, transform.position + expectedHitDirection, Color.green, 100f); // Ideal direction
        Debug.DrawLine(transform.position, transform.position + actualHitDirection, Color.red, 100f);
        DrawCone(transform.position, expectedHitDirection, toleranceAngle);


        if (angle <= toleranceAngle)
        {
            correctHit = true;
        }
        else
        {
            correctHit = false;
        }
    }

    public bool GetCorrectHit()
    {
        return correctHit;
    }

    private void DrawCone(Vector3 origin, Vector3 direction, float angle)
    {
        float radius = 2f; // Radius of the cone visualization
        Vector3 perpendicularVector = Vector3.Cross(direction, Vector3.up).normalized;

        for (int i = 0; i < coneSegments; i++)
        {
            float segmentAngle = (360f / coneSegments) * i;
            float radians = segmentAngle * Mathf.Deg2Rad;

            Quaternion rotation = Quaternion.AngleAxis(segmentAngle, direction);
            Vector3 rotatedVector = rotation * perpendicularVector;
            Vector3 pointOnCone = origin + direction * radius + rotatedVector * Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            Debug.DrawLine(origin, pointOnCone, Color.yellow, 100f);
        }
    }
}

/*
public class CorrectHit : MonoBehaviour
{
    [Range(0, 90)] public float toleranceAngle = 60f;
    public int coneSegments = 24;
    public bool correctHit = false;


    private void OnTriggerEnter(Collider other)
    {
        // Get the direction from the saber to this cube
        Vector3 actualHitDirection = (other.transform.position - transform.position).normalized;

        // the saber is likely to hit the cube from the lower edge in backward direction. means the expected hit direction is about 45° down to the lower egde of the playerside
        Vector3 expectedHitDirection = (transform.TransformDirection(Vector3.down) + transform.TransformDirection(Vector3.back)).normalized;
        float dotProduct = Vector3.Dot(expectedHitDirection, actualHitDirection);

        // calculate minimum tolerance. if dotProduct > tolerance, hit will be correct
        float toleranceDotProduct = Mathf.Cos(toleranceAngle * Mathf.Deg2Rad);


        //// Debug draw the directions
        //Debug.DrawLine(transform.position, transform.position + expectedHitDirection, Color.green, 100f); // Ideal direction
        //Debug.DrawLine(transform.position, transform.position + actualHitDirection, Color.red, 100f);
        //DrawCone(transform.position, expectedHitDirection, toleranceAngle);

        // Check if the hit is within the acceptable angle
        if (dotProduct >= toleranceDotProduct)
        {
            correctHit = true;
        }
    }

    public bool GetCorrectHit()
    {
        return correctHit;
    }

    private void DrawCone(Vector3 origin, Vector3 direction, float angle)
    {
        float radius = 2f; // Radius of the cone visualization
        Vector3 perpendicularVector = Vector3.Cross(direction, Vector3.up).normalized;

        for (int i = 0; i < coneSegments; i++)
        {
            float segmentAngle = (360f / coneSegments) * i;
            float radians = segmentAngle * Mathf.Deg2Rad;

            Quaternion rotation = Quaternion.AngleAxis(segmentAngle, direction);
            Vector3 rotatedVector = rotation * perpendicularVector;
            Vector3 pointOnCone = origin + direction * radius + rotatedVector * Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            Debug.DrawLine(origin, pointOnCone, Color.yellow, 100f);
        }
    }
}
*/