using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    
    private GameObject[] objectPool;

    [SerializeField]
    private bool poolOnAwake = false;

    [SerializeField]
    private int poolAmount = 100;

    [SerializeField]
    private GameObject objParent;

    private void Awake()
    {
        if (poolOnAwake)
        {
            PoolObjects();
        }
    }

    public void PoolObjects()
    {
        objectPool = new GameObject[poolAmount];

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject parent = objParent == null ? gameObject : objParent;

            objectPool[i] = Instantiate(obj, parent.transform);
            objectPool[i].SetActive(false);
        }
    }

    public void GetPoolFromChildren()
    {
        objectPool = new GameObject[objParent.transform.childCount];

        for (int i = 0; i < objParent.transform.childCount; i++)
        {
            objectPool[i] = objParent.transform.GetChild(i).gameObject;
        }

        DeActivateAll();
    }

    public GameObject GetInactiveObject(bool activate)
    {
        GameObject result = null;

        for (int i = 0; i < objectPool.Length; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                result = objectPool[i];

                if (activate)
                {
                    result.SetActive(true);
                }
                break;
            }
        }

        return result;
    }

    public GameObject GetActiveObject()
    {
        GameObject result = null;

        for (int i = 0; i < objectPool.Length; i++)
        {
            if (objectPool[i].activeInHierarchy)
            {
                result = objectPool[i];

                break;
            }
        }

        return result;
    }

    public void DeActivateAll()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            if (objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(false);
            }
        }
    }

    public void DestroyAll()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            DestroyImmediate(objectPool[i]);
        }
    }
}