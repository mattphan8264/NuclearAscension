using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats : MonoBehaviour {

    /*
    Status effects class in preparation for future enemy attacks
     */
    public int burnCount;
    public int shockCount;
    public int freezeCount;
    public int slowCount;


    public bool unableToAct;
    public bool unableToMove;

    //Instantiate all our variables
    void InstantiateStatusEffects()
    {
        burnCount = 0;
        shockCount = 0;
        freezeCount = 0;
        slowCount = 0;

        unableToAct = false;
        unableToMove = false;
    }

    //Calculate status effect duration based on our stat
    int calculateStatusResistance(int count)
    {
        //Duration decreases by statusResistance percent
        return (int)((float)count - (float)count * statusResistance);
    }

    public void burn(int count)
    {
        //If player is not burned already, start running burning
        //else just reset our cooldown
        if (burnCount <= 0)
        {
            burnCount = calculateStatusResistance(count);
            takeBurnDamage();
        }
        else
        {
            burnCount = calculateStatusResistance(count);
        }
    }

    void takeBurnDamage()
    {
        burnCount--;

        //Take damage
        if (burnCount > 0)
        {
            Invoke("takeBurnDamage", 1);
        }
    }

    public void shock(int count)
    {
        if (shockCount <= 0)
        {
            shockCount = calculateStatusResistance(count);
            takeShockDamage();
        }
        else
        {
            shockCount = calculateStatusResistance(count);
        }
    }

    void takeShockDamage()
    {
        shockCount--;

        //Unable to act
        if (shockCount > 0)
        {
            Invoke("takeShockDamage", 1);
        }
    }
}