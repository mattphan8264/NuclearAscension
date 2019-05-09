using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    RifleBullet rifleBullet;
    public override void instantiateTree()
    {
        container.instantiateTrees(2);
        setUpTree();
    }


    void Start()
    {
        damage = 10;
        bulletSpeed = 16;
        attackRate = 0;
    }

    public override void loadResources()
    {
        bullet = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/RifleBullet", typeof(GameObject));
        myBullet = bullet.GetComponent<RifleBullet>();
        rifleBullet = bullet.GetComponent<RifleBullet>();

        Sprite temp = Resources.Load<Sprite>("Images/Weapons/RifleBuff1");
        Transform t = Instantiate(BuffPrefab, transform.position, Quaternion.identity).transform;
        buffIcon1 = t.gameObject.GetComponent<PlayerBuff>();
        buffIcon1.buffImage.sprite = temp;
        myUI.addBuffChild(t);

        temp = Resources.Load<Sprite>("Images/Weapons/RifleBuff1");
        t = Instantiate(BuffPrefab, transform.position, Quaternion.identity).transform;
        buffIcon2 = t.gameObject.GetComponent<PlayerBuff>();
        buffIcon2.buffImage.sprite = temp;
        myUI.addBuffChild(t);
    }


    SkillTreeNode[] bulletPenetrationPath;
    SkillTreeNode[] rifleBoosterPath;
    SkillTreeUI bulletPenetrationTree;
    SkillTreeUI rifleBoosterTree;

    void setUpTree()
    {
        bulletPenetrationTree = container.gameObject.AddComponent<SkillTreeUI>();
        rifleBoosterTree = container.gameObject.AddComponent<SkillTreeUI>();

        setUpPath1();
        setUpPath2();
    }

    void setUpPath1()
    {
        bulletPenetrationPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = { 1, 3, 6 };
        int[] pathPreqPoints = { 1, 5, 5 };
        int count = 0;

        text = new string[] { "Bullet Penetration", "Chance to fire a bullet that penetrates an enemy and hit another before dissapearing" };
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        text = new string[] { "Penetration Hit Up", "Increases the max number of enemies a bullet can pierce through" };
        values = new float[] { 5 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        text = new string[] { "Penetration Crit Rate", "If the bullet penetrates, increases crit rate for that bullet" };
        values = new float[] { 10 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        text = new string[] { "Bullet Penetration Booster", "Grants a buff that inreases attack, crit damage, crit rate by a small amount on firing a penetration bullet" };
        values = new float[] { 5 };
        scale = new float[] { 5, .05f, 2, 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        text = new string[] { "Bullet Crit Penetration Damage", "Increases the damage of the bullet for every monster it hits via crit damage" };
        values = new float[] { 10 };
        scale = new float[] { .1f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        text = new string[] { "Fast Reload", "Chance to cancel attack cooldown when firing a penetration shot" };
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bulletPenetrationPath);

        addTree(0, bulletPenetrationTree, bulletPenetrationPath, pathIndex, pathPreqPoints);
    }


    void setUpPath2()
    {
        rifleBoosterPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = { 1, 3, 6 };
        int[] pathPreqPoints = { 1, 5, 5 };
        int count = 0;

        text = new string[] { "Rifle Booster Buff", "Chance to give a buff on firing that increases damage" };
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        text = new string[] { "Crit Rate Booster", "Increase crit rate when the Rifle Booster Buff is active" };
        values = new float[] { 10 };
        scale = new float[] { 2 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        text = new string[] { "Move Speed Booster", "Gain move speed when the buff is active" };
        values = new float[] { 10 };
        scale = new float[] { .05f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        text = new string[] { "Crit Damage Booster", "Increase crit damage when the Rifle Booster Buff is active" };
        values = new float[] { 10 };
        scale = new float[] { .01f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        text = new string[] { "Attack Speed Booster", "Increases attack speed when the Rifle Booster Buff is active" };
        values = new float[] { 10 };
        scale = new float[] { .05f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        text = new string[] { "Rifle Booster Buff Chance Boost", "Increases the chance of activating the rifle booster buff" };
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, rifleBoosterPath);

        addTree(1, rifleBoosterTree, rifleBoosterPath, pathIndex, pathPreqPoints);
    }


    //Ability variables
    bool penetrationBooster;
    bool penetrationBoosterActive;
    int penetrationBoosterTimer;
    int penetrationBoosterAtk, penetrationBoosterCritRate;
    float penetrationBoosterCritDamage;
    int tempCritRate;
    float tempCritDamage;
    bool reload;
    int rifleBoosterTimer;
    bool rifleBooster;
    bool rifleBoosterActive;
    int rifleBoosterCritRate;
    float rifleBoosterMoveSpeed;
    float rifleBoosterCritDamage;
    float rifleBoosterAttackSpeed;
    int rifleBoosterBuffChance;

    public override void updateWeaponStats()
    {
        reload = bulletPenetrationPath[5].currentLevel > 0;
        tempCritRate = (int)bulletPenetrationPath[2].value[0];
        tempCritDamage = bulletPenetrationPath[4].value[0];
        penetrationBooster = bulletPenetrationPath[0].currentLevel > 0;
        rifleBooster = rifleBoosterPath[0].currentLevel > 0;
        rifleBoosterBuffChance = (int)rifleBoosterPath[5].value[0];
    }

    public override void abilityOneCalculation(Vector2 spawnLocation)
    {
        if (rifleBooster)
        {
            if (roll(5 + rifleBoosterBuffChance))
            {
                rifleBoosterBuff();
            }
        }

        if (penetrationBooster)
        {
            if (roll(10))
            {
                penetrateBullet();
            }
            else
            {
                rifleBullet.hitCount = 2;
                myBullet.damage = bulletDamage(0, 0, 0);
            }
        }
        else
        {
            myBullet.damage = bulletDamage(0, 0, 0);
        }
    }

    public void penetrateBullet()
    {
        rifleBullet.hitCount = 2 + (int)bulletPenetrationPath[1].value[0];
        rifleBullet.critDamage = tempCritDamage;

        penetrationBoosterBuff();

        myBullet.damage = bulletDamage(0, tempCritRate, tempCritDamage);
        Instantiate(myBullet, transform.position, Quaternion.identity);

        if (reload && roll(bulletPenetrationPath[5].value[0]))
        {
            attackSpeedMultiplier = 0;
        }
        else
        {
            attackSpeedMultiplier = 1;
        }
    }

    void penetrationBoosterBuff()
    {
        if (!penetrationBoosterActive)
        {
            penetrationBoosterActive = true;
            penetrationBoosterTimer = 5 + (int)bulletPenetrationPath[3].value[3];

            penetrationBoosterAtk = damagePercent(bulletPenetrationPath[3].value[0]);
            penetrationBoosterCritDamage = bulletPenetrationPath[3].value[1];
            penetrationBoosterCritRate = (int)bulletPenetrationPath[3].value[2];

            damage += penetrationBoosterAtk;
            critDamage += penetrationBoosterCritDamage;
            critRate += penetrationBoosterCritRate;

            buffIcon1.gameObject.SetActive(true);
            buffIcon1.updateBuff(penetrationBoosterTimer);

            Invoke("penetrationBoosterBuffEnd", 1);
        }
    }

    void penetrationBoosterBuffEnd()
    {
        buffIcon1.updateBuff(--penetrationBoosterTimer);
        if (penetrationBoosterTimer <= 0)
        {
            penetrationBoosterActive = false;
            damage -= penetrationBoosterAtk;
            critDamage -= penetrationBoosterCritDamage;
            critRate -= penetrationBoosterCritRate;

            buffIcon1.gameObject.SetActive(false);
        }
        else
        {
            Invoke("penetrationBoosterBuffEnd", 1);
        }
    }

    void rifleBoosterBuff()
    {
        if (!rifleBoosterActive)
        {
            rifleBoosterCritRate = (int)rifleBoosterPath[1].value[0];
            rifleBoosterMoveSpeed = rifleBoosterPath[2].value[0];
            rifleBoosterCritDamage = rifleBoosterPath[3].value[0];
            rifleBoosterAttackSpeed = rifleBoosterPath[4].value[0];

            rifleBoosterActive = true;
            rifleBoosterTimer = 10;

            myStats.critRate += rifleBoosterCritRate;
            myStats.critDamage += rifleBoosterCritDamage;
            myStats.speed += rifleBoosterMoveSpeed;
            myStats.attackRate -= rifleBoosterAttackSpeed;

            buffIcon2.gameObject.SetActive(true);
            buffIcon2.updateBuff(rifleBoosterTimer);

            Invoke("rifleBoosterBuffEnd", 1);
        }
    }

    void rifleBoosterBuffEnd()
    {
        buffIcon2.updateBuff(--rifleBoosterTimer);
        if (rifleBoosterTimer <= 0)
        {
            myStats.critRate -= rifleBoosterCritRate;
            myStats.critDamage -= rifleBoosterCritDamage;
            myStats.speed -= rifleBoosterMoveSpeed;
            myStats.attackRate += rifleBoosterAttackSpeed;

            rifleBoosterActive = false;
            buffIcon2.gameObject.SetActive(false);
        }
        else
        {
            buffIcon2.updateBuff(rifleBoosterTimer);
            Invoke("rifleBoosterBuffEnd", 1);
        }
    }
}

