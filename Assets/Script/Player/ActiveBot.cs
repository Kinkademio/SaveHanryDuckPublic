using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ActiveBot : MonoBehaviour
{
    //[SerializeField]
    GameObject target;
    Weapon weapon;
    SpriteRenderer sprite;

    float targetDistance;

    int flag;
    public Vector2 Napravlenie;
    public Rigidbody2D rb;
    bool bforward, bright45, bright90, bright135, bleft45, bleft90, bleft135;
    float RayRadius = 0.6f;
    float RayLenght = 0.25f;

    float ReactionDelayConst = 0.4f;
    float reactionDelay = 0f;

    private void Start()
    {
        weapon = gameObject.GetComponent<Weapon>();
        target = GameObject.Find("Duck");
        sprite = this.GetComponent<SpriteRenderer>();

        gameObject.GetComponent<Weapon>().target = target;
    }

    void Update()
    {
        targetDistance = Mathf.Sqrt(
            (target.transform.position.x - gameObject.transform.position.x) * (target.transform.position.x - gameObject.transform.position.x) +
            (target.transform.position.y - gameObject.transform.position.y) * (target.transform.position.y - gameObject.transform.position.y));

        if (targetDistance < 1.5)
        {
            weapon.SetShoot(true);
        }
        else
        {
            weapon.SetShoot(false);
        }

        Timer(ref reactionDelay);


        float x = target.transform.position.x - this.GetComponent<Transform>().position.x;
        float y = target.transform.position.y - this.GetComponent<Transform>().position.y;

        Vector2 lookDirection = new Vector2(x, y);
        float localRotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (90 < Mathf.Abs(localRotation))
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }

        if (reactionDelay == 0)
        {

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation) * Mathf.Deg2Rad);
            RaycastHit2D forward0 = Physics2D.Raycast(new Vector2(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation - 45) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation - 45) * Mathf.Deg2Rad);
            RaycastHit2D right45 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation - 90) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation - 90) * Mathf.Deg2Rad);
            RaycastHit2D right90 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation - 135) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation - 135) * Mathf.Deg2Rad);
            RaycastHit2D right135 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation + 45) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation + 45) * Mathf.Deg2Rad);
            RaycastHit2D left45 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation + 90) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation + 90) * Mathf.Deg2Rad);
            RaycastHit2D left90 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            Napravlenie.x = RayRadius * Mathf.Cos((localRotation + 135) * Mathf.Deg2Rad);
            Napravlenie.y = RayRadius * Mathf.Sin((localRotation + 135) * Mathf.Deg2Rad);
            RaycastHit2D left135 = Physics2D.Raycast(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), RayLenght);
            Debug.DrawRay(new Vector3(transform.position.x + Napravlenie.x, transform.position.y + Napravlenie.y, transform.position.z), transform.TransformDirection(Napravlenie), UnityEngine.Color.red);

            if ((forward0.collider != null) && (!forward0.collider.isTrigger))
            {
                bforward = true;
            }
            else
            {
                bforward = false;
            }

            if ((right45.collider != null) && (!right45.collider.isTrigger))
            {
                bright45 = true;
            }
            else
            {
                bright45 = false;
            }

            if ((right90.collider != null) && (!right90.collider.isTrigger))
            {
                bright90 = true;
            }
            else
            {
                bright90 = false;
            }

            if ((right135.collider != null) && (!right135.collider.isTrigger))
            {
                bright135 = true;
            }
            else
            {
                bright135 = false;
            }

            if ((left45.collider != null) && (!left45.collider.isTrigger))
            {
                bleft45 = true;
            }
            else
            {
                bleft45 = false;
            }

            if ((left90.collider != null) && (!left90.collider.isTrigger))
            {
                bleft90 = true;
            }
            else
            {
                bleft90 = false;
            }

            if ((left135.collider != null) && (!left135.collider.isTrigger))
            {
                bleft135 = true;
            }
            else
            {
                bleft135 = false;
            }

            //Поведение бота

            if (targetDistance >= 1.5f)
            {
                if ((bforward == true) && (bright90 == true) && (bleft90 == true))
                {
                    Napravlenie.x = Mathf.Cos((localRotation + 180) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation + 180) * Mathf.Deg2Rad);
                }
                else if ((bforward == true) && (bright90 == true) && (bleft45 == true))
                {
                    Napravlenie.x = Mathf.Cos((localRotation + 90) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation + 90) * Mathf.Deg2Rad);
                }
                else if ((bforward == true) && (bright45 == true) && (bleft90 == true))
                {
                    Napravlenie.x = Mathf.Cos((localRotation - 90) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation - 90) * Mathf.Deg2Rad);
                }
                else if ((bforward == true) && (bright45 == true) && (bleft45 == true))
                {
                    flag = Random.Range(1, 3);
                    if (flag == 1)
                    {
                        Napravlenie.x = Mathf.Cos((localRotation + 90) * Mathf.Deg2Rad);
                        Napravlenie.y = Mathf.Sin((localRotation + 90) * Mathf.Deg2Rad);
                    }
                    if (flag == 2)
                    {
                        Napravlenie.x = Mathf.Cos((localRotation - 90) * Mathf.Deg2Rad);
                        Napravlenie.y = Mathf.Sin((localRotation - 90) * Mathf.Deg2Rad);
                    }
                }
                else if ((bforward == true) && (bleft45 == true))
                {
                    Napravlenie.x = Mathf.Cos((localRotation - 45) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation - 45) * Mathf.Deg2Rad);
                }
                else if ((bforward == true) && (bright45 == true))
                {
                    Napravlenie.x = Mathf.Cos((localRotation + 45) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation + 45) * Mathf.Deg2Rad);
                }
                else if (bforward == true)
                {
                    flag = Random.Range(1, 3);
                    if (flag == 1)
                    {
                        Napravlenie.x = Mathf.Cos((localRotation + 45) * Mathf.Deg2Rad);
                        Napravlenie.y = Mathf.Sin((localRotation + 45) * Mathf.Deg2Rad);
                    }
                    if (flag == 2)
                    {
                        Napravlenie.x = Mathf.Cos((localRotation - 45) * Mathf.Deg2Rad);
                        Napravlenie.y = Mathf.Sin((localRotation - 45) * Mathf.Deg2Rad);
                    }
                }
                else if (bleft45 == true)
                {
                    Napravlenie.x = Mathf.Cos((localRotation - 45) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation - 45) * Mathf.Deg2Rad);
                }
                else if (bright45 == true)
                {
                    Napravlenie.x = Mathf.Cos((localRotation + 45) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation + 45) * Mathf.Deg2Rad);
                }
                else
                {
                    Napravlenie.x = Mathf.Cos((localRotation) * Mathf.Deg2Rad);
                    Napravlenie.y = Mathf.Sin((localRotation) * Mathf.Deg2Rad);
                }

            }
            else
            {
                Napravlenie.x = Mathf.Cos((localRotation + 180) * Mathf.Deg2Rad);
                Napravlenie.y = Mathf.Sin((localRotation + 180) * Mathf.Deg2Rad);
            }

            reactionDelay = ReactionDelayConst;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Napravlenie.normalized * 200);
    }

    bool Timer(ref float waitingTime)
    {
        if (waitingTime > 0)
            waitingTime -= Time.deltaTime;

        if (waitingTime < 0)
        {
            waitingTime = 0;
            return true;
        }

        return false;
    }
}
