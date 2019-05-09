using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHomingComponent : MonoBehaviour {
    public CircleCollider2D homingCollider;
    public float homingColliderRadius;
    RocketBullet myBullet;
    LayerMask collisionMask;
    void Awake()
    {
        homingCollider = GetComponent<CircleCollider2D>();
        homingCollider.radius *= homingColliderRadius;
        myBullet = GetComponentInParent<RocketBullet>();
        collisionMask = LayerMask.GetMask("Ground");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && myBullet.homeTarget == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position).normalized, homingCollider.radius, collisionMask);
            
            if (!hit)
            {
                myBullet.homeTarget = other.transform;
            }
        }
    }
}