using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerComponentSelector : MonoBehaviour
{
    public MainMenuSelect[,] weapons = new MainMenuSelect[2, 2];
    public MainMenuSelect[,] players = new MainMenuSelect[2, 2];
    public Character character;

    public Transform weaponOneSelect;
    public Transform weaponTwoSelect;
    public Transform playerSelect;

    public MenuController menu;

    public Text playerDescription;
    public Text weaponOneDescription;
    public Text weaponTwoDescription;

    int weaponOneX;
    int weaponOneY;
    int weaponTwoX;
    int weaponTwoY;
    int playerX;
    int playerY;
    int tempX;
    int tempY;
    int currentMode;
    void Awake()
    {
        character = GameObject.Find("Character").GetComponent<Character>();
        weapons[0, 0] = transform.GetChild(3).GetComponent<MainMenuSelect>();
        weapons[1, 0] = transform.GetChild(4).GetComponent<MainMenuSelect>();
        weapons[0, 1] = transform.GetChild(5).GetComponent<MainMenuSelect>();
        weapons[1, 1] = transform.GetChild(6).GetComponent<MainMenuSelect>();

        players[0, 0] = transform.GetChild(7).GetComponent<MainMenuSelect>();
        players[1, 0] = transform.GetChild(8).GetComponent<MainMenuSelect>();
        players[0, 1] = transform.GetChild(9).GetComponent<MainMenuSelect>();
        players[1, 1] = transform.GetChild(10).GetComponent<MainMenuSelect>();

        menu = GameObject.Find("MenuCanvas").GetComponent<MenuController>();
    }

    void updateText()
    {
        playerDescription.text = character.playerDescription;
        weaponOneDescription.text = character.weaponOneDescription;
        weaponTwoDescription.text = character.weaponTwoDescription;
    }

    void Start()
    {
        weaponOneX = 0;
        weaponOneY = 0;
        weaponTwoX = 1;
        weaponTwoY = 0;
        playerX = 0;
        playerY = 0;

        updateText();
    }

    void increaseMode()
    {
        currentMode++;
        if (currentMode > 2)
        {
            currentMode = 0;
        }
    }

    void decreaseMode()
    {
        currentMode--;
        if (currentMode < 0)
        {
            currentMode = 2;
        }
    }

    void move()
    {
        if (currentMode == 0)
        {
            int tempX = weaponOneX;
            int tempY = weaponOneY;
            if (Input.GetKeyDown(KeyCode.W))
            {
                weaponOneY--;
                if (weaponOneY < 0)
                {
                    weaponOneY = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                weaponOneY++;
                if (weaponOneY > 1)
                {
                    weaponOneY = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                weaponOneX--;
                if (weaponOneX < 0)
                {
                    weaponOneX = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                weaponOneX++;
                if (weaponOneX > 1)
                {
                    weaponOneX = 0;
                }
            }

            bool temp = character.selectWeaponOne(weapons[weaponOneX, weaponOneY].value);

            if (temp)
            {
                weaponOneSelect.transform.position = weapons[weaponOneX, weaponOneY].transform.position;
            }
            else
            {
                weaponOneX = tempX;
                weaponOneY = tempY;
            }
        }
        else if (currentMode == 1)
        {
            int tempX = weaponTwoX;
            int tempY = weaponTwoY;
            if (Input.GetKeyDown(KeyCode.W))
            {
                weaponTwoY--;
                if (weaponTwoY < 0)
                {
                    weaponTwoY = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                weaponTwoY++;
                if (weaponTwoY > 1)
                {
                    weaponTwoY = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                weaponTwoX--;
                if (weaponTwoX < 0)
                {
                    weaponTwoX = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                weaponTwoX++;
                if (weaponTwoX > 1)
                {
                    weaponTwoX = 0;
                }
            }

            bool temp = character.selectWeaponTwo(weapons[weaponTwoX, weaponTwoY].value);

            if (temp)
            {
                weaponTwoSelect.transform.position = weapons[weaponTwoX, weaponTwoY].transform.position;
            }
            else
            {
                weaponTwoX = tempX;
                weaponTwoY = tempY;
            }
        }
        else if (currentMode == 2)
        {
            int tempX = playerX;
            int tempY = playerY;
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerY--;
                if (playerY < 0)
                {
                    playerY = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerY++;
                if (playerY > 1)
                {
                    playerY = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                playerX--;
                if (playerX < 0)
                {
                    playerX = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerX++;
                if (playerX > 1)
                {
                    playerX = 0;
                }
            }

            bool temp = character.selectPlayer(players[playerX, playerY].value);

            if (temp)
            {
                playerSelect.transform.position = players[playerX, playerY].transform.position;
            }
            else
            {
                playerX = tempX;
                playerY = tempY;
            }
        }

        updateText();
    }


    // Update is called once per frame
    void Update()
    {
        move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            increaseMode();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            decreaseMode();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            menu.loadScene(1);
        }
    }
}
