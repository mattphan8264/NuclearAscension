using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : Bullet {

    public int hitCount;
    public int currentCount;

    public float critDamage;

    void Start()
    {
        if (duration == 0)
        {
            duration = 3;
        }
        else
        {
            duration += 3;
        }

        if (hitCount == 0)
        {
            hitCount = 1;
        }

        if (critDamage == 0)
        {
            critDamage = 1;
        }
        Destroy(gameObject, duration);
    }


    public override void dealDamage()
    {
        currentCount++;
        if (currentCount >= hitCount)
        {
            enemy.dealDamage(damage);
            destroy();
        }
        else
        {
            enemy.dealDamage(damage);
            damage = (int)(damage * critDamage);
        }
    }
}