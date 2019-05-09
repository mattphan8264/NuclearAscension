using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{

    public MovementCheck temp;
    public BasicEnemyMovement movement;
    public bool jump;
    void Awake()
    {
        temp = GetComponent<MovementCheck>();
        movement = GetComponentInParent<BasicEnemyMovement>();
    }

    void Start()
    {
        jump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (temp.collide > 0)
        {
            if (jump)
            {
                jumpUp();
            }
        }
    }

    public virtual void jumpUp()
    {
        jump = false;
        movement.velocity.y = .6f;
        Invoke("stopJump", 1);
    }

    public void stopJump()
    {
        jump = true;
    }
}
