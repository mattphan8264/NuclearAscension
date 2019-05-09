using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    int health;
    public int maxHealth;
    int exp;
    public int damage;
    void Start()
    {
        maxHealth = GlobalStats.enemyHealth();
        health = maxHealth;
        exp = maxHealth / 7;
        damage = GlobalStats.enemyDamage();
    }

    
    void Update()
    {
        
    }

    public void dealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayerStats temp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            temp.addExp(exp);
            temp.increaseKillCount();
            Destroy(gameObject);
        }
    }

    public void heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    } 
}
