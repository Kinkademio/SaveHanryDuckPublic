using System;
using System.Collections;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//СТРУКТУРА JSON 
[Serializable]
public class Test
{
    public string _id;
    public string taskName;
    public Tasks[] tasks;
}

[Serializable]
public class Tasks
{
    public string text;
    public Ansver[] ansvers;
}

[Serializable]
public class Ansver
{
    public string text;
    public bool right;
}


public class GameManagerScript : MonoBehaviour
{
    //Костыльно так нельзя нужен scrObj
    [SerializeField] GameObject TestUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject SettingsUI;
    [SerializeField] GameObject HelpUI;
    [SerializeField] GameObject WinUI;
    [SerializeField] GameObject LoseUI;
    [SerializeField] GameObject ScoreUI;

    [SerializeField] AudioSource menuSoundPlayer;
    [SerializeField] AudioSource gameSoundPlayer;
    [SerializeField] Text timerField;
    [SerializeField] public float lifeTimer = 10f;
    [SerializeField] float baseVolume = 0.5f;

    private GameObject currentActiveUI;
    public string testHash;
    public Test test;

    public enum GameState
    {   
        onTestUI,
        onMenu, 
        onPause,
        GameProcess,
    }

    //Текущее состояние игры
    public GameState gameState;


    public GameObject Player;
    public GameObject PlayerDrone;
    GameObject MainCamera;
    
    private float Timer;
    private float allSurviveTime = 0;


