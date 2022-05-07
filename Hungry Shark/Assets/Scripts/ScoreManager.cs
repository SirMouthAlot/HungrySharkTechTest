using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    static int _currentScore = 0;
    
    public static int GetCurrentScore()
    {
        return _currentScore;
    }

    public static void AddToScore(int value)
    {
        _currentScore += value;
    }

    public static void RemoveFromScore(int value)
    {
        _currentScore -= value;
    }
}
