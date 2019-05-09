using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public delegate void blockMovement(bool input, int weaponNumber);
    public blockMovement blockMove;
    public delegate void blockAction(bool input, int weaponNumber);
    public blockAction blockAct;
    public int weaponNumber;

    public PlayerStats myStats;
    public PlayerUI myUI;
    public PlayerControls myControls;
    public Rigidbody2D rb;
    public SkillTreeContainer container;
    public GameObject BuffPrefab;
    public GameObject bullet;
    public GameObject button;
    public Bullet myBullet;
    public PlayerBuff buffIcon1;
    public PlayerBuff buffIcon2;
    public PlayerBuff buffIcon3;


    public int damage;
    public float attackTime;
    public float attackRate;
    public int critRate;
    public float critDamage;
    public float bulletSpeed;
    public float attackSpeedMultiplier;

    void Awake()
    {
        myStats = GetComponent<PlayerStats>();
        myControls = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody2D>();
        myUI = GetComponentInChildren<PlayerUI>();
        BuffPrefab = (GameObject)Resources.Load("Prefabs/PlayerBuff", typeof(GameObject));
        button = (GameObject)Resources.Load("Prefabs/SkillTreeIcon", typeof(GameObject));
        attackSpeedMultiplier = 1;
        loadResources();
    }

    public virtual void loadResources()
    {

    }

    public bool roll(float value)
    {
        return Random.Range(0, 99) < (int)value;
    }

    public virtual void abilityOne(Vector2 Direction, Vector2 spawnLocation)
    {
        if (Time.time >= attackTime || attackTime == 0)
        {
            myBullet.direction = Direction * bulletSpeed;
            abilityOneCalculation(spawnLocation);
            attackTime = abilityOneAttackRate();
            Instantiate(myBullet, spawnLocation, Quaternion.identity);
        }
    }

    public int damagePercent(float percent)
    {
        return (int) (myStats.damage * percent);
    }

    public virtual void abilityOneCalculation(Vector2 spawnLocation)
    {
        
    }

    public virtual void abilityTwo(Vector2 Direction)
    {

    }

    public virtual void abilityThree(Vector2 Direction)
    {

    }

    public virtual void assignContainer(SkillTreeContainer container)
    {
        this.container = container;
    }

    public virtual void instantiateTree()
    {
        
    }

    public virtual void updateWeaponStats()
    {

    }

    public virtual float abilityOneAttackRate()
    {
        float temp = myStats.attackRate + attackRate;
        if (temp < .1f)
        {
            temp = .1f;
        }
        temp *= attackSpeedMultiplier;
        temp += Time.time;
        return temp;
    }

    public int bulletDamage(int damage, int critRate, float critDamage)
    {
        int retVal = myStats.damage + damage + this.damage;

        int rand = Random.Range(0, 99);
        if (rand < myStats.critRate + critRate + this.critRate)
        {
            retVal = (int)(rand * (myStats.critDamage + critDamage + this.critDamage));
        }

        return retVal;
    }

    public void addNode(string[] text, float[] values, float[] scale, Sprite sprite, int count, SkillTreeNode[] array)
    {
        SkillTreeNode temp = Instantiate(button, new Vector2(), Quaternion.identity).GetComponent<SkillTreeNode>();
        array[count] = temp;
        temp.setValues(text, values, scale, sprite);
        temp.transform.SetParent(container.transform, true);
    }

    public void addTree(int i, SkillTreeUI input, SkillTreeNode[] list, int[] listIndex, int[] preq)
    {
        container.trees[i] = input;
        container.trees[i].updateButtonPosition(list, listIndex, preq, i);
    }

}