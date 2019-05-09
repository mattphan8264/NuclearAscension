using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : Bullet
{
    public delegate int bossKiller(int input);
    public bossKiller onBossHit;
    public Vector2 startPos;
    public Vector2 endPos;
    public bool speedBullet;

    void Start()
    {
        startPos = transform.position;
    }

    public override void dealDamage()
    {
        if (speedBullet)
        {
            endPos = transform.position;
            int temp = (int)Vector2.Distance(startPos, endPos);
            temp = temp % 3;
            damage += temp;
        }

        if (isBoss)
        {
            damage = onBossHit(damage);
        }

        enemy.dealDamage(damage);
        destroy();
    }
}