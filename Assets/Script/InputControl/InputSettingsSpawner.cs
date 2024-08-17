using TMPro;
using UnityEngine;


public class InputSettingsSpawner : MonoBehaviour
{
    [SerializeField] GameObject inputSettingsPrefab;

    void Start()
    {
        InputControlButton[] allGameInputs = InputController.getAllInputs();
        int key = 0;
        foreach (InputControlButton gameInput in allGameInputs)
        {
           GameObject inputSettingsObj = Instantiate(inputSettingsPrefab, gameObject.transform);
           inputSettingsObj.name = inputSettingsObj.name + key;
           InputSettingsObject inputSettings = inputSettingsObj.GetComponent<InputSettingsObject>();
           inputSettings.initSettingObject(gameInput);
           key++;
        }
    }

}
