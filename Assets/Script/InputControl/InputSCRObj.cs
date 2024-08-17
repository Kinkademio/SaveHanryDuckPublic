using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefaultControls", menuName = "ScriptableObjects/GameDefaultControls", order = 1)]
public class InputSCRObj : ScriptableObject
{
    //Дефолтные значения по умолчанию
    [SerializeField] InputControlButton[] inputButtons;
    public InputControlButton getByName(string name)
    {
        return inputButtons.FirstOrDefault(obj => obj.inputName == name);
    }

    public InputControlButton[] getButtons()
    {
        return inputButtons;
    }

    public void setNewButtonCode(string name, KeyCode newCode)
    {
       getByName(name).buttonCode = newCode;
    }

} 
