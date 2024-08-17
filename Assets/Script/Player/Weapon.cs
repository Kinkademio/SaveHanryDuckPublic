using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public bool Player;
    [SerializeField]
    private bool keyboardActive;

    Camera MainCamera;

    //[SerializeField]
    public GameObject target;
    [SerializeField]
    TrailRenderer tracerEffect;
    [SerializeField] 
    Text ammoField;

    Vector2 targetPosition;
    Vector2 lookDirection;

    [SerializeField]
    float TimeBetweenShots;
    [SerializeField]
    float ReloadTime;
    [SerializeField]
    int MagazineSize;
    [SerializeField]
    int MaxAmmoCount;
    [SerializeField]
    int StartAmmoCount;
    [SerializeField]
    int Damage;
    [SerializeField]
    int BulletInShootCount;
    [SerializeField]
    float Spread;
    [SerializeField]
    float AttackRadius;
    [SerializeField]
    float RepulsiveForce;
    [SerializeField]
    bool UsingAmmo;


    float betweenShots;
    float reload;
    int magazine;
    int ammoCount;
    bool shooting;

    private void Start()
    {
        Initializate();
    }

    public void Initializate()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        betweenShots = 0;
        reload = 0;
        magazine = MagazineSize;
        ammoCount = StartAmmoCount;
        shooting = false;
    }

    void Update()
    {
        Timer(ref betweenShots);

        if (Timer(ref reload)) 
        { 
            Reload(); 
        }

        if ((magazine == 0) && (ammoCount > 0) && (reload == 0))
        {
            reload = ReloadTime;
        }

        if ((Player) && (keyboardActive) && Input.GetKeyDown(InputController.getInput("reload")) && (reload == 0) && (magazine < MagazineSize) && (ammoCount > 0))
        {
            ammoCount += magazine;
            magazine = 0;
            reload = ReloadTime;
        }

        if (Player && (keyboardActive) && Input.GetKey(InputController.getInput("shooting")))
        {
            shooting = true;
        }
        else if (!Player)
        {
            // TODO: bot logic
        }
        else
        {
            shooting = false;
        }

        if (shooting && (betweenShots == 0) && (reload == 0) && (magazine > 0))
        {
            if (Player) {
                targetPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            } else {
                targetPosition = target.transform.position;
            }

            for (int Bullet = 0; Bullet < BulletInShootCount; Bullet++)
            {
                float x = targetPosition.x - this.GetComponent<Transform>().position.x;
                float y = targetPosition.y - this.GetComponent<Transform>().position.y;

                lookDirection = new Vector2(
                    x + Random.Range(0f, x * Spread),
                    y + Random.Range(0f, y * Spread));

                Shooting();
                betweenShots = TimeBetweenShots;
            }

            if (UsingAmmo)
            {
                int vasteBullets = int.Parse(ScoreController.getCurrentScoreByName("uesed_ammo").scoreValue);
                vasteBullets++;
                ScoreController.setCurrentScoreNewValue("uesed_ammo", vasteBullets.ToString());
                magazine--;
            }
        }

        if (Player)
        {
            ammoField.text = (ammoCount + magazine).ToString();
        }
    }

    void Shooting()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(this.GetComponent<Transform>().position, lookDirection, AttackRadius);
        //Debug.DrawRay(this.GetComponent<Transform>().position, lookDirection, Color.red, 1.0f);

        float localRotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Vector2 Napravlenie;
        Napravlenie.x = AttackRadius * Mathf.Cos((localRotation) * Mathf.Deg2Rad);
        Napravlenie.y = AttackRadius * Mathf.Sin((localRotation) * Mathf.Deg2Rad);

        Vector2 EndShoot = 
            new Vector2(this.GetComponent<Transform>().transform.position.x + Napravlenie.x, this.GetComponent<Transform>().transform.position.y + Napravlenie.y);

        for (int i = 0; i < hit.Length; i++)
        {
            if ((hit[i].point.x == this.GetComponent<Transform>().position.x) && (hit[i].point.y == this.GetComponent<Transform>().position.y))
            {
                continue;
            }

            if (Player && hit[i].transform.gameObject.name == "Duck")
            {
                continue;
            }

            if ((hit[i].transform.gameObject.GetComponent<Health>() != null) && (!hit[i].collider.isTrigger))
            {
                EndShoot = hit[i].point;

                hit[i].transform.gameObject.GetComponent<Health>().TakeDamage(Damage);
                if (hit[i].transform.gameObject.GetComponent<Rigidbody2D>())
                {
                    hit[i].transform.gameObject.GetComponent<Rigidbody2D>().AddForce(-hit[i].normal.normalized * RepulsiveForce);
                }
                break;
            }
        }

        TrailRenderer tracer = Instantiate(tracerEffect, this.GetComponent<Transform>().position, Quaternion.identity);
        tracer.AddPosition(this.GetComponent<Transform>().position);

        tracer.transform.position = EndShoot;
    }

    void Reload()
    {
        if (ammoCount > MagazineSize)
        {
            ammoCount -= MagazineSize;
            magazine = MagazineSize;
        }
        else
        {
            magazine = ammoCount;
            ammoCount = 0;
        }
    }

    bool Timer(ref float waitingTime)
    {
        if (waitingTime > 0)
            waitingTime -= Time.deltaTime;

        if (waitingTime < 0) { 
            waitingTime = 0;
            return true;
        }

        return false;
    }

    public void SetShoot(bool shoot)
    {
        shooting = shoot;
    }

    public void AddAmmo(int countAddAmmo)
    {
        ammoCount += countAddAmmo;

        if (ammoCount > MaxAmmoCount)
        {
            ammoCount = MaxAmmoCount;
        }
    }

    public float GetTimeBetweenShots()
    {
        return TimeBetweenShots;
    }

    public void SetkeyboardActive(bool Active)
    {
        if (Active)
        {
            keyboardActive = true;
        }
        else
        {
            keyboardActive = false;
            shooting = false;
        }
    }

}
