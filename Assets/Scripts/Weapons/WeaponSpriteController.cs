using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteController : MonoBehaviour
{
    //Child of this object
    public Transform bulletSpawn;
    bool facingRight = true;
    Vector2 right;
    void Awake()
    {
        bulletSpawn = transform.GetChild(0).transform;
        right = new Vector2(1, 0);
    }

    public void faceDirection(Vector2 direction)
    {
        if (facingRight)
        {
            transform.right = direction;
        }
        else
        {
            transform.right = -direction;
        }
    }

    public void flip()
    {
        facingRight = !facingRight;
    }

    public void reset()
    {
        transform.right = right;
    }
}
