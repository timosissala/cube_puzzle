using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHelper : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnAwake;

    [SerializeField]
    private UnityEvent OnEnabled;

    [SerializeField]
    private UnityEvent OnStart;

    [SerializeField]
    private UnityEvent OnDestroyed;

    private void Awake()
    {
        OnAwake?.Invoke();
    }

    private void OnEnable()
    {
        OnEnabled?.Invoke();
    }

    private void Start()
    {
        OnStart?.Invoke();
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
