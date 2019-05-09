using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeContainer : MonoBehaviour
{
    public SkillTreeUI[] trees;
    int selected;
    int maxtrees;
    public SkillTreeUIReferences references;

    void Awake()
    {
        addUI();
    }

    void Start()
    {
        maxtrees = trees.Length;
        for (int i = 0; i < maxtrees; i++)
        {
            if (trees[i] != null)
            {   
                trees[i].references = references;
            }
        }
        if (trees.Length > 0)
        {
            trees[0].startTree();
        }
        
        gameObject.SetActive(false);
    }

    public void instantiateTrees(int input)
    {
        trees = new SkillTreeUI[input];
    }

    public void selectDown()
    {
        trees[selected].selectDown();
    }

    public void selectUp()
    {
        trees[selected].selectUp();
    }

    public void selectLeft()
    {
        trees[selected].selectLeft();
    }

    public void selectRight()
    {
        trees[selected].selectRight();
    }

    public void levelUp()
    {
        trees[selected].levelUp();
    }

    public void levelUpMax()
    {
        trees[selected].levelUpMax();
    }

    void addUI()
    {
        //Set up UI with what we need
        GameObject o = Instantiate((GameObject)Resources.Load("Prefabs/SkillTreeAdditionalUI", typeof(GameObject)));
        Button[] button = o.GetComponentsInChildren<Button>();
        o.transform.SetParent(transform, false);
        o.transform.SetSiblingIndex(0);
        //Add listeners to the buttons in the UI
        button[0].onClick.AddListener(() => levelUp());
        button[1].onClick.AddListener(() => levelUpMax());
        references = o.GetComponent<SkillTreeUIReferences>();
    }

    public void selectNext()
    {
        selected++;
        if (selected >= maxtrees)
        {
            selected = 0;
        }
        trees[selected].updateSelectNode();
    }

    public void selectPrev()
    {
        selected--;
        if (selected < 0)
        {
            selected = maxtrees - 1;
        }
        trees[selected].updateSelectNode();
    }
}