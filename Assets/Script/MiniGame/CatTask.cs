using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatTask : Task, IPointerDownHandler, IPointerUpHandler
{
    public Text TaskTimer;
    public int maxHoldTime = 10, minHoldTime = 0;
    public int Reqwest = 0;
    bool corutineWork = false;
    public IEnumerator coroutine;
    public void OnPointerDown(PointerEventData eventData) { StartTask(); }
    public void OnPointerUp(PointerEventData eventData) { Stop(); }
    public void Stop()
    {
        StopCoroutine();
        if (Reqwest >= minHoldTime && Reqwest <= maxHoldTime)
        {
            TaskComleted.SetActive(true);
            taskComplete = true;

            int fixedObjects = int.Parse(ScoreController.getCurrentScoreByName("fixed_objects").scoreValue);
            fixedObjects++;
            ScoreController.setCurrentScoreNewValue("fixed_objects", fixedObjects.ToString());

            Completer();
        }
        else if (Reqwest < minHoldTime) { TaskCounter.text = "Мало :<"; }
        else { TaskCounter.text = "Долго >:0"; }

    }

    public void StartTask()
    {
        if (minHoldTime == 0) { MinHoldTimetRND(); }
        Debug.Log("Мин:" + minHoldTime);
        Reqwest = 0;
        if (!corutineWork)
        {
            coroutine = TestCoroutine();
            StartCoroutine(coroutine);
            corutineWork = true;
        }

    }
    private void MinHoldTimetRND() { minHoldTime = rnd.Next(1, 6); }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            Reqwest++;
            TaskTimer.text = "Время:" + Reqwest;
            yield return new WaitForSeconds(1f);
        }
    }

    public void StopCoroutine()
    {
        StopCoroutine(coroutine);
        corutineWork = false;
    }

    public override void closeTask()
    {
        if (corutineWork) StopCoroutine();
        minHoldTime = 0;
        Reqwest = 0;
        TaskTimer.text = "" + Reqwest;
        base.closeTask();
    }
}
