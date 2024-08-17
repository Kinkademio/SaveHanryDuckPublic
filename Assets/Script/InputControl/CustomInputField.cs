using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CustomInputField : InputField
{
    public InputSettingsObject parentInputController;
    public new void Start()
    {
        base.Start();
        parentInputController = gameObject.GetComponentInParent<InputSettingsObject>();
    }
    private void OnGUI()
    {
        if(EventSystem.current.currentSelectedGameObject != gameObject) return;
        KeyCode currentPressedButton = KeyCode.None;
        Event current = Event.current;
        if (current.isKey)
        {
            currentPressedButton = current.keyCode;
        }
        if (current.isMouse)
        {
            Debug.Log(current.button);
            currentPressedButton = (KeyCode) (323 + current.button);
        }
        if (currentPressedButton == KeyCode.None) return;

        if (!parentInputController)
        {
            throw new System.Exception("В родительском компоненте не установлен скрипт InputSettingsObject!");
        }
        //Записываем нажатую клавишу
        text = currentPressedButton.ToString();
        parentInputController.chandeInputSetting(currentPressedButton);
        EventSystem.current.SetSelectedGameObject(null);
    }
   
}
