using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpaceShipTask : Task, IPointerDownHandler, IPointerUpHandler
{
    public GameObject SpaceShip, Star1, Star2, Star3;
    public readonly int RequiredStar = 3;
    public int Reqwest = 0;

    public Vector2 point;

    public void Start() { point = SpaceShip.transform.position; }

    //public void Awake() { SpaceShip.transform.position = point; }
    public void OnPointerDown(PointerEventData eventData) {  }

    public void OnPointerUp(PointerEventData eventData) {  StartTask(); }

    public void CheckTask(Vector2 point)
    {
        int fixedObjects = int.Parse(ScoreController.getCurrentScoreByName("fixed_objects").scoreValue);
        fixedObjects++;
        ScoreController.setCurrentScoreNewValue("fixed_objects", fixedObjects.ToString());

        Completer();
    }

    public override void closeTask() 
    {
        Star1.SetActive(true);
        Star2.SetActive(true);
        Star3.SetActive(true);
        SpaceShip.transform.position = point;
        Reqwest = 0;
        base.closeTask();
    }
        
    public void StartTask()
    {
        switch (Reqwest)
        {
            case 0:
                
                PickUpStar(Star1);
                break;
            case 1:
                PickUpStar(Star2);
                break;
            case 2:
                PickUpStar(Star3);
                break;
        }

        if (Reqwest == RequiredStar) CheckTask(point);
    }

    public void PickUpStar(GameObject Star)
    {
        SpaceShip.transform.position = Star.transform.position;
        Reqwest++;
        TaskCounter.text = "Колличество звёзд:" + Reqwest;
        Star.SetActive(false);
    }
}