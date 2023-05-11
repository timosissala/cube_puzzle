using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rotator : MonoBehaviour
{
    Vector3 origin;
    Vector3 target;
    bool rotating;
    private MonoBehaviourTimer timer;

    private void Start()
    {
        target = transform.rotation.eulerAngles;
        timer = gameObject.AddComponent<MonoBehaviourTimer>();

        RotateDegrees(new Vector3(1, 0, 0), 90, 5.0f);
    }

    private void Update()
    {
        if (rotating)
        {
            timer.StartTimer();

            Vector3 rotation = Vector3.Lerp(origin, target, 1.0f / timer.duration);
            transform.rotation.eulerAngles.Set(rotation.x, rotation.y, rotation.z);

            if (timer.isFinished)
            {
                timer.StopTimer();
                rotating = false;
            }
        }
    }

    public void RotateDegrees(Vector3 direction, int degrees, float duration)
    {
        rotating = true;

        Vector3 rotation = direction * degrees;

        origin = transform.rotation.eulerAngles;
        target = target + rotation;
        timer.duration = duration;
    }
}
