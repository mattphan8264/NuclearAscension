using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoppingEnemyDF1 : ParentHoppingEnemy
{

    EnemyBulletTest enemyBulletStats;

    // Start is called before the first frame update
    void Start()
    {
        patternCompleted = true;
        weightedPercentFloat = 0f;
        agroRange = 20f;
        simpleWeightedPercent = 0;
        otherWeightedPercent = 0f;
    }

    public override void farLeftPatterns()
    {
        patternCompleted = false;
        int closeRandom = Random.Range(1, 2);

        if (closeRandom == 1)
        {
            Pattern2();
        }

    }

    public override void closeLeftPatterns()
    {
        //Debug.Log("Left patterns");
        patternCompleted = false;
        int closeRandom = Random.Range(1, 2);

        if (closeRandom == 1)
        {
            Pattern2();
        }

    }

    public override void farRightPatterns()
    {
        //Debug.Log("right Patterns");
        patternCompleted = false;
        int closeRandom = Random.Range(1, 2);

        if (closeRandom == 1)
        {
            Pattern1();
        }

    }

    public override void closeRightPatterns()
    {
        patternCompleted = false;
        int closeRandom = Random.Range(1, 2);

        if (closeRandom == 1)
        {
            Pattern1();
        }
    }

    //start moving to the right
    public void Pattern1()
    {
        //Debug.Log("pattern1");
        moveX(.1f);
        moveY(1f);

        int closeRandom = Random.Range(1, 2);
        Invoke("completePattern", closeRandom);
    }

    //start moving to the left 
    public void Pattern2()
    {
        //Debug.Log("pattern2");
        moveX(-.1f);
        moveY(1f);

        int closeRandom = Random.Range(1, 2);
        Invoke("completePattern", closeRandom);
    }


    //shoot
    public void enemyShoot()
    {
        enemyBulletStats = enemyBullet.GetComponent<EnemyBulletTest>();
        if (player.transform.position.x < transform.position.x)
        {
            enemyBulletStats.direction = new Vector2(-.5f, 0);
            enemyBulletStats.damage = 1;
        }
        else
        {
            enemyBulletStats.direction = new Vector2(.5f, 0);
            enemyBulletStats.damage = 1;
        }


        Instantiate(enemyBullet, transform.position, Quaternion.identity);
    }

}
