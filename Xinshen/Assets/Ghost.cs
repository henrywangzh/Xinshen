using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float range = 5f;  // The radius of the spherical range
    public float speed = 2f;  // The speed at which the object moves
    public float smoothTime = 1f;  // The smooth time for dampening

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 velocity;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        SetRandomTargetPosition();

        // Move towards the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
    }

    void SetRandomTargetPosition()
    {
        // Generate a random direction within the unit sphere
        Vector3 randomDirection = Random.insideUnitSphere;

        // Calculate the target position based on the random direction and range
        targetPosition = startPosition + randomDirection * range;
    }
}