using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    [SerializeField]
    private GameObject cube;
    private ObjectRotator cubeRotator;
    private Vector3 oddRotation = new Vector3(-1, 0, 0);
    private Vector3 evenRotation = new Vector3(0, 0, 1);

    [SerializeField]
    private GameObject exitPrefab;
    private GameObject exit;
    
    void Awake()
    {
        levelData.StartLevel(0);
        levelData.OnExitReached += OnExitReached;

        cubeRotator = cube.GetComponent<ObjectRotator>();

        exit = Instantiate(exitPrefab, levelData.GetCurrentStage().exit, Quaternion.identity);
    }

    void Update()
    {
        
    }

    public void OnExitReached()
    {
        cubeRotator.StartRotationToNextState();
    }
}
