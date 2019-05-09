using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{

    Image image;
    public SkillTreeUI myUI;

    /*
    SkillTreeNode class. Holds all the values for skills.
     */
    public string skillName;
    public string skillDescription;
    public Sprite sprite;
    public int currentLevel;
    public int maxLevel;
    public int treeNumber;
    public float[] scale;
    public float[] value;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        currentLevel = 0;
    }

    public void updateUI()
    {
        //Update the skill tree UI to show our info
        myUI.selectNode(this);
    }

    public bool canLevelUp()
    {
        if (currentLevel < maxLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Level up our skill
    public void levelUp()
    {
        currentLevel++;
        calculateValue();
    }

    //Level up our skill with a given number of points
    public void levelUp(int input)
    {
        if (maxLevel - currentLevel >= input)
        {
            currentLevel += input;
            calculateValue();
        }
    }

    //Take in inputs and set them to our variables. Calculate the new value
    public void setValues(string[] text, float[] values, float[] scaleIn, Sprite spriteIn)
    {

        sprite = spriteIn;
        image.sprite = sprite;

        skillName = text[0];
        skillDescription = text[1];

        maxLevel = (int)values[0];

        scale = scaleIn;

        calculateValue();

    }

    //Calculate our value based on level and scale (used in skills)
    void calculateValue()
    {
        value = new float[scale.Length];
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = scale[i] * currentLevel;
        }
    }
}