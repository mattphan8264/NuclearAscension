using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    Vector2 spawnPos;
    public bool canSpawn;
    LayerMask collisionMask;
    Transform player;

    IEnumerator Start()
    {
        spawnPos = transform.position += new Vector3(0, 1);
        canSpawn = false;

        collisionMask = LayerMask.GetMask("Ground");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPos, 1, collisionMask);

        if (!hit)
        {
            yield return new WaitForSeconds(5);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Invoke("spawnEnemy", GlobalStats.spawnRate);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {

    }


    float dist;
    void spawnEnemy()
    {
        dist = Vector2.Distance(player.position, spawnPos);
        if (dist <= 10)
        {
            GameObject enemy = EnemyList.getEnemy();
            if (enemy != null)
            {

                RaycastHit2D hit = Physics2D.Raycast(spawnPos, (player.position - (Vector3)spawnPos).normalized, dist, collisionMask);

                if (!hit)
                {
                    Instantiate(enemy, spawnPos, Quaternion.identity);
                    Invoke("spawnEnemy", GlobalStats.spawnRate);
                }
                else
                {
                    Invoke("spawnEnemy", 1);
                }

            }
        }
    }
}
