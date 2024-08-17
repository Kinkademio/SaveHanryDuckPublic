
using System;

[System.Serializable]
public class Score : ICloneable
{
    //�������� ������� �������� ��� �����
    public string scoreNameForMenu;
    //���������� �������� ��� ��������� 
    public string scoreName;
    //�������� �������
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
