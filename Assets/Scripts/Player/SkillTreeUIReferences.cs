using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUIReferences : MonoBehaviour
{

	/*
	Additional UI class for the skill tree. Holds text objects and provides functions to update them.
	 */
    public Text skillName;
    public Text description;
    public Text currentLevel;
    public Text maxLevel;
    public Text currentSkillPointCount;
    public Image skillImage;
	public GameObject selectIcon;


    public void setAll(string name, string description, int current, int max, int currentPoints, Sprite skillSprite)
    {
        setName(name);
        setDescription(description);
        setCurrent(current);
        setMax(max);
        setSkillPoint(currentPoints);
        skillImage.sprite = skillSprite;
    }

    public void setName(string name)
    {
        this.skillName.text = "Skill Name: " + name;
    }

    public void setDescription(string description)
    {
        this.description.text = "Skill Description: " + description;
    }

    public void setCurrent(int current)
    {
        this.currentLevel.text = "Current Level: " + current;
    }

    public void setMax(int max)
    {
        this.maxLevel.text = "Max Level: " + max;
    }

    public void setSkillPoint(int count)
    {
        currentSkillPointCount.text = "Skill Points: " + count;
    }
}
