using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStats : MonoBehaviour
{
    static int baseHealth;
    static int baseDamage;
    static int baseHealthIncrease;
    static int baseDamageIncrease;
    static int baseHealthScaling;
    static int baseDamageScaling;
    public static bool breakWall;
    public static float spawnRate;
    public static int spawnCap;


    void Awake()
    {
        baseHealth = 25;
        baseDamage = 5;
        baseHealthIncrease = 3;
        baseDamageIncrease = 1;
        baseHealthScaling = 5;
        baseDamageScaling = 2;

        breakWall = false;
        spawnRate = 3f;
        spawnCap = 5;
    }

    public static int enemyHealth()
    {
        return baseHealth;
    }

    public static int enemyDamage()
    {
        return baseDamage;
    }

    static int checkLevel()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().level % 3;
    }

    public static void leveled(int level)
    {
        if (level % 5 == 0)
        {
            baseHealthIncrease += baseHealthScaling;
            baseDamageIncrease += baseDamageScaling;
            spawnRate -= .25f;
            if (spawnCap < 20)
            {
                spawnCap += 5;
            }
        }

        baseHealth += baseHealthIncrease;
        baseDamage += baseDamageIncrease;
    }

}