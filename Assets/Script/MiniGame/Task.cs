using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public GameObject Parent, TaskComleted;
    public Text TaskCounter;

    public System.Random rnd = new();

    public bool taskActive = false, taskComplete = false;

    public virtual void Completer()
    {
        TaskComleted.SetActive(true);
        taskComplete = true;
        GameObject.Find("Manager").GetComponent<TaskChecker>().iamDONE(gameObject);
        Invoke("closeTask", 0.5f);
    }

    public virtual void closeTask()
    {
        taskActive = false;
        if(TaskCounter) TaskCounter.text = "";
        if (TaskComleted) TaskComleted.SetActive(false);
        Parent.SetActive(false);
    }

    public virtual void restartTask()
    {
        taskComplete = false;
    }

}
