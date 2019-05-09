using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats : MonoBehaviour {
	/*
	Level up implementation for player
	 */
	public int level;
	public int currentExp;
	public int expToNextLevel;
	public int enemiesKilled;


	int healthGrowth;
    int damageGrowth;
    int critRateGrowth;
    float critDamageGrowth;
    int defenseGrowth;
    float attackRateGrowth;
    int levelMod;

	void loadLevelGrowth()
	{
		Character temp = GameObject.Find("Character").GetComponent<Character>();
		healthGrowth = temp.healthGrowth;
		damageGrowth = temp.damageGrowth;
		critRateGrowth = temp.critRateGrowth;
		critDamageGrowth = temp.critDamageGrowth;
		defenseGrowth = temp.defenseGrowth;
		attackRateGrowth = temp.attackRateGrowth;
		levelMod = temp.levelMod;
		spriteSheetName = temp.characterType;

	}

	//Instantiate our start level and exp
	void InstantiateLevelStats()
	{
		level = 1;
		currentExp = 0;
		expToNextLevel = 10;
		enemiesKilled = 0;
	}

	//Level up
	//Increase our level by one, calculate exp for next level
	//Add skill points
	void levelUp()
	{
		level++;
		updateExpToNext();
		levelUpStats();
		GlobalStats.leveled(level);

		weaponOneSkillPoints++;
		weaponTwoSkillPoints++;

		UI.updateLevelText();
	}

	void levelUpStats()
	{
		float percent = health / maxHealth;
		maxHealth += healthGrowth;
		health = (int) (maxHealth * percent);
		damage += damageGrowth;
		defense += defenseGrowth;
		if (defense > 500)
		{
			defense = 500;
		}
		
		attackRate -= attackRateGrowth;

		if (level % levelMod == 0)
		{
			critRate += critRateGrowth;
			critDamage += critDamageGrowth;
		}
	}

	//Increase exp to next level exponentially based on current level
	void updateExpToNext()
	{
		if (level < 20)
		{
			expToNextLevel = (int)(float)(expToNextLevel * 1.15f);
		}
		else if (level < 50)
		{
			expToNextLevel = (int)(float)(expToNextLevel * 1.2f);
		}
		else
		{
			expToNextLevel = (int)(float)(expToNextLevel * 1.3f);
		}
	}

	//Increase our base health and damage for the level up
	//Keeps our health percent
	void updateStats()
	{
		float healthPercent = (float) health / (float) maxHealth;
		maxHealth += 2;
		damage += 1;
		health = (int)(float)(maxHealth * healthPercent);

		UI.updateHealth();
	}

	//Increase our current exp and check for level up
	//Subtract expToNextLevel from current exp (so that extra carries over)
	public void addExp(int exp)
	{
		currentExp += exp;
		if (currentExp >= expToNextLevel)
		{
			currentExp -= expToNextLevel;
			levelUp();
			updateStats();
		}
		UI.updateExp();
	}
	public void increaseKillCount()
	{
		enemiesKilled++;
	}
}
