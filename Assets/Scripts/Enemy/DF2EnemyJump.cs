using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DF2EnemyJump : EnemyJump
{
    public override void jumpUp()
    {
        jump = false;
        movement.velocity.y = .6f;
        Invoke("stopJump", 1);
    }
}