    //Смена состояния игры
    public void changeGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.GameProcess:
                break;
            case GameState.onPause:
                break;
            case GameState.onMenu:
                break;
        }
        gameState = newState;
    }
    //Интерфейс для получения текущего состояния игры из-вне
    public GameState getCurretGameState()
    {
        return gameState;
    }


    private void Start()
    {
        //Устанавливаем по умолчанию состояние игры в Меню
        changeGameState(GameState.onTestUI);

        Timer = lifeTimer;
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", baseVolume);                                                                                                                 
        }
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        this.currentActiveUI = TestUI;
        menuSoundPlayer.Play();

        MainCamera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        if (gameState == GameState.GameProcess)
        {
            endGame();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                this.openMenu();
                gameSoundPlayer.Pause();
                menuSoundPlayer.Play();
            }
        }
    }


    //Закрытие приложения игры
    public void exitGame()
    {
        Application.Quit();
    }
    

    //Открытие настроек игры
    public void openSettings()
    {
        this.swapVisibleUi(currentActiveUI, SettingsUI);
        GameObject.FindGameObjectWithTag("SoundSettingsSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");
    }
    
    public void openScores()
    {
        this.swapVisibleUi(currentActiveUI, ScoreUI);
        TableFill tableControl =  ScoreUI.GetComponentInChildren<TableFill>();
        tableControl.fillTable();
    }

    public void openMenu()
    {
        this.swapVisibleUi(currentActiveUI, MenuUI);
        GameObject playButton = GameObject.Find("LoadGame");
        Text buttonText = playButton.GetComponentInChildren<Text>();
        if (gameState == GameState.GameProcess)
        {
            pauseGame();
        }
        if (gameState == GameState.onPause)
        {
            buttonText.text = "Продолжить";
        }
        else
        {
            buttonText.text = "Играть";
        }
    }
    
    IEnumerator getTestData()
    {
        string url = "https://graduate-map.ru/api/test/" + this.testHash;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text != "")
                {
                    test = JsonUtility.FromJson<Test>(www.downloadHandler.text);
                    this.openMenu();
                }
            }
        }
    }
   public void openMenuFromTestUI(string str)
    {
        changeGameState(GameState.onMenu);
        this.testHash = str;
        StartCoroutine(getTestData());
    }

    //Переход к игре
    public void backToGame()
    {
        this.swapVisibleUi(currentActiveUI, GameUI);
        menuSoundPlayer.Pause();
        gameSoundPlayer.Play();
        changeGameState(GameState.GameProcess);
    }

    //Сохранение игрововго прогресса
    public void saveGame()
    {
        //Тут нужно найти менеджер игровых уровней и получить от него массив собранных кветов и их тип и  сохранить в строку
        string saveString = "";
        PlayerPrefs.SetString("save", saveString);
    }

    public void loadGame()
    {
        if(gameState == GameState.onMenu) {
            allSurviveTime = 0;
            ScoreController.createNewScores();
            //Тут нужно эту структуру проинициализировать и выполнить действия для загрузки юзера на уровень
            Player.transform.position = new Vector3(0, 0, Player.transform.position.z);
            Player.SetActive(true);
            PlayerDrone.transform.position = new Vector3(0, 0, -3);
            PlayerDrone.SetActive(true);
            MainCamera.GetComponent<GenerationManager>().enabled = true;
            MainCamera.GetComponent<GenerationManager>().GenerationManagerFirstRoom();
            MainCamera.transform.position = new Vector3(0, 0, MainCamera.transform.position.z);
            MainCamera.GetComponent<CameraControl>().enabled = true;
            Player.GetComponent<PlayerControl>().SetKeyboardActive(true);

            PlayerDrone.GetComponent<Weapon>().Initializate();
        }
        if (gameState == GameState.onPause)
        {
            resumeGame();
        }   
        backToGame();
    }

    public void endGame()
    {
        if (Timer < 0f)
        {
            this.swapVisibleUi(currentActiveUI, LoseUI);
            changeGameState(GameState.onMenu);
            ScoreController.setCurrentScoreNewValue("survive_time", convertTimertoStringVal(allSurviveTime));
            ScoreController.saveCurrentScore();
            deactivateGame();
            
        }
        else
        {
            allSurviveTime += Time.deltaTime;
            Timer -= Time.deltaTime;
            updateTimer(Timer);
        }
    }

    public void winGame()
    {
        gameObject.GetComponent<TaskChecker>().ResetMiniGames();

        this.swapVisibleUi(currentActiveUI, WinUI);
        changeGameState(GameState.onMenu);
        ScoreController.setCurrentScoreNewValue("survive_time", convertTimertoStringVal(allSurviveTime));
        //ScoreController.setCurrentScoreNewValue("survived", "Да");
        ScoreController.saveCurrentScore();
        deactivateGame();
    }

    public void deactivateGame()
    {

        gameObject.GetComponent<TaskChecker>().ResetMiniGames();
        Player.GetComponent<PlayerControl>().SetKeyboardActive(false);
        MainCamera.GetComponent<GenerationManager>().DestroyAllWithout();
        MainCamera.GetComponent<GenerationManager>().BaseRoomSize = 0;
        MainCamera.GetComponent<GenerationManager>().enabled = false;
        MainCamera.GetComponent<CameraControl>().enabled = false;
        PlayerDrone.SetActive(false);
        Player.SetActive(false);

        

        resetTimer();
    }

    public void resetGame()
    {

        this.swapVisibleUi(currentActiveUI, MenuUI);
        gameSoundPlayer.Pause();
        menuSoundPlayer.Play();
    }


    //Пауза
    public void pauseGame()
    {
       gameState = GameState.onPause;
       Time.timeScale = 0f;
    }

    //Возобновление игрового процесса
    public void resumeGame()
    {
        Time.timeScale = 1f;
    }

    //Меняет активынй UI
    private void swapVisibleUi(GameObject hide, GameObject show)
    {
        hide.SetActive(false);
        show.SetActive(true);
        currentActiveUI = show;
    }

    //Изменеие громкости звука
    public void changeGameVolume(float volume)
    {
        Debug.Log(volume);
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
    }

    private string convertTimertoStringVal(float value)
    {
        string time = System.Convert.ToString(value);
        string[] parts = time.Split(',');
        return parts[0];
    }
    private void updateTimer(float newTime)
    {
        timerField.text = convertTimertoStringVal(newTime);

    }


    public void resetTimer()
    {
        this.Timer = lifeTimer;
    }


    public void openHelp()
    {
        this.swapVisibleUi(currentActiveUI, HelpUI);
    }
}

