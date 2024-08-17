using UnityEngine;

public static class InputController
{
    private static InputSCRObj controls = Resources.Load<InputSCRObj>("Input/InputSettings");

    //��������� ���������� �� ����� � ����� ������ � ������ ��������
    private static InputControlButton checkInputName(string inputName)
    {
        InputControlButton found = controls.getByName(inputName);
        if (found == null)
        { //���� �� ����������, �� ���������� ������
            throw new System.Exception("������! ���������������� ���� � ����������� '" + inputName + "' �� ����������! ��������� ��������: " + controls);
        }
        return found;

    }

    public static void changeInput(string inputName, KeyCode newInputKey)
    {
       //��������� ���������� �� ����� � ����� ������ � ������
       checkInputName(inputName);
       controls.setNewButtonCode(inputName, newInputKey);
    }

    //��������� ���� �� ����� ������
    public static KeyCode getInput(string inputName)
    {
       //���� ����������, �� ��������� ���������� � ������������ � ���������� ��� �������
       return checkInputName(inputName).buttonCode;
    }

    //��������� ������ ���� ��������� ������� � ����
    public static InputControlButton[] getAllInputs()
    {
       return controls.getButtons();
    } 

}
