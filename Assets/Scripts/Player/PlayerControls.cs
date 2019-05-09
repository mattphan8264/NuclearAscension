using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    /*
    Class that contains implementation for movement in game (jumping and left/right)
    Class also contains implementation for player controls
     */
    PlayerStats myStats;
    PlayerUI myUI;
    public PlayerMovement myMove;
    public Weapon wepOne;
    public Weapon wepTwo;
    TeleporterController teleporter;
    WeaponSpriteController weaponSprite;
    Grapple grapple;

    int weaponMode;
    int maxWeapons;
    float attackTime;

    public bool right;
    public bool left;
    public bool up;
    public bool down;
    Vector2 abilityDirection;

    bool skillTreeOpen;

    MenuController menu;

    //Grabbing scripts we need from player
    void Awake()
    {
        myStats = GetComponent<PlayerStats>();
        myUI = GetComponentInChildren<PlayerUI>();
        myMove = GetComponent<PlayerMovement>();
        weaponSprite = GetComponentInChildren<WeaponSpriteController>();
        grapple = GetComponentInChildren<Grapple>();
        grapple.transform.localPosition = new Vector2();
        grapple.gameObject.SetActive(false);

        menu = GameObject.Find("MenuCanvas").GetComponent<MenuController>();

        //Test for adding a weapon to player
        loadWeapons();
        assignWeapon(0);
        assignWeapon(1);
    }

    void loadWeapons()
    {
        Character temp = GameObject.Find("Character").GetComponent<Character>();
        temp.loadWeapons(this);
    }

    void assignWeapon(int weaponNumber)
    {
        Weapon temp;
        if (weaponNumber == 0)
        {
            temp = wepOne;
            temp.assignContainer(myUI.skillTree1);
        }
        else
        {
            temp = wepTwo;
            temp.assignContainer(myUI.skillTree2);
        }
        temp.instantiateTree();
        for (int i = 0; i < temp.container.trees.Length; i++)//wepOne.container.trees.Length; i++)
        {
            if (weaponNumber == 0)
            {
                temp.container.trees[i].function = myStats.weaponOneLevelUp;
                temp.container.trees[i].weapon = temp;
                temp.container.trees[i].getSkillPoints = myStats.getWeaponOneSkillPoints;
            }
            else
            {
                temp.container.trees[i].function = myStats.weaponTwoLevelUp;
                temp.container.trees[i].weapon = temp;
                temp.container.trees[i].getSkillPoints = myStats.getWeaponTwoSkillPoints;
            }
        }
        temp.blockAct = myStats.weaponActingDisable;
        temp.blockMove = myStats.weaponMovingDisable;
        temp.weaponNumber = weaponNumber;
    }

    //Instantiate all variables used
    void Start()
    {
        //Loading screen
        //Time.timeScale = 1;

        right = false;
        left = false;
        up = false;
        down = false;
        abilityDirection = new Vector2();

        weaponMode = 0;
        maxWeapons = 2;

        skillTreeOpen = false;

        usingGrapple = false;
    }

    void swapWeaponMode()
    {
        weaponMode++;
        if (weaponMode >= maxWeapons)
        {
            weaponMode = 0;
        }
    }

    //Calculate the direction of where the bullet should go (supports 8 directions)
    void calculateShotDirection()
    {
        if (up)
        {
            abilityDirection.y = 1;
            if (right)
            {
                abilityDirection.x = 1;
            }
            else if (left)
            {
                abilityDirection.x = -1;
            }
            else
            {
                abilityDirection.x = 0;
            }
        }
        else if (down)
        {
            abilityDirection.y = -1;
            if (right)
            {
                abilityDirection.x = 1;
            }
            else if (left)
            {
                abilityDirection.x = -1;
            }
            else
            {
                abilityDirection.x = 0;
            }
        }
        else
        {
            abilityDirection.y = 0;
            if (right)
            {
                abilityDirection.x = 1;
            }
            else
            {
                abilityDirection.x = -1;
            }
        }

        abilityDirection = abilityDirection.normalized;
    }

    public bool usingGrapple;
    public bool grappleMove;
    //Check key inputs and fasterFall
    void FixedUpdate()
    {
        if (!usingGrapple)
        {
            checkPlayerMove();
        }
    }

    void Update()
    {
        checkGrapple();
        checkDirection();
        checkBreakWall();
        if (!usingGrapple)
        {
            checkPlayerUI();
            checkPlayerAct();
            checkPlayerJump();
            myMove.grappling = false;
        }
        else if (grappleMove)
        {
            myMove.grappleMove();
        }

    }

    void checkDirection()
    {
        //Calculate which direction player wants to shoot and use an ability in that direction
        if (Input.GetKey(KeyCode.I))
        {
            up = true;
            down = false;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            down = true;
            up = false;
        }
        else
        {
            down = false;
            up = false;
        }

        if (Input.GetKey(KeyCode.J))
        {
            left = true;
            right = false;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            right = true;
            left = false;
        }
        else
        {
            left = false;
            right = false;
        }
    }

    void checkBreakWall()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GlobalStats.breakWall = !GlobalStats.breakWall;
        }
    }

    public bool canUngrapple;
    void checkGrapple()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!usingGrapple)
            {
                calculateShotDirection();
                usingGrapple = grapple.shootGrapple(abilityDirection);
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if (canUngrapple)
            {   
                grapple.reset();
            }
        }

    }

    void checkPlayerUI()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!skillTreeOpen)
            {
                skillTreeOpen = true;
                myUI.openTree();
                Time.timeScale = 0;
                myStats.canMove = false;
                myStats.canAct = false;
            }
            else
            {
                skillTreeOpen = false;
                myUI.closeTree();
                Time.timeScale = 1;
                myStats.canMove = true;
                myStats.canAct = true;
            }
        }

        //If the skill tree is open
        if (skillTreeOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                myUI.openNextTree();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                myUI.nextTree();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                myUI.prevTree();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                myUI.selectPrevNode();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                myUI.selectNextNode();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                myUI.selectRight();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                myUI.selectLeft();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                myUI.levelUp();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                myUI.levelUpMax();
            }
        }
    }

    void moveRight()
    {
        myMove.move(1);
        myMove.checkSpriteDirection(true);
    }

    void moveLeft()
    {
        myMove.move(-1);
        myMove.checkSpriteDirection(false);
    }

    void stopMove()
    {
        myMove.stopMove();
    }

    void checkPlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myMove.jump();
        }
    }

    void checkPlayerAct()
    {
        if (myStats.canAct)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                swapWeaponMode();
            }

            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K))
            {
                fire();
            }
            else
            {
                weaponSprite.reset();
            }
        }
    }

    void fire()
    {
        calculateShotDirection();

        if (Time.time >= attackTime || attackTime == 0)
        {
            weaponSprite.faceDirection(abilityDirection);
            if (weaponMode == 0)
            {
                wepOne.abilityOne(abilityDirection, weaponSprite.bulletSpawn.position);
                attackTime = wepOne.attackTime;
            }
            else if (weaponMode == 1)
            {
                wepTwo.abilityOne(abilityDirection, weaponSprite.bulletSpawn.position);
                attackTime = wepTwo.attackTime;
            }
        }
    }

    void checkPlayerMove()
    {

        if (myStats.canMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                moveRight();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveLeft();
            }
            else
            {
                stopMove();
            }
        }
    }

}