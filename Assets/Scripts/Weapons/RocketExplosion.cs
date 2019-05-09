using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    public int damage;
    public int rocketChainExplosion;
    List<Transform> hitList;

    void Awake()
    {
        
    }

    void Start()
    {
        hitList = new List<Transform>();
        Destroy(gameObject, .5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!hitList.Contains(other.transform))
            {
                EnemyStats stats = other.GetComponent<EnemyStats>();
                stats.dealDamage(damage);
                if (Random.Range(0, 99) < rocketChainExplosion)
                {
                    Instantiate(transform.gameObject, transform.position, Quaternion.identity);
                }
                hitList.Add(other.transform);
            }
        }
    }
}
