using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGroundFire : MonoBehaviour
{
    int duration;
    public int damage;
    List<Transform> hitList;

    void Start()
    {
        hitList = new List<Transform>();
        duration = 3;
        Destroy(gameObject, duration);
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
                EnemyStats temp = other.GetComponent<EnemyStats>();
                temp.dealDamage(damage);
                hitList.Add(other.transform);
            }
        }

    }
}
