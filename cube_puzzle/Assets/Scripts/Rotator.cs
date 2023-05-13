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

        //RotateDegrees(new Vector3(1, 0, 0), 90, 10.0f);
    }

    private void Update()
    {
        if (rotating)
        {
            Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(origin), Quaternion.Euler(target), timer.currentTime / timer.duration);
            transform.localRotation = rotation;

            if (timer.isFinished)
            {
                timer.StopTimer();
                rotating = false;
                transform.localRotation = Quaternion.Euler(target);
            }
        }
    }

    public void RotateDegrees(Vector3 direction, int degrees, float duration)
    {
        rotating = true;

        Vector3 rotation = direction * degrees;

        origin = transform.rotation.eulerAngles;
        target = origin + rotation;
        timer.duration = duration;

        timer.StartTimer();
    }
}
