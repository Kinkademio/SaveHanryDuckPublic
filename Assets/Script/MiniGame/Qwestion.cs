using RoomInteriorGeneratorTag;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEditor.Progress;

public class Qwestion : Task
{
    public UnityEngine.UI.Text qwest;

    public GameObject buttonsBlock;
    public GameObject button;
    public GameObject horizontalBlock;
    
    public GameManagerScript manager;

    private Tasks currentTask;
    private int IndexTask = 0, IndexSwap, CountSwap = 0;

    void OnEnable()
    {
        IndexSwap = manager.test.tasks.Length - 1;

        TestLoad();
    }

    void CheckAnswer(bool ansver)
    {
        if (ansver)
        {
            if ((manager.test.tasks.Length - 1) == IndexTask){
                IndexTask = 0;
                Completer();
            }
            else
            {
                IndexTask++;
                TaskComleted.SetActive(true);
                Invoke("closeTask", 0.5f);
            }
        }
        else
        {
            //Что-то про не верный ответ
            Tasks thisAnsver = manager.test.tasks[IndexTask];
            manager.test.tasks[IndexTask] = manager.test.tasks[IndexSwap];
            manager.test.tasks[IndexSwap] = thisAnsver;

            if ((IndexSwap - 1) > IndexTask)
            {
                CountSwap++;
            }
            else
            {
                CountSwap = 0;
            }
            IndexSwap = manager.test.tasks.Length - 1 - CountSwap;

            TestLoad();
        }
    }

    public override void Completer()
    {
        TaskComleted.SetActive(true);
        taskComplete = true;
        //GameObject.Find("Manager").GetComponent<TaskChecker>().iamDONE(gameObject);
        Invoke("closeTask", 0.5f);
    }

    public override void closeTask()
    {
        GameObject.Find("Manager").GetComponent<TaskChecker>().iamDONE(gameObject);
        base.closeTask();
    }

    public override void restartTask()
    {
        IndexTask = 0;
        CountSwap = 0;
        base.restartTask();
    }

    void TestLoad()
    {
        currentTask = manager.test.tasks[IndexTask];
        SetQwestion(currentTask.text);

        DespawnButton();

        GameObject lastHorizontalBlock = null;
        for (int i = 0; i < currentTask.ansvers.Length; i++)
        {
            if (i % 2 == 0)
            {
                lastHorizontalBlock = Instantiate(horizontalBlock, buttonsBlock.transform);
            }

            SpawnButton(currentTask.ansvers[i].text, lastHorizontalBlock, currentTask.ansvers[i].right);
        }
    }

    void SetQwestion(string text)
    {
        qwest.text = text;
    }

    void SpawnButton(string text, GameObject Parent, bool ansver)
    {
        GameObject Button = Instantiate(button, Parent.transform);
        Button.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
        Button.GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(ansver); }); ;
    }

    void DespawnButton()
    {
        int childCounter = buttonsBlock.transform.childCount;
        for (int i = 0; i < childCounter; i++)
        {
            Destroy(buttonsBlock.transform.GetChild(i).gameObject);
        }
    }
}

