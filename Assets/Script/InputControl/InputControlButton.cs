using UnityEngine;

[System.Serializable]
//Структура записи инпута
public class InputControlButton
{
    //Название клавиши управления понятное для пользователя
    public string inputForMenu;
    //Глобальное название для команды
    public string inputName;
    //Код клавиши
    public KeyCode buttonCode;
}
