using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTest : MonoBehaviour
{

    /*
    How to use:
    Assign an int as damage.
    Assign a vector2 as direction.
    Instantiate.
     */
    
    public int damage;
    public Vector3 direction;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, 3);
    }

    
    void Update()
    {
        rb.velocity = direction;
        transform.right = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            stats.takeDamage(damage);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
