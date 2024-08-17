using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public static class ScoreController 
{
    private static ScoreSCRObj scoresTypes = Resources.Load<ScoreSCRObj>("Score/ScoresSettings");
    private static ScoresSavedSCRObj savedScores = Resources.Load<ScoresSavedSCRObj>("Score/SavedScores");
    private static Score[] currentScores = null;

    public static void createNewScores()
    { 
        currentScores = scoresTypes.getScores();
    }

    public static SavedScore[] getAllSavedScosres()
    {
        return savedScores.getAllSavedScores();
    }
    public static void saveCurrentScore()
    {
        checkCurrentScore();
        savedScores.saveScore(currentScores);
    }

    public static Score getCurrentScoreByName(string name)
    {
        checkCurrentScore();
        Score found = currentScores.FirstOrDefault(obj => obj.scoreName == name);
        if (found == null)
        { //���� �� ����������, �� ���������� ������
            throw new Exception("������! ������ � ����������� '" + name + "' �� ����������! ��������� ��������: " + scoresTypes);
        }
        return found;
    }

    public static void setCurrentScoreNewValue(string name, string newCode)
    {
        getCurrentScoreByName(name).scoreValue = newCode;
    }

    private static void checkCurrentScore()
    {
        if (currentScores == null)
        {
            if(scoresTypes == null)
            {
                throw new Exception("������ �������� 'Score/ScoresSettings' �� ������ ��� �� ������������������!");
            }
            else
            {
                throw new Exception("������ ������� �������� �� ��� ������! �������������� ��������: ScoreController.createNewScores().");
            }  
        }
    }

    public static void clearAllSaves()
    {
        savedScores.clearAllSavedScores();
    }
}
