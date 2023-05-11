using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorChanger : MonoBehaviour
{
    [Tooltip("If null, will use the parent object")]
    [SerializeField]
    private Material coloredMaterial;
    private new Renderer renderer;

    [Tooltip("What state should the object be initialised. Use -1 for no initial state")]
    [SerializeField]
    private int initialStateID = -1;

    [Tooltip("If no transition times are specified, translations will use this instead")]
    [SerializeField]
    private float defaultTransitionTime = 0.5f;

    [SerializeField]
    private bool ignoreR = false;

    [SerializeField]
    private bool ignoreG = false;

    [SerializeField]
    private bool ignoreB = false;

    [SerializeField]
    private bool ignoreA = false;

    [Tooltip("Translation states in euler angles")]
    [SerializeField]
    private List<Color32> colorStates = null;
    private Color32 initialColor;

    [Tooltip("Transition times for their respective translation state")]
    [SerializeField]
    private List<float> transitionTimes = new List<float>();

    private int currentTranslationStateID;

    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();

        if (coloredMaterial == null)
        {
            coloredMaterial = renderer.material;
        }

        initialColor = coloredMaterial.color;

        if (colorStates != null && colorStates.Count > 0 && initialStateID >= 0 && initialStateID < colorStates.Count)
        {
            coloredMaterial.color = colorStates[initialStateID];
        }
    }

    public void StartChangingToNextColorState()
    {
        int nextState = currentTranslationStateID + 1 == colorStates.Count ? 0 : currentTranslationStateID + 1;

        StartChangingColor(nextState);
    }

    //<summary> Translate to specified state id, -1 for initial state </summary>
    public void StartChangingColor(int stateID)
    {
        float transitionTime = transitionTimes.Count > 0 && stateID >= 0 && stateID < transitionTimes.Count ? transitionTimes[stateID] : defaultTransitionTime;

        currentTranslationStateID = stateID;

        Color32 colorState = stateID >= 0 ? colorStates[stateID] : initialColor;
        StartColorChange(coloredMaterial, colorState, transitionTime);
    }

    public void StartColorChange(Material changedMaterial, Color32 targetColor, float transitionTime)
    {
        StartCoroutine(TranslateObject(changedMaterial, targetColor.r, targetColor.g, targetColor.b, targetColor.a, transitionTime));
    }

    private IEnumerator TranslateObject(Material mat, byte r, byte g, byte b, byte a, float transitionTime)
    {
        r = ignoreR ? initialColor.r : r;
        g = ignoreG ? initialColor.g : g;
        b = ignoreB ? initialColor.b : b;
        a = ignoreA ? initialColor.a : a;

        Color32 targetColor = new Color32(r, g, b, a);

        Color32 currentColor = mat.color;
        float t = 0;

        while (t < transitionTime)
        {
            Color32 color = Color32.Lerp(currentColor, targetColor, t / transitionTime);
            mat.color = color;

            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        mat.color = targetColor;
    }
}
