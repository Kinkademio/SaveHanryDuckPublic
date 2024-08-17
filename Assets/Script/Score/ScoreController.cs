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
        { //Если не существует, то возвращаем ошибку
            throw new Exception("Ошибка! Рекорд с псевдонимом '" + name + "' не установлен! Выполните проверку: " + scoresTypes);
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
                throw new Exception("Шаблон рекордов 'Score/ScoresSettings' не найден или не проинициализирован!");
            }
            else
            {
                throw new Exception("Массив текущих рекордов не был создан! Воспользуйтесь командой: ScoreController.createNewScores().");
            }  
        }
    }

    public static void clearAllSaves()
    {
        savedScores.clearAllSavedScores();
    }
}
