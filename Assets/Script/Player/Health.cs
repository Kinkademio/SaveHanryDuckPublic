using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoint;
    public bool CanTakeDamage;

    public void TakeDamage(int Damage)
    {
        if (CanTakeDamage)
        {
            healthPoint -= Damage;

            if (healthPoint <= 0)
            {
                float destroedObjects = int.Parse(ScoreController.getCurrentScoreByName("destroy_objects").scoreValue);
                destroedObjects++;
                ScoreController.setCurrentScoreNewValue("destroy_objects", destroedObjects.ToString());
                Destroy(gameObject);
            }
        }
    }

    public void TakeHealth(int HealthPoint)
    {
        healthPoint += HealthPoint;
    }
}
