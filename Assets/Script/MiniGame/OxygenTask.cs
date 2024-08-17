using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OxygenTask : Task, IPointerDownHandler, IPointerUpHandler
{
    public Text TaskTimer;
    public int maxHoldTime = 10, minHoldTime = 0;
    public int Reqwest = 0;
    bool corutineWork = false;
    public IEnumerator coroutine;
    public void OnPointerDown(PointerEventData eventData) {  StartTask(); }
    public void OnPointerUp(PointerEventData eventData) { Stop(); }

    public void Awake()
    {
        TaskTimer.text = "" + Reqwest;
    }

    public void Stop()
    {
        StopCoroutine();
        if (Reqwest >= minHoldTime && Reqwest <= maxHoldTime)
        {
            int fixedObjects = int.Parse(ScoreController.getCurrentScoreByName("fixed_objects").scoreValue);
            fixedObjects++;
            ScoreController.setCurrentScoreNewValue("fixed_objects", fixedObjects.ToString());

            Completer();
        }
        else if (Reqwest < minHoldTime) { TaskCounter.text = "Слишком слабо"; }
        else { TaskCounter.text = "По аккуратнее!!!"; }

    }

    public void StartTask()
    {
        if (minHoldTime == 0) { MinHoldTimetRND(); }
        Debug.Log( "Мин:" + minHoldTime);
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
            TaskTimer.text = "Сила удара:" + Reqwest;
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
        base.closeTask();
    }
}