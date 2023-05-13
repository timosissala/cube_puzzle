using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    [SerializeField]
    public float moveSpeed;

    private MonoBehaviourTimer movementTimer;
    private Vector3 origin;
    private Vector3 target;
    private Vector3 queuedTarget;

    private Vector3 movementVector;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        movementTimer = gameObject.AddComponent<MonoBehaviourTimer>();
        movementTimer.duration = levelData.levelTransitionDuration;

        origin = transform.position;
        target = Vector3.zero;
        queuedTarget = Vector3.zero;

        rb = GetComponent<Rigidbody>();
        levelData.OnExitReached += OnExitReached;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (target != Vector3.zero)
        {
            MoveToTarget();
        }
        else if (movementVector != Vector3.zero)
        {
            transform.Translate(movementVector * moveSpeed / 1000);
        }
    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {

        bool performed = callbackContext.action.WasPerformedThisFrame();
        bool pressed = callbackContext.action.WasPressedThisFrame();

        Vector2 input = callbackContext.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0, input.y);

        if (performed)
        {
            movementVector = movement;
        }
        else if (!performed && !pressed)
        {
            movementVector = Vector3.zero;
        }
    }

    private void OnExitReached()
    {
        origin = transform.position;
        target = levelData.GetCurrentStage().spawn;
        movementTimer.StartTimer();
    }

    private void MoveToTarget()
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

    public void OnMovementGridBased(InputAction.CallbackContext callbackContext)
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
