using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTranslator : MonoBehaviour
{
    [Tooltip("If null, will use the parent object")]
    [SerializeField]
    private Transform translatedObject;

    [Tooltip("What state should the object be initialised. Use -1 for no initial state")]
    [SerializeField]
    private int initialStateID = -1;

    [Tooltip("If no transition times are specified, translations will use this instead")]
    [SerializeField]
    private float defaultTransitionTime = 0.5f;

    [SerializeField]
    private bool ignoreX = false;

    [SerializeField]
    private bool ignoreY = false;

    [SerializeField]
    private bool ignoreZ = false;

    [Tooltip("Translation states in euler angles")]
    [SerializeField]
    private List<Vector3> translationStates = new List<Vector3>();
    private Vector3 initialPosition;

    [Tooltip("Transition times for their respective translation state")]
    [SerializeField]
    private List<float> transitionTimes = new List<float>();

    private int currentTranslationStateID;

    private void Awake()
    {
        if (translatedObject == null)
        {
            translatedObject = transform;
        }

        initialPosition = translatedObject.localPosition;

        if (translationStates != null && translationStates.Count > 0 && initialStateID >= 0 && initialStateID < translationStates.Count)
        {
            translatedObject.localPosition = translationStates[initialStateID];
        }
    }

    public void StartTranslationToNextState()
    {
        int nextState = currentTranslationStateID + 1 == translationStates.Count ? 0 : currentTranslationStateID + 1;

        StartTranslationToState(nextState);
    }

    //<summary> Translate to specified state id, -1 for initial state </summary>
    public void StartTranslationToState(int stateID)
    {
        float transitionTime = transitionTimes.Count > 0 && stateID >= 0 && stateID < transitionTimes.Count ? transitionTimes[stateID] : defaultTransitionTime;

        currentTranslationStateID = stateID;

        Vector3 translationState = stateID >= 0 ? translationStates[stateID] : initialPosition;
        StartTranslation(translatedObject, translationState, transitionTime);
    }

    public void StartTranslation(Transform translatedTransform, Vector3 targetPosition, float transitionTime)
    {
        StartCoroutine(TranslateObject(translatedTransform, targetPosition.x, targetPosition.y, targetPosition.z, transitionTime));
    }

    private IEnumerator TranslateObject(Transform trans, float xAxis, float yAxis, float zAxis, float transitionTime)
    {
        xAxis = ignoreX ? initialPosition.x : xAxis;
        yAxis = ignoreY ? initialPosition.y : yAxis;
        zAxis = ignoreZ ? initialPosition.z : zAxis;

        Vector3 targetPos = new Vector3(xAxis, yAxis, zAxis);

        Vector3 currentPos = trans.localPosition;
        float t = 0;

        while (t < transitionTime)
        {
            Vector3 position = Vector3.Lerp(currentPos, targetPos, t / transitionTime);
            trans.localPosition = position;

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        trans.localPosition = targetPos;
    }
}
