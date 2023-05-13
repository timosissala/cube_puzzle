using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    private void OnTriggerEnter(Collider other)
    {
        levelData.ExitReached();
        transform.position = levelData.GetCurrentStage().exit;
    }
}
