using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Clock : MonoBehaviour
{
    private MonoBehaviourTimer timer;
    [SerializeField]
    private float interval = 0.5f;
    
    [SerializeField, Tooltip("How much will the interval value change after each tick")]
    private float intervalChange = 0.0f;

    [SerializeField]
    private bool startImmidiately = true;

    public UnityEvent OnStart;

    public UnityEvent OnTick;

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        if (timer == null)
        {
            timer = gameObject.AddComponent<MonoBehaviourTimer>();
            timer.duration = interval;

            if (startImmidiately)
            {
                StartClock();
            }
        }
    }

    public void StartClock()
    {
        if (timer == null)
        {
            Initialise();
        }

        timer.StartTimer();

        OnStart?.Invoke();
    }

    private void Update()
    {
        if (timer.isFinished)
        {
            Tick();
        }
    }

    private void Tick()
    {
        OnTick?.Invoke();

        interval += intervalChange;

        if (interval > 0.0f)
        {
            timer.duration = interval;
            timer.StartTimer();
        }
        else
        {
            StopClock();
        }
    }

    public void StopClock()
    {
        timer.StopTimer();
    }
}
