using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField]
    int MinAmmoAdd;
    [SerializeField]
    int MaxBonusAmmoAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Duck")
        {
            GameObject.Find("Drone").GetComponent<Weapon>().AddAmmo(MinAmmoAdd + new System.Random().Next(0, MaxBonusAmmoAdd + 1));
            Destroy(gameObject);
        }
    }
}
