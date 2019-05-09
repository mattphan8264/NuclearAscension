using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void instantiateTree()
    {
        container.instantiateTrees(2);
        setUpTree();
    }


    void Start()
    {
        damage = 7;
        bulletSpeed = 14;
        attackRate = -.3f;
    }

    public override void loadResources()
    {
        bullet = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/PistolBullet", typeof(GameObject));
        myBullet = bullet.GetComponent<PistolBullet>();

        Sprite temp = Resources.Load<Sprite>("Images/Weapons/PistolBuff1");
        Transform t = Instantiate(BuffPrefab, transform.position, Quaternion.identity).transform;
        buffIcon1 = t.gameObject.GetComponent<PlayerBuff>();
        buffIcon1.buffImage.sprite = temp;
        myUI.addBuffChild(t);
    }


    SkillTreeNode[] soulAbsorbPath;
    SkillTreeNode[] multiShotPath;
    SkillTreeUI soulAbsorbTree;
    SkillTreeUI multiShotTree;


    void setUpTree()
    {
        soulAbsorbTree = container.gameObject.AddComponent<SkillTreeUI>();
        multiShotTree = container.gameObject.AddComponent<SkillTreeUI>();

        setUpPath1();
        setUpPath2();
    }

    void setUpPath1()
    {
        soulAbsorbPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = {1,3,6};
        int[] pathPreqPoints = {1,5,5};
        int count = 0;

        text = new string[]{"Soul Absorbtion", "Chance to gain a buff that records the number of enemies you kill while the buff is active." +
        "Once the buff ends, increases your damage for a certain amount of time based on number of enemies killed"};
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        text = new string[]{"Absorbtion Duration", "Increases the amount of time you get to absorb souls"};
        values = new float[] { 10 };
        scale = new float[] { 2 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        text = new string[]{"Soul Damage Buff Duration", "Increases the amount of time the damage buff lasts"};
        values = new float[] { 10 };
        scale = new float[] { 2 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        text = new string[]{"Soul Damage Buff Absorb Amount", "Increases the amount of damage gained for each enemy killed but decreases the buff duration"};
        values = new float[] { 5 };
        scale = new float[] { 2, 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        text = new string[]{"Soul Damage Buff Crit Rate", "Increases your crit rate based on enemies killed for the buff"};
        values = new float[] { 5 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        text = new string[]{"Soul Damage Buff Crit Damage", "Increases your crit damage based on enemies killed for the buff"};
        values = new float[] { 5 };
        scale = new float[] { .04f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, soulAbsorbPath);

        addTree(0, soulAbsorbTree, soulAbsorbPath, pathIndex, pathPreqPoints);
    }

    void setUpPath2()
    {
        multiShotPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = {1,3,6};
        int[] pathPreqPoints = {1,5,5};
        int count = 0;

        text = new string[]{"Multi-Shot", "Chance to shoot two bullets at once. Bullets fire in the same direction with only their height being slightly different"
        + "Each bullet only does half damage, but both have the same chance to crit"};
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        text = new string[]{"Multi-Shot Chance Boost", "Increases the chance of a multi-shot"};
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        text = new string[]{"Multi-Shot Crit Rate", "Increases the critical rate of a multi-shot bullet"};
        values = new float[] { 5 };
        scale = new float[] { 2 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        text = new string[]{"Multi-Shot Bullet Count", "Increases the number of bullets shot when multi-shot occurs. Damage is divided evenly across all bullets"};
        values = new float[] { 5 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        text = new string[]{"Multi-Shot Crit Damage", "Increases the crit damage of a multi-shot bullet"};
        values = new float[] { 5 };
        scale = new float[] { .1f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        text = new string[]{"Pistol Damage Boost", "Increases the base damage of this weapon's bullets"};
        values = new float[] { 10 };
        scale = new float[] { 5 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, multiShotPath);

        addTree(1, multiShotTree, multiShotPath, pathIndex, pathPreqPoints);
    }


    //Level Up variables
    int multiShotDamageBoost;
    


    //Ability variables
    bool soulAbsorbActive;
    int soulAbsorbKillCount;
    int soulAbsorbTimer;
    int soulAbsorbDamage;
    int soulAbsorbCritRate;
    float soulAbsorbCritDamage;
    int damageBoost;

    public override void updateWeaponStats()
    {
        multiShotDamageBoost = (int)multiShotPath[5].value[0];

        if (damageBoost != 0)
        {
            damage -= damageBoost;
            damageBoost = damagePercent(multiShotDamageBoost);
            damage += damageBoost;
        }
        else
        {
            damageBoost = damagePercent(multiShotDamageBoost);
            damage += damageBoost;
        }
    }

    void soulAbsorbBuff()
    {
        if (!soulAbsorbActive)
        {
            soulAbsorbActive = true;
            soulAbsorbTimer = 15 + (int)soulAbsorbPath[1].value[0] - (int)soulAbsorbPath[3].value[1];
            buffIcon1.gameObject.SetActive(true);
            buffIcon1.updateBuff(soulAbsorbTimer);
            soulAbsorbKillCount = myStats.enemiesKilled;
            Invoke("soulAbsorbBuffExpire", 1);
        }
    }
    void soulAbsorbBuffExpire()
    {
        buffIcon1.buffTime.text = "" + soulAbsorbTimer--;
        if (soulAbsorbTimer <= 0)
        {
            soulAbsorbKillCount = myStats.enemiesKilled - soulAbsorbTimer;
            soulAbsorbDamage = damagePercent(soulAbsorbKillCount * soulAbsorbPath[3].value[0]);
            soulAbsorbCritRate = (int)soulAbsorbPath[4].value[0];
            soulAbsorbCritDamage = (int)soulAbsorbPath[5].value[0];
            
            myStats.damage += soulAbsorbDamage;
            myStats.critRate += soulAbsorbCritRate;
            myStats.critDamage += soulAbsorbCritDamage;

            soulAbsorbTimer = 15 + (int)soulAbsorbPath[2].value[0] - (int)soulAbsorbPath[3].value[1];
            buffIcon1.updateBuff(soulAbsorbTimer);
            Invoke("soulAbsorbBuffEnd", 1);
        }
        else
        {
            Invoke("soulAbsorbBuffExpire", 1);
        }
    }
    void soulAbsorbBuffEnd()
    {
        buffIcon1.updateBuff(--soulAbsorbTimer);
        if (soulAbsorbTimer <= 0)
        {
            myStats.damage -= soulAbsorbDamage;
            myStats.critRate -= soulAbsorbCritRate;
            myStats.critDamage -= soulAbsorbCritDamage;

            buffIcon1.gameObject.SetActive(false);
            soulAbsorbActive = false;
        }
        else
        {
            Invoke("soulAbsorbBuffEnd", 1);
        }
    }
    
    public override void abilityOneCalculation(Vector2 spawnLocation)
    {
        if (soulAbsorbPath[0].currentLevel > 0)
        {
            if (roll(15))
            {
                soulAbsorbBuff();
            }
        }

        if (multiShotPath[0].currentLevel > 0)
        {
            if (roll(5 + (int)multiShotPath[1].value[0]))
            {

                for (int i = 0; i < 1 + (int)multiShotPath[3].value[0]; i++)
                {
                    myBullet.damage = (bulletDamage(0, (int)multiShotPath[2].value[0], (int)multiShotPath[4].value[0]) / (int)multiShotPath[3].value[0]);
                    Instantiate(myBullet, spawnLocation + new Vector2(0, (i + 1) * .02f), Quaternion.identity);
                }
            }
        }

        myBullet.damage = bulletDamage(0, 0, 0);
    }

}
