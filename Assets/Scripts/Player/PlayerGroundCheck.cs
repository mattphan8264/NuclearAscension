using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{

    //Script that implements touching a the ground to reset jump count
    PlayerMovement controls;

    void Awake()
    {
        controls = GetComponentInParent<PlayerMovement>();
    }

    //If we touch ground, reset jumpCount (See PlayerControls.cs)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            controls.resetJumpCount();
            controls.setJumpingFalse();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            controls.setJumpingFalse();
        }
    }

    void resetJump()
    {
        controls.resetJumpCount();
    }
}
