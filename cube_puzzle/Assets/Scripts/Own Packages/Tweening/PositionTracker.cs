using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float trackingAmount = 1.0f;

    [SerializeField]
    private GameObject trackedObject;

    [SerializeField]
    public bool enableTracking = true;

    [SerializeField, Header("Tracked Axes")]
    private bool x = true;
    [SerializeField]
    private bool y = true;
    [SerializeField]
    private bool z = true;

    [SerializeField]
    private float maxMoveDistancePerFrame = 0.05f;

    private Vector3 originalPosition;
    private Vector3 previousPosition;

    private Vector3 targetOriginalPosition;
    private Vector3 targetPreviousPosition;

    private void Start()
    {
        originalPosition = transform.position;
        targetOriginalPosition = trackedObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (enableTracking)
        {
            TrackPosition();
        }
    }

    private void TrackPosition()
    {
        Vector3 targetPosition = trackedObject.transform.position;

        if (targetPosition != targetPreviousPosition)
        {
            Vector3 targetDifference = targetPosition - targetOriginalPosition;
            Vector3 scaledTargetDifference = targetDifference * trackingAmount;

            Vector3 position = originalPosition + scaledTargetDifference;

            position.x = x ? ClampPosition(position.x, previousPosition.x) : originalPosition.x;
            position.y = y ? ClampPosition(position.y, previousPosition.y) : originalPosition.y;
            position.z = z ? ClampPosition(position.z, previousPosition.z) : originalPosition.z;

            transform.position = position;

            previousPosition = position;

            targetPreviousPosition = targetPosition;
        }
    }

    private float ClampPosition(float coord, float prevCoord)
    {
        float clampedPosition = Mathf.Clamp(coord - prevCoord, -maxMoveDistancePerFrame, maxMoveDistancePerFrame) + prevCoord;

        return clampedPosition;
    }
}
