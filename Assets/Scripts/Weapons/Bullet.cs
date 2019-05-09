using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public Vector2 direction;
    public Rigidbody2D rb;
    public float duration;
    public EnemyStats enemy;
    public bool isBoss;
    public List<Transform> hitList;
    bool hitWall;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitList = new List<Transform>();
        hitWall = false;
    }

    void Update()
    {
        rb.velocity = direction;
        transform.right = direction;

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (!hitList.Contains(other.transform))
            {
                enemy = other.GetComponent<BossStats>();
                if (enemy == null)
                {
                    enemy = other.GetComponent<EnemyStats>();
                }
                else
                {
                    isBoss = true;
                }

                hitList.Add(other.transform);
                dealDamage();
            }
        }

        if (other.CompareTag("Ground"))
        {
            WallBreak temp = other.GetComponent<WallBreak>();
            if (temp != null && GlobalStats.breakWall && !hitWall)
            {
                temp.breakWall();
                hitWall = true;
            }
            destroy();
        }
    }

    public virtual void dealDamage()
    {

    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}