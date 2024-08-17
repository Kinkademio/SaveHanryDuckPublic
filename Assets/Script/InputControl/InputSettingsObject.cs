using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSettingsObject : MonoBehaviour
{
    [SerializeField] Text inputSettingName;
    [SerializeField] InputField inputSettingButtonPicker;

    public InputControlButton inputButton;

    public void initSettingObject(InputControlButton button)
    {
        inputButton = button;
        setSettingName(button.inputForMenu);
        setButtonPiackerLabel(button.buttonCode.ToString());
    }
   

    public void setSettingName(string name)
    {
       inputSettingName.text = name + ":";
    }
    public string getSettingName()
    {
        return inputSettingName.text;
    }

  
    public void setButtonPiackerLabel(string newInput)
    {
        inputSettingButtonPicker.text = newInput;
    }

    public void chandeInputSetting(KeyCode newButton)
    {
        InputController.changeInput(inputButton.inputName, newButton);
    }


}
