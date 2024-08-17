using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElectricityTask : Task, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Spark;
    private int PointNumber = 0;
    public int Reqwest = 0;

    public void OnPointerDown(PointerEventData eventData) { Spark.SetActive(true); }
    public void OnPointerUp(PointerEventData eventData)
    {
        Spark.SetActive(false);
        Reqwest++;
        TaskCounter.text = "Колличество ударов:" + Reqwest;
        StartTask();
    }

    public override void closeTask()
    {
        Spark.SetActive(false);
        Reqwest = 0;
        base.closeTask();
    }

    private void StartTask()
    {
        if (PointNumber == 0) { PointRND(); }
        Debug.Log(PointNumber);
        if (Reqwest == PointNumber) Stop();
    }

    public void Stop()
    {
        int fixedObjects = int.Parse(ScoreController.getCurrentScoreByName("fixed_objects").scoreValue);
        fixedObjects++;
        ScoreController.setCurrentScoreNewValue("fixed_objects", fixedObjects.ToString());

        Completer();
    }

    private void PointRND() { PointNumber = rnd.Next(5, 30);}
}