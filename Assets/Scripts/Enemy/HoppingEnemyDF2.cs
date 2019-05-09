using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoppingEnemyDF2 : ParentHoppingEnemy
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

    //move to the left and shoot once
    public void Pattern3()
    {
        //Debug.Log("pattern3");
        moveX(-.1f);
        moveY(1f);

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
        moveY(1f);

        int closeRandom = Random.Range(1, 2);
        float shootTimer = closeRandom / 2;

        Invoke("enemyShoot", shootTimer);
        Invoke("completePattern", closeRandom);
    }

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
            enemyBulletStats.direction = player.transform.position - transform.position + new Vector3(randomOffset,0,0);
            enemyBulletStats.damage = 1;
        }

        //Debug.Log("player position: " + player.transform.position);

        Instantiate(enemyBullet, transform.position, Quaternion.identity);
    }

    //messing with this every once in a while to see if i can make bullets a little more interesting
    /*
      public  Vector3 GetProjectilVelocity(Vector3 target, Vector3 origin)
        {
        float projectileSpeed = 30.0f;

        Vector3 velocity = Vector3.zero;
        Vector3 toTarget = target - origin;

        float gSquared = Physics.gravity.sqrMagnitude;
        float b = projectileSpeed * projectileSpeed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if (discriminant < 0)
        {
            Debug.Log("am i stuck in here?");
            velocity = toTarget;
            velocity.y = 0;
            velocity.Normalize();
            velocity.y = 0.7f;

            velocity *= projectileSpeed;
            return velocity;
        }
        Debug.Log("do i get here");
        float discRoot = Mathf.Sqrt(discriminant);

        // Highest
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

        // Lowest speed arc
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));

        // Most direct with max speed
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

        float T = 0;

        // 0 = highest, 1 = lowest, 2 = most direct
        int shotType = 0;

        switch (shotType)
        {
            case 0:
                T = T_max;
                break;
            case 1:
                T = T_lowEnergy;
                break;
            case 2:
                T = T_min;
                break;
            default:
                break;
        }

        velocity = toTarget / T - Physics.gravity * T / 2f;

        return velocity;
    }
    */
}
