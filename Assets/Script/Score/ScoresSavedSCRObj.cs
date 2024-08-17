using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SavedScores", menuName = "ScriptableObjects/SavedScores", order = 1)]
public class ScoresSavedSCRObj : ScriptableObject
{
    [SerializeField] SavedScore[] savedScores;

    public void saveScore(Score[] scores)
    {
        SavedScore newSave = new SavedScore();
        newSave.savedScore = scores;
        savedScores = savedScores.Append(newSave).ToArray();
    }

    public void clearAllSavedScores()
    {
        savedScores = new SavedScore[0];
    }

    public SavedScore[] getAllSavedScores()
    {
        SavedScore[] copyOfSaves = new SavedScore[savedScores.Length];
        savedScores.CopyTo(copyOfSaves, 0);
        return copyOfSaves;
    }
}
