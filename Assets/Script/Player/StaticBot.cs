using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBot : MonoBehaviour
{
    //[SerializeField]
    GameObject target;
    Weapon weapon;
    Animator animator;


    float targetDistance;
    float animationDelayConst;
    float animationDelay;

    private void Start()
    {
        weapon = gameObject.GetComponent<Weapon>();
        target = GameObject.Find("Duck");
        animator = this.GetComponent<Animator>();

        animationDelayConst = weapon.GetTimeBetweenShots();

        gameObject.GetComponent<Weapon>().target = target;
    }

    void Update()
    {
        Timer(ref animationDelay);

        targetDistance = Mathf.Sqrt (
            (target.transform.position.x - gameObject.transform.position.x) * (target.transform.position.x - gameObject.transform.position.x) +
            (target.transform.position.y - gameObject.transform.position.y) * (target.transform.position.y - gameObject.transform.position.y));

        if ((targetDistance < 1.5) && (animationDelay == 0))
        {
            weapon.SetShoot(true);
            animator.Play("BotStatic_atack");
            animationDelay = animationDelayConst;
        }
        else
        {
            weapon.SetShoot(false);
        }
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
