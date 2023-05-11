using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLocker : MonoBehaviour
{
    [SerializeField]
    List<GameObject> lockerObjects = null;
    Vector3 originalPosition = Vector3.zero;
    GameObject currentObject = null;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void LockObject()
    {
        if (!currentObject)
            return;

        transform.parent = currentObject.transform;
        transform.localPosition = originalPosition;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (lockerObjects.Contains(other.gameObject))
        {
            currentObject = other.gameObject;
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if (currentObject == other.gameObject)
        {
            currentObject = null;
        }
    }
}
