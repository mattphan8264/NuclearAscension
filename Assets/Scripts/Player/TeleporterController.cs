using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    bool collide;
    bool stage;
    int teleportCooldown;
    public bool canTeleport;
    public int facingRight;

    int teleportDistance;

    void Start()
    {
        teleportCooldown = 10;
        teleportDistance = 25;
        canTeleport = true;
    }

    public void flip()
    {
        if (facingRight == 0)
        {
            facingRight = 1;
        }
        facingRight *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool teleport()
    {
        if (!collide && canTeleport && stage)
        { 
            transform.parent.transform.position = transform.position;
            canTeleport = false;
            transform.localPosition = new Vector3();
            Invoke("resetCD", teleportCooldown);
            return true;
        }
        return false;
    }

    public void resetCD()
    {
        canTeleport = true;
    }

    public void moveLeft()
    {
        if (transform.localPosition.x > -teleportDistance)
        transform.localPosition += new Vector3(-.5f * facingRight, 0);
    }

    public void moveRight()
    {
        if (transform.localPosition.x < teleportDistance)

        transform.localPosition += new Vector3(.5f * facingRight, 0);
    }

    public void moveUp()
    {
        if (transform.localPosition.y < teleportDistance)
        transform.localPosition += new Vector3(0, .5f);
    }

    public void moveDown()
    {
        if (transform.localPosition.y > -teleportDistance)
        transform.localPosition += new Vector3(0, -.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            collide = true;
        }

        if (other.CompareTag("Stage"))
        {
            stage = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            collide = false;
        }

        if (other.CompareTag("Stage"))
        {
            stage = false;
        }
    }
}
