using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public int healthGrowth;
    public int damageGrowth;
    public int critRateGrowth;
    public float critDamageGrowth;
    public int defenseGrowth;
    public float attackRateGrowth;
    public int levelMod;
    public string characterType;
    public int weaponOne;
    public int weaponTwo;
    public string weaponOneDescription;
    public string weaponTwoDescription;
    public string playerDescription;


    void Awake()
    {
        DontDestroyOnLoad(this);

        weaponOne = 0;
        weaponTwo = 1;
        selectWeaponOne(weaponOne);
        selectWeaponTwo(weaponTwo);
        characterType = "BalancePlayer";
        balancePlayer();
    }


    public bool selectPlayer(int value)
    {
        switch (value)
        {
            case 0:
                balancePlayer();
                break;
            case 1:
                tankPlayer();
                break;
            case 2:
                damagePlayer();
                break;
            case 3:
                critPlayer();
                break;
        }
        return true;
    }

    public bool selectWeaponOne(int value)
    {
        if (weaponTwo != value)
        {
            weaponOne = value;
            updateWeaponText(ref weaponOneDescription, weaponOne);
            return true;
        }
        return false;
    }

    public bool selectWeaponTwo(int value)
    {
        if (weaponOne != value)
        {
            weaponTwo = value;
            updateWeaponText(ref weaponTwoDescription, weaponTwo);
            return true;
        }
        return false;
    }

    void updateWeaponText(ref string description, int input)
    {
        switch (input)
        {
            case 0:
                description = "Weapon Selected: <color=cyan>Pistol</color>. \nWeapon that focuses on single target damage. \nProvides skills that boosts stats based on enemies killed and a chance to shoot multiple bullets at once";
                break;
            case 1:
                description = "Weapon Selected: <color=red>Rifle</color>. \nWeapon that focuses on multiple target damage. \nProvides skills that boost allow bullets to hit multiple enemies and a buff that boosts all stats";
                break;
            case 2:
                description = "Weapon Selected: <color=purple>Sniper</color>. \nWeapon that focuses on single target damage. \nProvides skills that boosts damage against bosses and shooting while standing still";
                break;
            case 3:
                description = "Weapon Selected: <color=yellow>Rocket Launcher</color>. \nWeapon that focuses on multiple target damage. \nProvides skills that allow rockets to explode on contact and lock on enemies";
                break;
        }
    }

    public void loadWeapons(PlayerControls input)
    {
        selectWeapon(weaponOne, 0, input);
        selectWeapon(weaponTwo, 1, input);
    }

    void selectWeapon(int value, int input, PlayerControls controls)
    {
        switch (value)
        {
            case 0:
                if (input == 0)
                {
                    controls.wepOne = controls.gameObject.AddComponent<Pistol>();
                }
                else
                {
                    controls.wepTwo = controls.gameObject.AddComponent<Pistol>();
                }
                break;
            case 1:
                if (input == 0)
                {
                    controls.wepOne = controls.gameObject.AddComponent<Rifle>();
                }
                else
                {
                    controls.wepTwo = controls.gameObject.AddComponent<Rifle>();
                }
                break;
            case 2:
                if (input == 0)
                {
                    controls.wepOne = controls.gameObject.AddComponent<Sniper>();
                }
                else
                {
                    controls.wepTwo = controls.gameObject.AddComponent<Sniper>();
                }
                break;
            case 3:
                if (input == 0)
                {
                    controls.wepOne = controls.gameObject.AddComponent<RocketLauncher>();
                }
                else
                {
                    controls.wepTwo = controls.gameObject.AddComponent<RocketLauncher>();
                }
                break;
        }
    }

    public void balancePlayer()
    {
        healthGrowth = 2;
        damageGrowth = 2;
        critRateGrowth = 1;
        critDamageGrowth = .05f;
        defenseGrowth = 2;
        attackRateGrowth = .02f;
        levelMod = 3;
        characterType = "BalancePlayer";

        playerDescription = "Character Type: <color=blue>Balanced</color>. \nCharacter that gains stats evenly on each level up.";
    }

    public void tankPlayer()
    {
        healthGrowth = 4;
        damageGrowth = 1;
        critRateGrowth = 1;
        critDamageGrowth = .01f;
        defenseGrowth = 7;
        attackRateGrowth = .01f;
        levelMod = 5;
        characterType = "TankPlayer";

        playerDescription = "Character Type: <color=grey>Tank</color>. \nCharacter that gains more health and defense on each level up and less damage, attack rate, and crit damage.";
    }

    public void damagePlayer()
    {
        healthGrowth = 1;
        damageGrowth = 4;
        critRateGrowth = 1;
        critDamageGrowth = .025f;
        defenseGrowth = 2;
        attackRateGrowth = .02f;
        levelMod = 3;
        characterType = "DamagePlayer";

        playerDescription = "Character Type: <color=red>Damage</color>. \nCharacter that gains more damage on each level up and less crit damage and health.";
    }

    public void critPlayer()
    {
        healthGrowth = 1;
        damageGrowth = 1;
        critRateGrowth = 2;
        critDamageGrowth = .075f;
        defenseGrowth = 2;
        attackRateGrowth = .01f;
        levelMod = 3;
        characterType = "CritPlayer";

        playerDescription = "Character Type: <color=yellow>Crit</color>. \nCharacter that gains more critical rate and critical damage on each level up and less attack rate, health, and damage.";
    }
}