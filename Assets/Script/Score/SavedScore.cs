using System;

[System.Serializable]
public class SavedScore
{
    public string saveDate = DateTime.Now.ToString();
    public Score[] savedScore;
}
