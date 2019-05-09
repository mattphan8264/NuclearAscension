using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyDF2 : ParentBasicEnemy
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
        int closeRandom = Random.Range(1, 3);

        if (closeRandom == 1)
        {
            Pattern2();
        }

        if (closeRandom == 2)
        {
            Pattern3();
        }
    }

    public override void closeLeftPatterns()
    {
        //Debug.Log("Left patterns");
        patternCompleted = false;
        int closeRandom = Random.Range(1, 3);

        if (closeRandom == 1)
        {
            Pattern2();
        }

        if (closeRandom == 2)
        {
            Pattern3();
        }
    }

    public override void farRightPatterns()
    {
        //Debug.Log("right Patterns");
        patternCompleted = false;
        int closeRandom = Random.Range(1, 3);

        if (closeRandom == 1)
        {
            Pattern1();
        }

        if (closeRandom == 2)
        {
            Pattern4();
        }
    }

    public override void closeRightPatterns()
    {
        patternCompleted = false;
        int closeRandom = Random.Range(1, 3);

        if (closeRandom == 1)
        {
            Pattern1();
        }

        if (closeRandom == 2)
        {
            Pattern4();
        }
    }

    //start moving to the right
    public void Pattern1()
    {
        //Debug.Log("pattern1");
        moveX(.1f);

        int closeRandom = Random.Range(1, 2);
        Invoke("completePattern", closeRandom);
    }

    //start moving to the left 
    public void Pattern2()
    {
        //Debug.Log("pattern2");
        moveX(-.1f);

        int closeRandom = Random.Range(1, 2);
        Invoke("completePattern", closeRandom);
    }

    //move to the left and shoot once
    public void Pattern3()
    {
        //Debug.Log("pattern3");
        moveX(-.1f);

        int closeRandom = Random.Range(1, 2);
        float shootTimer = closeRandom / 2;

        Invoke("enemyShoot", shootTimer);
        Invoke("completePattern", closeRandom);
    }

    //move to the right and shoot once 
    public void Pattern4()
    {
        //Debug.Log("pattern4");
        moveX(.1f);

        int closeRandom = Random.Range(1, 2);
        float shootTimer = closeRandom / 2;

        Invoke("enemyShoot", shootTimer);
        Invoke("completePattern", closeRandom);
    }

    //shoot

    //shoot
    public void enemyShoot()
    {
        float randomOffset = Random.Range(-2.0f, 2.0f);


        enemyBulletStats = enemyBullet.GetComponent<EnemyBulletTest>();
        if (player.transform.position.x < transform.position.x)
        {
            enemyBulletStats.direction = player.transform.position - transform.position + new Vector3(randomOffset, 0, 0);
            enemyBulletStats.damage = 1;
        }
        else
        {
            enemyBulletStats.direction = player.transform.position - transform.position + new Vector3(randomOffset, 0, 0);
            enemyBulletStats.damage = 1;
        }

        //Debug.Log("player position: " + player.transform.position);

        Instantiate(enemyBullet, transform.position, Quaternion.identity);
    }

}
