using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{

    /*
	Skill tree UI. Provides functions to level up skills and view skill information
	 */
    SkillTreeNode selected;
    SkillTreeNode[] nodes;
    int[] index;
    int[] skilLRequirements;
    int[] currentSkillPointCount;
    public delegate int weaponSkillUp(int input);
    public weaponSkillUp function;
    public SkillTreeUIReferences references;

    public delegate int getPointCount();
    public getPointCount getSkillPoints;

    public Weapon weapon;

    int nodeSelected;
    int nodeSelectedRow;
    int maxNodes;
    int indexLength;

    //Update all the skills to make it organized in the skill tree
    public void updateButtonPosition(SkillTreeNode[] nodes, int[] index, int[] skillRequirements, int treeNumber)
    {
        nodeSelected = 0;
        maxNodes = nodes.Length - 1;
        indexLength = index.Length - 1;
        this.nodes = nodes;
        this.index = new int[indexLength + 1];
        
        this.skilLRequirements = skillRequirements;
        this.currentSkillPointCount = new int[indexLength + 1];

        int count = 0;
        for (int i = 0; i < index.Length; i++)
        {
            int temp = index[i] - count;
            int xTemp = 0;
            this.index[i] = count;

            for (int a = count; a < count + temp; a++)
            {
                nodes[a].transform.localPosition = new Vector2(-425 + (55 * xTemp++) + treeNumber * 175, 200 - (i * 65));
                nodes[a].myUI = this;
                nodes[a].treeNumber = i;
            }

            count = index[i];
        }
    }

    public void selectUp()
    {
        nodeSelectedRow--;
        if (nodeSelectedRow < 0)
        {
            nodeSelectedRow = indexLength;
        }
        nodeSelected = index[nodeSelectedRow];
        selectNode(nodes[nodeSelected]);
    }

    public void selectDown()
    {
        nodeSelectedRow++;
        if (nodeSelectedRow > index.Length - 1)
        {
            nodeSelectedRow = 0;
        }
        nodeSelected = index[nodeSelectedRow];
        selectNode(nodes[nodeSelected]);
    }

    public void selectRight()
    {
        nodeSelected++;
        if (nodeSelected > maxNodes)
        {
            nodeSelected = 0;
        }
        selectNode(nodes[nodeSelected]);
    }
    public void selectLeft()
    {
        nodeSelected--;
        if (nodeSelected < 0)
        {
            nodeSelected = maxNodes;
        }
        selectNode(nodes[nodeSelected]);
    }

    

    //Level up a skill. Uses skilltreenodes levelup and playerstats level up
    public void levelUp()
    {
        if (selected != null)
        {
            if (levelUpChecker() && selected.canLevelUp())
            {
                int response = function(1);
                if (response != -1)
                {
                    selected.levelUp();
                    currentSkillPointCount[selected.treeNumber] += response;
                    references.setCurrent(selected.currentLevel);
                    weapon.updateWeaponStats();
                    selectNode(selected);
                }
            }
        }
    }

    //Level up a skill as much as possible. Uses skilltreenodes levelup and playerstats level up
    public void levelUpMax()
    {
        if (selected != null)
        {
            if (levelUpChecker() && selected.canLevelUp())
            {
                //Check how much the skill can be leveled
                int temp = selected.maxLevel - selected.currentLevel;
                //See how many points we actually have available
                int response = function(temp);
                if (response != -1)
                {
                    //Level up our skill and update UI
                    selected.levelUp(response);
                    currentSkillPointCount[selected.treeNumber] += response;
                    references.setCurrent(selected.currentLevel);
                    weapon.updateWeaponStats();
                    selectNode(selected);
                }
            }
        }
    }

    bool levelUpChecker()
    {
        if (selected.treeNumber == 0)
        {
            return true;
        }
        else if (currentSkillPointCount[selected.treeNumber - 1] >= skilLRequirements[selected.treeNumber - 1])
        {
            return true;
        }
        return false;
    }

    //Select a node
    public void selectNode(SkillTreeNode input)
    {
        selected = input;
        references.setAll(selected.skillName, selected.skillDescription,
        selected.currentLevel, selected.maxLevel, getSkillPoints(), selected.sprite);

        if (!references.selectIcon.activeSelf)
        {
            references.selectIcon.SetActive(true);
        }
        references.selectIcon.transform.position = selected.transform.position;
    }

    public void updateSelectNode()
    {
        selectNode(nodes[nodeSelected]);
    }
    
    public void startTree()
    {
        selectNode(nodes[nodeSelected]);
        nodeSelectedRow = 0;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
