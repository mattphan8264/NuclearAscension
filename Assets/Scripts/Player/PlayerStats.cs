using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats : MonoBehaviour {

	/*
	Class that holds all of player stats
	Partial class that also links status effects and level system
	 */
	public int health;
	public int maxHealth;
	public float speed;
	public float jumpSpeed;
	public int damage;
	public float attackRate; //1-3 where 3 is the max attack rate (attack cd / attack rate)
	public int critRate;	//0-100 where 100 means 100% crit rate
	public float critDamage;	//multiplyer for base damage
	public float statusResistance;	//0-1 where .5 represents status effects on you have 50% less duration
	public int statusDuration;	//How long you're status last on enemies (1-5x)
	public bool canMove;
	public bool canAct;
	public bool invulnerable;
	public float damageTime;
	public int defense; 	//damage taken - (damage * 0 / 1000)
	
	public int weaponOneSkillPoints;
	public int weaponTwoSkillPoints;

	public bool[] weaponMove;
    public bool[] weaponAct;
	public bool[] invulnerability;

	public PlayerUI UI;
	public PlayerMovement movement;

	void InstantiateStats()
	{
		maxHealth = 100;
		health = maxHealth;

		speed = 2f;
		jumpSpeed = .6f;

		damage = 0;
		attackRate = .75f;

		critRate = 0;
		critDamage = 1.5f;

		statusResistance = 0;
		statusDuration = 1;

		defense = 50;

		weaponOneSkillPoints = 90;
		weaponTwoSkillPoints = 1;

		canMove = false;
		canAct = false;
		invulnerable = false;

		weaponAct = new bool[]{true, true};
        weaponMove = new bool[]{true, true};
		invulnerability = new bool[]{false, false};
	}

	//Decrease health based on int damage
	public void takeDamage(int damage)
	{
		if (invulnerable || Time.time - damageTime > .1)
		{
			return;
		}
		damage = damage - (int)((defense / 1000) * damage);

		
		health -= damage;
		damageTime = Time.time;
		if (health <= 0)
		{
			health = 0;
			//Game Over
		}

		UI.updateHealth();
	}

	//Increase health based on int heal
	public void heal(int heal)
	{
		health += heal;
		if (health > maxHealth)
		{
			health = maxHealth;
		}

		UI.updateHealth();
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
		UI = GetComponentInChildren<PlayerUI>();
		movement = GetComponent<PlayerMovement>();

		InstantiateStats();
		InstantiateLevelStats();
		InstantiateStatusEffects();
		loadLevelGrowth();
	}

	public string spriteSheetName;
	void LateUpdate()
	{
		foreach (var renderer in GetComponents<SpriteRenderer>())
		{
			string spriteName = renderer.sprite.name;
			var subSprites = Resources.LoadAll<Sprite>("Images/Player/" + spriteSheetName);
			foreach (var sprite in subSprites)
			{
				if (sprite.name == spriteName)
				{
					renderer.sprite = sprite;
				}
			}
		}
	}


	//Weapon level up skills for the three different skill points
	public int weaponOneLevelUp(int input)
	{
		if (weaponOneSkillPoints >= input)
		{
			weaponOneSkillPoints -= input;
			return input;
		}
		else if (weaponOneSkillPoints > 0)
		{
			int difference = weaponOneSkillPoints;
			weaponOneSkillPoints = 0;
			return difference;
		}
		return -1;
	}
	public int weaponTwoLevelUp(int input)
	{
		if (weaponTwoSkillPoints >= input)
		{
			weaponTwoSkillPoints -= input;
			return input;
		}
		else if (weaponTwoSkillPoints > 0)
		{
			int difference = weaponTwoSkillPoints;
			weaponTwoSkillPoints = 0;
			return difference;
		}
		return -1;
	}
	
	public int getWeaponOneSkillPoints()
	{
		return weaponOneSkillPoints;
	}
	public int getWeaponTwoSkillPoints()
	{
		return weaponTwoSkillPoints;
	}

	public void weaponActingDisable(bool input, int weaponNumber)
	{
		weaponAct[weaponNumber] = input;
		checkWeaponAct();
	}
	void checkWeaponAct()
	{
		for (int i = 0; i < weaponAct.Length; i++)
		{
			if (weaponAct[i] != true)
			{
				canAct = false;
				return;
			}
		}
		canAct = true;
	}

	public void weaponMovingDisable(bool input, int weaponNumber)
	{
		weaponMove[weaponNumber] = input;
		checkWeaponMove();
	}
	void checkWeaponMove()
	{
		for (int i = 0; i < weaponMove.Length; i++)
		{
			if (weaponMove[i] != true)
			{
				canMove = false;
				movement.setVelocity(0);
				return;
			}
		}
		canMove = true;
	}

	public void setInvulnerable(bool input, int weaponNumber)
	{
		invulnerability[weaponNumber] = input;
		checkInvulnerability();
	}
	void checkInvulnerability()
	{
		for (int i = 0; i < invulnerability.Length; i++)
		{
			if (invulnerability[i] != false)
			{
				invulnerable = true;
				return;
			}
		}
		invulnerable = false;
	}

	void Start () {
		StartCoroutine(loadingStage());


	}

	IEnumerator loadingStage()
	{
		BlackFade blackScreen = GameObject.Find("MenuCanvas").GetComponentInChildren<BlackFade>();

		while (blackScreen.fadingOut == true)
		{
			yield return new WaitForSeconds(1);
		}

		canMove = true;
		canAct = true;
	}
	
	void Update () {
		
	}
}
