using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreak : MonoBehaviour
{
    int hp;
    bool canBreak;

    void Start()
    {
        hp = 5;
        canBreak = true;
    }

    public void breakWall()
    {
        hp--;
        if (hp <= 0 && canBreak)
        {
            canBreak = false;
            MovementCheck move = GameObject.FindWithTag("Player").GetComponentInChildren<MovementCheck>();
            move.destroyBlock(transform);
            Destroy(gameObject);
        }
    }

}