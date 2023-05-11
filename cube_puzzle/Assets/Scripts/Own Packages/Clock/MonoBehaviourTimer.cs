using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourTimer : MonoBehaviour
{
    public float currentTime { get; protected set; }
    public float duration;

    public bool StartOnAwake = false;

    public bool isRunning { get; protected set; }
    public bool isFinished { get; protected set; }

    public event Action OnTimerFinishedAction;
    public UnityEvent OnTimerFinished;

    private void Awake()
    {
        if (StartOnAwake)
        {
            StartTimer();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        isFinished = false;
        currentTime = 0.0f;
    }

    public void StopTimer()
    {
        isRunning = false;
        isFinished = false;
        currentTime = 0.0f;
    }

    private void Update()
    {
        if (isRunning)
        {
            if (currentTime < duration)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                isFinished = true;
                isRunning = false;
                OnTimerFinishedAction?.Invoke();
                OnTimerFinished?.Invoke();
            }
        }
    }
}
