using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemyDF1 : ParentTeleportingEnemy
{

    EnemyBulletTest enemyBulletStats;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<BasicEnemyMovement>();
    }


    // Start is called before the first frame update
    void Start()
    {
        patternCompleted = true;
        weightedPercentFloat = 0f;
        agroRange = 20f;
        simpleWeightedPercent = 0;
        otherWeightedPercent = 0f;
        teleportRange = 5f;

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
        Debug.Log("pattern1");
        float randomRange = Random.Range(3, 6);
        Vector2 myVector = new Vector3(playersLastPosition.x - (float)randomRange, playersLastPosition.y, 0);
        transform.position = myVector;

        int closeRandom = Random.Range(4, 6);
        float shootTimer = closeRandom / 2;

        Invoke("enemyShoot", shootTimer);
        Invoke("completePattern", closeRandom);
    }

    //start moving to the left 
    public void Pattern2()
    {
        Debug.Log("pattern1");
        float randomRange = Random.Range(3, 6);
        Vector2 myVector = new Vector3(playersLastPosition.x + (float)randomRange, playersLastPosition.y, 0);
        transform.position = myVector;

        int closeRandom = Random.Range(4, 6);
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

        

        Instantiate(enemyBullet, transform.position, Quaternion.identity);
    }

}
