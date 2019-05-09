using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    SniperBullet sniperBullet;

    public override void instantiateTree()
    {
        container.instantiateTrees(2);
        setUpTree();
    }

    void Start()
    {
        damage = 11;
        bulletSpeed = 23;
        attackRate = .3f;
    }

    public override void loadResources()
    {
        bullet = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/SniperBullet", typeof(GameObject));
        myBullet = bullet.GetComponent<SniperBullet>();
        sniperBullet = bullet.GetComponent<SniperBullet>();

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

        temp = Resources.Load<Sprite>("Images/Weapons/RifleBuff1");
        t = Instantiate(BuffPrefab, transform.position, Quaternion.identity).transform;
        buffIcon3 = t.gameObject.GetComponent<PlayerBuff>();
        buffIcon3.buffImage.sprite = temp;
        myUI.addBuffChild(t);
    }

    SkillTreeNode[] scopedPath;
    SkillTreeNode[] bossKillerPath;
    SkillTreeUI scopedTree;
    SkillTreeUI bossKillerTree;

    void setUpTree()
    {
        scopedTree = container.gameObject.AddComponent<SkillTreeUI>();
        bossKillerTree = container.gameObject.AddComponent<SkillTreeUI>();

        setUpPath1();
        setUpPath2();
    }

    void setUpPath1()
    {
        scopedPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = { 1, 3, 6 };
        int[] pathPreqPoints = { 1, 5, 5 };
        int count = 0;

        text = new string[] { "Scope", "Stand still when firing to increase bullet speed and increase bullet damage" };
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        text = new string[] { "Scope Damage", "Increases the amount of damage you gain while shooting" };
        values = new float[] { 10 };
        scale = new float[] { 4 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        text = new string[] { "Scope Crit Rate", "Increases the chance for a bullet to crit while shooting" };
        values = new float[] { 10 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        text = new string[] { "Multi-tasking", "Allows movement while shooting but decreases movement speed" };
        values = new float[] { 5 };
        scale = new float[] { .15f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        text = new string[] { "Quick Reload", "Chance to skip half of the cool down for attacking for the sniper weapon" };
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        text = new string[] { "Bullet Speed Booster", "Chance to make bullets fly faster. If this occurs, increases bullet damage based on distance traveled" };
        values = new float[] { 5 };
        scale = new float[] { 2 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, scopedPath);

        addTree(0, scopedTree, scopedPath, pathIndex, pathPreqPoints);
    }

    void setUpPath2()
    {
        bossKillerPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = { 1, 3, 6 };
        int[] pathPreqPoints = { 1, 5, 5 };
        int count = 0;

        text = new string[] { "Boss Killer", "Increases damage against bosses" };
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        text = new string[] { "Boss Hitter", "Upon hitting a boss, chance to gain a buff that boosts damage" };
        values = new float[] { 10 };
        scale = new float[] { 2, 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        text = new string[] { "Attack Speed Booster", "Upon hitting a boss, chance to gain an attack speed boost" };
        values = new float[] { 10 };
        scale = new float[] { 2, .025f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        text = new string[] { "Boss Killer Up", "Increases damage against bosses" };
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        text = new string[] { "Killer Bullet", "Chance to shoot a bullet that deals double damage to a boss (on top of other boosts)" };
        values = new float[] { 5 };
        scale = new float[] { 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        text = new string[] { "Move Speed Boost", "Chance to increase move speed for a short time upon hitting a boss" };
        values = new float[] { 5 };
        scale = new float[] { 2, .1f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/RifleBuff0"), count++, bossKillerPath);

        addTree(1, bossKillerTree, bossKillerPath, pathIndex, pathPreqPoints);
    }

    bool scope;
    bool multiTasking;
    float multiTaskingSpeed;
    int scopeDamage;
    int scopeCritRate;
    bool bossKiller;
    int bossHitterBuffTimer;
    bool bossHitterBuffActive;
    int bossHitterDamage;
    int attackSpeedBoosterTimer;
    bool attackSpeedBoosterActive;
    float attackSpeedBoosterAmount;
    int moveSpeedBoostTimer;
    bool moveSpeedBoostActive;
    float moveSpeedBoostAmount;
    bool scopedActivated;

    public override void updateWeaponStats()
    {
        scope = scopedPath[0].currentLevel > 0;
        multiTasking = scopedPath[3].currentLevel > 0;
        scopeDamage = damagePercent(scopedPath[1].value[0]);
        scopeCritRate = (int)scopedPath[2].value[0];
        bossKiller = bossKillerPath[0].currentLevel > 0;
    }

    public int bossKillerCalc(int damage)
    {
        damage += damagePercent(25) + damagePercent(bossKillerPath[3].value[0]);
        if (roll(bossKillerPath[1].value[0]))
        {
            bossHitterBuffStart();
        }
        if (roll(bossKillerPath[2].value[1]))
        {
            attackSpeedBoosterStart();
        }
        if (roll(bossKillerPath[4].value[0]))
        {
            damage *= 2;
        }
        if (roll(bossKillerPath[5].value[0]))
        {

        }

        return damage;
    }

    public override void abilityOneCalculation(Vector2 spawnLocation)

    {
        if (bossKiller)
        {
            sniperBullet.onBossHit = bossKillerCalc;
        }

        if (scope)
        {
            if (!scopedActivated)
            {
                scopedActivated = true;
                if (!multiTasking)
                {
                    myStats.weaponMovingDisable(false, weaponNumber);
                }
                else
                {
                    multiTaskingSpeed = myStats.speed * (1 - scopedPath[3].value[0]);
                    myStats.speed -= multiTaskingSpeed;
                }
            }

            if (roll(scopedPath[4].value[0]))
            {
                attackSpeedMultiplier = .5f;
            }
            else
            {
                attackSpeedMultiplier = 1;
            }

            if (roll(scopedPath[5].value[0]))
            {
                sniperBullet.speedBullet = true;
            }
            else
            {
                sniperBullet.speedBullet = false;
            }

            myBullet.damage = bulletDamage(scopeDamage, scopeCritRate, 0);

            Invoke("scopeDone", .25f);
        }
        else
        {
            myBullet.damage = bulletDamage(0, 0, 0);
        }

    }

    void scopeDone()
    {
        if (scopedActivated)
        {
            scopedActivated = false;

            if (!multiTasking)
            {
                myStats.weaponMovingDisable(true, weaponNumber);
            }
            else
            {
                myStats.speed += multiTaskingSpeed;
            }
        }
    }

    void bossHitterBuffStart()
    {
        if (!bossHitterBuffActive)
        {
            bossHitterBuffActive = true;
            bossHitterBuffTimer = 5;

            bossHitterDamage = damagePercent(bossKillerPath[1].value[1]);
            damage += bossHitterDamage;

            Invoke("bossHitterBuffEnd", 1);
        }
    }

    void bossHitterBuffEnd()
    {
        bossHitterBuffTimer--;
        if (bossHitterBuffTimer <= 0)
        {
            damage -= bossHitterDamage;
            bossHitterBuffActive = false;
        }
        else
        {
            Invoke("bossHitterBuffEnd", 1);
        }
    }

    void attackSpeedBoosterStart()
    {
        if (!attackSpeedBoosterActive)
        {
            attackSpeedBoosterActive = true;
            attackSpeedBoosterTimer = 5;

            attackSpeedBoosterAmount = bossKillerPath[2].value[1];
            attackRate -= attackSpeedBoosterAmount;

            Invoke("attackSpeedBoosterEnd", 1);
        }
    }

    void attackSpeedBoosterEnd()
    {
        attackSpeedBoosterTimer--;
        if (bossHitterBuffTimer <= 0)
        {
            attackSpeedBoosterActive = false;
            attackRate += attackSpeedBoosterAmount;
        }
        else
        {
            Invoke("attackSpeedBoosterEnd", 1);
        }
    }

    void moveSpeedBoostStart()
    {
        if (!moveSpeedBoostActive)
        {
            moveSpeedBoostActive = true;
            moveSpeedBoostTimer = 5;

            moveSpeedBoostAmount = bossKillerPath[5].value[1];
            myStats.speed += moveSpeedBoostAmount;

            Invoke("moveSpeedBoostEnd", 1);
        }
    }

    void moveSpeedBoostEnd()
    {
        moveSpeedBoostTimer--;
        if (moveSpeedBoostTimer <= 0)
        {
            moveSpeedBoostActive = false;
            myStats.speed -= moveSpeedBoostAmount;
        }
        else
        {
            Invoke("attackSpeedBoosterEnd", 1);
        }
    }
}