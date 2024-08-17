
using System;

[System.Serializable]
public class Score : ICloneable
{
    //Название рекорда понятное для юзера
    public string scoreNameForMenu;
    //Глобальное название для программы 
    public string scoreName;
    //Значение рекорда
    public string scoreValue = "0";

    public Object Clone()
    {
        Score clone = new Score();
        clone.scoreNameForMenu = scoreNameForMenu;
        clone.scoreName = scoreName;
        clone.scoreValue = scoreValue;  
        return clone;
    }


}
