using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grapple : MonoBehaviour
{
    public bool grapple;
    float grappleTime;
    float grappleCD;
    PlayerControls myself;
    Vector2 startPos;
    LineRenderer line;
    LayerMask collisionMask;
    Vector2 myPosition;
    Collider2D myCollider;
    float playerWidth;
    float playerHeight;

    void Awake()
    {
        myself = GetComponentInParent<PlayerControls>();
        line = GetComponent<LineRenderer>();
        myCollider = GetComponent<Collider2D>();
        collisionMask = LayerMask.GetMask("Ground");

        playerWidth = myCollider.bounds.size.x / 2;
        playerHeight = myCollider.bounds.size.y / 2;

    }

    void Start()
    {
        grappleCD = 10;
    }

    void Update()
    {
        if (grapple)
        {
            transform.position = startPos;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, myself.transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myself.canUngrapple = true;
        }
    }

    public void reset()
    {
        grapple = false;
        myself.usingGrapple = false;
        myself.grappleMove = false;
        myself.myMove.resetGravity();
        transform.position = transform.parent.position;
        this.gameObject.SetActive(false);
    }

    Vector2 topRight;
    Vector2 topLeft;
    Vector2 bottomLeft;
    Vector2 bottomRight;
    Vector2 middleRight;
    Vector2 middleLeft;
    Vector2 middleTop;
    Vector2 middleBottom;
    void updateRaycastOrigins()
    {
        myPosition = (Vector2)transform.parent.position;

        bottomLeft = new Vector2(myPosition.x - playerWidth, myPosition.y - playerHeight);
        bottomRight = new Vector2(myPosition.x + playerWidth, myPosition.y - playerHeight);
        topLeft = new Vector2(myPosition.x - playerWidth, myPosition.y + playerHeight);
        topRight = new Vector2(myPosition.x + playerWidth, myPosition.y + playerHeight);
        middleRight = new Vector2(myPosition.x + playerWidth, myPosition.y);
        middleLeft = new Vector2(myPosition.x - playerWidth, myPosition.y);
        middleTop = new Vector2(myPosition.x, myPosition.y + playerHeight);
        middleBottom = new Vector2(myPosition.x, myPosition.y - playerHeight);
    }

    public bool shootGrapple(Vector2 direction)
    {
        this.direction = direction;
        RaycastHit2D mainHit = Physics2D.Raycast(transform.parent.position, this.direction.normalized, 15, collisionMask);
        if (mainHit)
        {
            hitLength = mainHit.distance - 0.5f;
            if (rayCastCheck())
            {
                gameObject.SetActive(true);
                myself.myMove.gravity = 0;
                myself.canUngrapple = false;
                transform.position = mainHit.point;
                startPos = transform.position;

                transform.up = this.direction.normalized;

                myself.myMove.resetMovement();
                myself.grappleMove = true;

                grapple = true;
                grappleTime = Time.time + grappleCD;
                return true;
            }
        }
        return false;
    }

    RaycastHit2D hit;
    float hitLength;
    Vector2 direction;
    bool rayCastCheck()
    {
        updateRaycastOrigins();
        if ((direction.y == 1 || direction.y == -1) && direction.x == 0)
        {
            return rayCastLocation(middleLeft, middleRight);
        }
        else if (direction.y > 0 && direction.x > 0)
        {
            return rayCastLocation(topLeft, bottomRight);
        }
        else if (direction.y < 0 && direction.x > 0)
        {
            return rayCastLocation(bottomLeft, topRight);
        }
        else if (direction.y > 0 && direction.x < 0)
        {
            return rayCastLocation(bottomLeft, topRight);
        }
        else if (direction.y < 0 && direction.x < 0)
        {
            return rayCastLocation(topLeft, bottomRight);
        }
        else
        {
            return rayCastLocation(middleTop - new Vector2(0, .01f), middleBottom + new Vector2(0, .01f));
        }
    }

    bool rayCastLocation(Vector2 one, Vector2 two)
    {
        hit = Physics2D.Raycast(one, direction.normalized, 15, collisionMask);

        if (hit.distance >= hitLength)
        {
            hit = Physics2D.Raycast(two, direction.normalized, 15, collisionMask);
            if (hit.distance >= hitLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}