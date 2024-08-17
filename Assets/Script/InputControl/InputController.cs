using UnityEngine;

public static class InputController
{
    private static InputSCRObj controls = Resources.Load<InputSCRObj>("Input/InputSettings");

    //Проверяем существует ли инпут с таким именем в списке настроек
    private static InputControlButton checkInputName(string inputName)
    {
        InputControlButton found = controls.getByName(inputName);
        if (found == null)
        { //Если не существует, то возвращаем ошибку
            throw new System.Exception("Ошибка! Пользовательский ввод с псевдонимом '" + inputName + "' не установлен! Выполните проверку: " + controls);
        }
        return found;

    }

    public static void changeInput(string inputName, KeyCode newInputKey)
    {
       //Проверяем существует ли инпут с таким именем в списке
       checkInputName(inputName);
       controls.setNewButtonCode(inputName, newInputKey);
    }

    //Получение кода по имени инпута
    public static KeyCode getInput(string inputName)
    {
       //Если существует, то провереям сохранения у пользователя и возвращаем код клавиши
       return checkInputName(inputName).buttonCode;
    }

    //Получение списка всех возможных инпутов в игре
    public static InputControlButton[] getAllInputs()
    {
       return controls.getButtons();
    } 

}
