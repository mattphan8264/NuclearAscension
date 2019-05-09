using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher: Weapon
{
    RocketBullet rocketBullet;
    public override void instantiateTree()
    {
        container.instantiateTrees(2);
        setUpTree();
    }

    void Start()
    {
        damage = 5;
        bulletSpeed = 1;
        attackRate = .5f;
    }

    public override void loadResources()
    {
        bullet = (GameObject)Resources.Load("Prefabs/WeaponPrefabs/RocketBullet", typeof(GameObject));
        myBullet = bullet.GetComponent<RocketBullet>();
        rocketBullet = bullet.GetComponent<RocketBullet>();

        rocketBullet.loadResources();
    }

    SkillTreeNode[] explosionPath;
    SkillTreeNode[] homingPath;
    SkillTreeUI explosionTree;
    SkillTreeUI homingTree;

    void setUpTree()
    {
        explosionTree = container.gameObject.AddComponent<SkillTreeUI>();
        homingTree = container.gameObject.AddComponent<SkillTreeUI>();

        setUpPath1();
        setUpPath2();
    }

    void setUpPath1()
    {
        explosionPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = {1,3,6};
        int[] pathPreqPoints = {1,5,5};
        int count = 0;

        text = new string[]{"Rocket Explosion", "Gives rockets an explosion that damages multiple enemies."};
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        text = new string[]{"Rocket Explosion Radius", "Increases the explosion radius of each rocket"};
        values = new float[] { 10 };
        scale = new float[] { .2f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        text = new string[]{"Rocket Explosion Concentration", "Decreases rocket explosion radius but increases damage"};
        values = new float[] { 10 };
        scale = new float[] { 3, .1f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        text = new string[]{"Rocket Hit Booster", "Increases the damage the explosion does based on the number of enemies hit"};
        values = new float[] { 5 };
        scale = new float[] { 1, 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        text = new string[]{"Rocket Chain Explosion", "Decreases explosion damage, but gives a chance to create another explosion on enemy hit"};
        values = new float[] { 5 };
        scale = new float[] { 3, };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        text = new string[]{"Rocket Speed Booster", "Increases the speed at which the rocket flies"};
        values = new float[] { 5 };
        scale = new float[] { .15f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, explosionPath);

        addTree(0, explosionTree, explosionPath, pathIndex, pathPreqPoints);
    }

    void setUpPath2()
    {
        homingPath = new SkillTreeNode[6];
        string[] text = new string[0];
        float[] values = new float[0];
        float[] scale = new float[0];
        int[] pathIndex = {1,3,6};
        int[] pathPreqPoints = {1,5,5};
        int count = 0;

        text = new string[]{"Homing capabilities", "Gives rockets the ability to home in on enemies (Rockets don't explode unless explosion buff is active)."};
        values = new float[] { 1 };
        scale = new float[] { 1 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        text = new string[]{"Rocket Speed", "Increases the speed of each rocket. Rocket gains more damage the farther it flies"};
        values = new float[] { 10 };
        scale = new float[] { .1f, 3 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        text = new string[]{"Rocket Homing Radius", "Increases the area a rocket can detect and home in on an enemy"};
        values = new float[] { 10 };
        scale = new float[] { .2f };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        text = new string[]{"Second Shot", "Gives a chance to fire two rockets at once. The second does more damage"};
        values = new float[] { 5 };
        scale = new float[] { 4, 5 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        text = new string[]{"Ground Fire", "On contact with an enemy, chance to spawn fire that falls to the ground"};
        values = new float[] { 5 };
        scale = new float[] { 4, };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        text = new string[]{"Rocket Crit Boost", "Increases the base crit damage and crit rate of rockets"};
        values = new float[] { 5 };
        scale = new float[] { .15f, 4 };
        addNode(text, values, scale, Resources.Load<Sprite>("Images/Weapons/PistolBuff0"), count++, homingPath);

        addTree(1, homingTree, homingPath, pathIndex, pathPreqPoints);
    }

    int secondShotChance;
    public override void updateWeaponStats()
    {
        rocketBullet.explosion = explosionPath[0].currentLevel > 0;
        rocketBullet.explosionRadius = 1 + explosionPath[1].value[0] - explosionPath[2].value[1];
        rocketBullet.myExplosion.rocketChainExplosion = (int)explosionPath[4].value[0];
        
        rocketBullet.homing = homingPath[0].currentLevel > 0;
        if (rocketBullet.homing)
        {
            rocketBullet.myHoming.gameObject.SetActive(true);
            rocketBullet.distanceDamage = damagePercent(homingPath[1].value[1]);
            rocketBullet.myHoming.homingColliderRadius = 1 + homingPath[2].value[0];
            secondShotChance = (int)homingPath[3].value[0];
        }
        else
        {
            rocketBullet.myHoming.gameObject.SetActive(false);
        }
        
        rocketBullet.groundFireChance = (int)homingPath[4].value[0];

        myBullet.direction *= 1 + explosionPath[5].value[0] + homingPath[1].value[0];
    }

    public override void abilityOneCalculation(Vector2 spawnLocation)
    {
        rocketBullet.hitBooster = (int)explosionPath[3].value[0];
        rocketBullet.damageHitBooster = damagePercent(explosionPath[3].value[1]);

        myBullet.damage = bulletDamage(0, (int)homingPath[5].value[1], homingPath[5].value[0]);
        if (roll(secondShotChance))
        {
            Instantiate(myBullet, spawnLocation, Quaternion.identity);
        }
    }
}