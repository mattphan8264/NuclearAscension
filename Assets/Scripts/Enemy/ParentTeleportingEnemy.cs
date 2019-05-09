using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTeleportingEnemy : MonoBehaviour
{
    public GameObject player;
    public bool patternCompleted;
    public float agroRange;

    public float weightedPercentFloat;
    public int simpleWeightedPercent;
    public float otherWeightedPercent;
    public BasicEnemyMovement movement;
    public Vector2 playersLastPosition;
    public float teleportRange;

    public GameObject enemyBullet;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<BasicEnemyMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        weightedPercentFloat = dist / agroRange;
        otherWeightedPercent = weightedPercentFloat * 100f;
        simpleWeightedPercent = (int)otherWeightedPercent;

        //Debug.Log("Dist: " + dist);
        //Debug.Log("WeightedPercent: " + weightedPercentFloat);
        //Debug.Log("otherWeightedPercent: " + otherWeightedPercent);
        //Debug.Log("simpleWeightedPercent: " + simpleWeightedPercent);



        if (patternCompleted && dist < agroRange)
        {
            playersLastPosition = player.transform.position;
            //Debug.Log("proper distance");

            int random = Random.Range(1, 100);
            //patterns that are farther
            if (random <= simpleWeightedPercent)
            {
                //Debug.Log("farther away");
                //if player is on left side of the enemy
                if (player.transform.position.x < transform.position.x)
                {
                    //Debug.Log("player is on left 1");
                    int leftrightRandom = Random.Range(1, 100);
                    //player is to the left
                    if (leftrightRandom <= 75)
                    {
                        farLeftPatterns();
                    }
                    //player is to the right 
                    else
                    {
                        farRightPatterns();
                    }
                }
                else
                {
                    //Debug.Log("player is on right 1");
                    int leftrightRandom = Random.Range(1, 100);
                    //player is to the right
                    if (leftrightRandom <= 75)
                    {
                        farRightPatterns();
                    }
                    //player is to the left 
                    else
                    {
                        farLeftPatterns();
                    }
                }
            }
            //patterns that are closer away
            else
            {
                //Debug.Log("closer");
                //if player is on left side of the enemy
                if (player.transform.position.x < transform.position.x)
                {
                    //Debug.Log("player is on left 2");
                    int leftrightRandom = Random.Range(1, 100);
                    //player is to the left
                    if (leftrightRandom <= 75)
                    {
                        closeLeftPatterns();
                    }
                    //player is to the right 
                    else
                    {
                        closeRightPatterns();
                    }
                }
                else
                {
                    //Debug.Log("player is on right 2");
                    int leftrightRandom = Random.Range(1, 100);
                    //player is to the right
                    if (leftrightRandom <= 75)
                    {
                        closeRightPatterns();
                    }
                    //player is to the left 
                    else
                    {
                        closeLeftPatterns();
                    }
                }
            }

        }
    }

    public virtual void farLeftPatterns()
    {
 
    }

    public virtual void closeLeftPatterns()
    {

    }

    public virtual void farRightPatterns()
    {

    }

    public virtual void closeRightPatterns()
    {

    }


    public void completePattern()
    {
        movement.velocity.x = 0;
        patternCompleted = true;
    }

    bool facingRight = false;
    public void checkSpriteDirection(bool direction)
    {
        if (direction != facingRight)
        {
            Vector3 charscale = transform.localScale;
            charscale.x *= -1;
            transform.localScale = charscale;
            facingRight = !facingRight;
        }
    }

    public void moveX(float x)
    {
        movement.velocity.x = x;

        checkSpriteDirection(x > 0);
    }
}
