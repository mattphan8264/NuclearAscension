using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : Bullet {
    public RocketHomingComponent myHoming;
    public RocketExplosion myExplosion;
    public GameObject groundFire;
    public RocketGroundFire rocketGroundFire;
    public int groundFireChance;

    public bool explosion;
    public float explosionRadius;
    public int hitBooster;
    public int damageHitBooster;
    public bool homing;
    public Transform homeTarget;

    public int distanceDamage;
    Vector2 myPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        myExplosion.transform.localScale = new Vector3(1, 1, 1);
        myExplosion.transform.localScale *= explosionRadius;
        hitList = new List<Transform>();
    }

    public void loadResources()
    {
        GameObject temp = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/RocketExplosion", typeof(GameObject));
        myExplosion = temp.GetComponent<RocketExplosion>();
        groundFire = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/RocketFire", typeof(GameObject));
        rocketGroundFire = groundFire.GetComponentInChildren<RocketGroundFire>();
        
        myHoming = GetComponentInChildren<RocketHomingComponent>();
        myHoming.gameObject.SetActive(false);
    }

    void Start()
    {
        myPosition = transform.position;
    }

    void Update()
    {
        if (homeTarget != null && homing)
        {
            rb.velocity = Vector2.MoveTowards(transform.position, homeTarget.position, direction.magnitude);
            transform.right = rb.velocity;
        }
        else
        {
            rb.velocity = direction;
            transform.right = direction;
        }
        
        if (direction.magnitude < 18)
        {
            direction *= 1.1f;
        }
    }

    public override void dealDamage()
    {
        damage += (int)(Vector2.Distance(myPosition, transform.position) / 4 * distanceDamage);

        enemy.dealDamage(damage);
        if (explosion)
        {
            myExplosion.damage = damage;
            Instantiate(myExplosion, transform.position, Quaternion.identity);
        }
        
        if (Random.Range(0, 99) < groundFireChance)
        {
            rocketGroundFire.damage = damage;
            Instantiate(groundFire, transform.position, Quaternion.identity);
        }
        destroy();
    }

}