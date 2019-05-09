using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPremade : MonoBehaviour
{

    Vector2 spawnPos;
    public bool canSpawn;
    LayerMask collisionMask;
    Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        collisionMask = LayerMask.GetMask("Player");
    }

    void Start()
    {
        Invoke("spawnEnemy", GlobalStats.spawnRate);
        spawnPos = transform.position += new Vector3(0, 1);
        canSpawn = false;
    }


    void Update()
    {

    }


    float dist;
    void spawnEnemy()
    {
        if (canSpawn)
        {
            dist = Vector2.Distance(player.position, spawnPos);
            RaycastHit2D hit = Physics2D.Raycast(spawnPos, (player.position - (Vector3)spawnPos).normalized, dist, collisionMask);

            if (!hit)
            {
                GameObject enemy = EnemyList.getEnemy();
                if (enemy != null)
                {
                    Instantiate(enemy, spawnPos, Quaternion.identity);
                    Invoke("spawnEnemy", GlobalStats.spawnRate);
                }
            }
            else
            {
                Invoke("spawnEnemy", 1);
            }
        }
        else
        {
            Invoke("spawnEnemy", 1);
        }

    }
}