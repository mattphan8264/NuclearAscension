using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    //Player UI class. Updates the UI for the player
    public Image health;
    public Image exp;
    public Text healthText;
    public Text expText;
    public Text levelText;

    public PlayerStats myStats;
    public Transform buffs;

    public SkillTreeContainer skillTree1;
    public SkillTreeContainer skillTree2;

    public SkillTreeContainer[] trees;
    int selected;
    int maxTrees;

    Button temp;

    public void openTree()
    {
        trees[selected].gameObject.SetActive(true);
    }
    public void closeTree()
    {
        trees[selected].gameObject.SetActive(false);
    }
    public void openNextTree()
    {
        closeTree();
        selected++;
        if (selected >= maxTrees)
        {
            selected = 0;
        }
        openTree();
    }

    public void nextTree()
    {
        trees[selected].selectNext();
    }
    public void prevTree()
    {
        trees[selected].selectPrev();
    }

    public void selectNextNode()
    {
        trees[selected].selectDown();
    }
    public void selectPrevNode()
    {
        trees[selected].selectUp();
    }
    public void selectRight()
    {
        trees[selected].selectRight();
    }
    public void selectLeft()
    {
        trees[selected].selectLeft();
    }

    public void levelUp()
    {
        trees[selected].levelUp();
    }
    public void levelUpMax()
    {
        trees[selected].levelUpMax();
    }

    void Awake()
    {
        myStats = GetComponentInParent<PlayerStats>();
        trees = new SkillTreeContainer[2];
        trees[0] = skillTree1;
        trees[1] = skillTree2;
        selected = 0;
        maxTrees = trees.Length;
    }
    void Start()
    {
        healthText.text = myStats.health + "/" + myStats.maxHealth;
        expText.text = myStats.currentExp + "/" + myStats.expToNextLevel;
        levelText.text = "Level " + myStats.level;
    }

    //Methods for updating our UI
    public void updateHealth()
    {
        healthText.text = myStats.health + "/" + myStats.maxHealth;
        health.fillAmount = (float)myStats.health / (float)myStats.maxHealth;
    }
    public void updateExp()
    {
        expText.text = myStats.currentExp + "/" + myStats.expToNextLevel;
        exp.fillAmount = (float)myStats.currentExp / (float)myStats.expToNextLevel;
    }

    public void updateLevelText()
    {
        levelText.text = "Level " + myStats.level;
    }

    public void addBuffChild(Transform t)
    {
        t.SetParent(buffs, false);
        t.gameObject.SetActive(false);
        t.GetComponent<PlayerBuff>().myUI = this;
        updateChildrenPosition();
    }
    public void updateChildrenPosition()
    {
        float xCoor = 45f;
        float yCoor = 45f;
        int xCount = 0;
        int yCount = 0;
        foreach (Transform child in buffs)
        {
            if (child.gameObject.activeSelf)
            {
                child.transform.position = new Vector2(buffs.position.x + xCoor * xCount, buffs.position.y - yCoor * yCount);
                xCount++;
            }

            if (xCount > 10)
            {
                yCount++;
                xCount = 0;
            }
        }
    }

    void Update()
    {

    }
}
