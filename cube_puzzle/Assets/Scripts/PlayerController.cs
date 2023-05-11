using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    private MonoBehaviourTimer movementTimer;
    private Vector3 origin;
    private Vector3 target;
    private Vector3 queuedTarget;
    // Start is called before the first frame update
    void Start()
    {
        movementTimer = gameObject.AddComponent<MonoBehaviourTimer>();
        movementTimer.duration = moveSpeed;
        origin = transform.position;
        target = Vector3.zero;
        queuedTarget = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (movementTimer.isRunning)
        {
            Vector3 movement = Vector3.Lerp(origin, target, movementTimer.currentTime / movementTimer.duration);
            transform.position = movement;
        }
        else if (movementTimer.isFinished)
        {
            movementTimer.StopTimer();
            transform.position = target;
            origin = transform.position;
            target = queuedTarget;
            queuedTarget = Vector3.zero;

            if (target != Vector3.zero)
            {
                movementTimer.StartTimer();
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {

        bool performed = callbackContext.action.WasPerformedThisFrame();
        bool pressed = callbackContext.action.WasPressedThisFrame();

        Vector2 input = callbackContext.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0, input.y);
        movement.x = movement.z != 0 ? 0 : movement.x;

        if (!performed && pressed)
        {
            if (target == Vector3.zero)
            {
                origin = transform.position;
                target = transform.position + movement;
                movementTimer.StartTimer();
            }
            else if (queuedTarget == Vector3.zero)
            {
                queuedTarget = target + movement;
            }
        }
    }
}
