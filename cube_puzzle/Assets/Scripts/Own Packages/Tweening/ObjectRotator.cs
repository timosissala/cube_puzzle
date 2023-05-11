using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRotator : MonoBehaviour
{
    [Tooltip("If null, will use the parent object")]
    [SerializeField]
    private Transform rotatedObject;

    [Tooltip("What state should the object be initialised. Use -1 for no initial state")]
    [SerializeField]
    private int initialStateID = -1;

    [Tooltip("If no transition times are specified, rotations will use this instead")]
    [SerializeField]
    private float defaultTransitionTime = 0.5f;

    [Tooltip("Rotation states in euler angles")]
    [SerializeField]
    private List<Vector3> rotationStates = new List<Vector3>();

    [Tooltip("Rotation times for their respective rotation state")]
    [SerializeField]
    private List<float> transitionTimes = new List<float>();

    private int currentRotationStateID;

    private void Awake()
    {
        if (rotatedObject == null)
        {
            rotatedObject = transform;
        }

        if (rotationStates != null && rotationStates.Count > 0 && initialStateID >= 0 && initialStateID < rotationStates.Count)
        {
            rotatedObject.localRotation = Quaternion.Euler(rotationStates[initialStateID]);
        }
    }

    public void StartRotationToNextState()
    {
        int nextState = currentRotationStateID + 1 == rotationStates.Count ? 0 : currentRotationStateID + 1;

        StartRotationToState(nextState);
    }

    public void StartRotationToState(int stateID)
    {
        float rotationTime = transitionTimes.Count > 0 && stateID >= 0 && stateID < transitionTimes.Count ? transitionTimes[stateID] : defaultTransitionTime;

        currentRotationStateID = stateID;

        StartRotation(rotatedObject, rotationStates[stateID], rotationTime);
    }

    public void StartRotation(Transform rotatedTransform, Vector3 targetRotation, float rotationTime)
    {
        StartCoroutine(RotateObject(rotatedTransform, targetRotation.x, targetRotation.y, targetRotation.z, rotationTime));
    }

    private IEnumerator RotateObject(Transform trans, float xAxis, float yAxis, float zAxis, float rotationTime)
    {
        xAxis = TargetRotAdjuster(xAxis);
        yAxis = TargetRotAdjuster(yAxis);
        zAxis = TargetRotAdjuster(zAxis);

        Vector3 targetRot = new Vector3(xAxis, yAxis, zAxis);

        Vector3 currentRot = trans.localEulerAngles;
        float t = 0;

        while (t < rotationTime)
        {
            Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(currentRot), Quaternion.Euler(targetRot), t / rotationTime);
            trans.localRotation = rotation;

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        trans.localRotation = Quaternion.Euler(targetRot);
    }

    private float TargetRotAdjuster(float axis)
    {
        if (axis == 360)
        {
            axis = 0;
        }
        else if (axis > 180)
        {
            axis -= 360;
        }

        return axis;
    }
}
