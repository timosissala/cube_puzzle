using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels;

    [SerializeField]
    public int currentLevel;
    public Level GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    [SerializeField]
    public int currentStage;
    public Stage GetCurrentStage()
    {
        return GetCurrentLevel().stages[currentStage];
    }

    public float levelTransitionDuration = 5.0f;

    public event Action OnExitReached;

    public void StartLevel(int level)
    {
        currentLevel = level;
        currentStage = 0;
    }

    public void NextLevel()
    {
        currentLevel++;
        currentStage = 0;
    }

    public void NextStage()
    {
        currentStage++;
    }

    public void ExitReached()
    {
        NextStage();
        OnExitReached.Invoke();
    }
}
