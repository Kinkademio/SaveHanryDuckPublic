using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObjects/Scores", order = 1)]
public class ScoreSCRObj : ScriptableObject
{
    [SerializeField] Score[] scores;

    public Score getByName(string name)
    {
        return scores.FirstOrDefault(obj => obj.scoreName == name);
    }

    public Score[] getScores()
    {
        Score[] scoresClone = new Score[scores.Length];
        for(int i=0; i<scores.Length; i++)
        {
            scoresClone[i] = (Score)scores[i].Clone();
        }

        return scoresClone;
    }

    public void setNewScoreValue(string name, string newCode)
    {
        getByName(name).scoreValue = newCode;
    }

}
