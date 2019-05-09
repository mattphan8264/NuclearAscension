using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats myStats;
    Animator anim;
    JumpCheck jumpCheck;
    WeaponSpriteController weaponSprite;
    TeleporterController teleporter;
    Grapple grapple;

    public bool jumping;
    public bool jumpDirection;
    int jumpCount;
    bool doubleJump;
    public float jumpVelocity;
    public float timeToJumpApex;
    float accelerationTimeAirborne;
    float accelerationTimeGrounded;

    public bool movingRight;
    float maxClimbAngle;

    Vector3 velocity;
    public float gravity;
    float verticalRaySpacing;
    float horizontalRaySpacing;
    int horizontalRayCount = 4;
    int verticalRayCount = 4;
    Collider2D myCollider;
    public float playerWidth;
    float playerHeight;
    Vector2 bottomLeft;
    Vector2 bottomRight;
    Vector2 topLeft;
    public Vector2 myPosition;
    LayerMask collisionMask;
    public CollisionInfo collision;
    SlopeCheckBottom slopeCheckBottom;
    SlopeCheckTop slopeCheckTop;
    MovementCheck movementCheck;

    public SpriteRenderer r;
    void Awake()
    {
        myStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        r = GetComponent<SpriteRenderer>();
        jumpCheck = GetComponentInChildren<JumpCheck>();
        weaponSprite = GetComponentInChildren<WeaponSpriteController>();
        grapple = GetComponentInChildren<Grapple>();
        slopeCheckBottom = GetComponentInChildren<SlopeCheckBottom>();
        slopeCheckTop = GetComponentInChildren<SlopeCheckTop>();
        movementCheck = GetComponentInChildren<MovementCheck>();

        collisionMask = LayerMask.GetMask("Ground");

        calculateRays();
        updateRaycastOrigins();
    }

    public void setVelocity(float input)
    {
        velocity.x = input;
    }

    void updateRaycastOrigins()
    {
        myPosition = (Vector2)transform.position;

        playerWidth = myCollider.bounds.size.x / 2;
        playerHeight = myCollider.bounds.size.y / 2;
        bottomLeft = new Vector2(myPosition.x - playerWidth, myPosition.y - playerHeight);
        bottomRight = new Vector2(myPosition.x + playerWidth, myPosition.y - playerHeight);
        topLeft = new Vector2(myPosition.x - playerWidth, myPosition.y + playerHeight);
    }
    void calculateRays()
    {
        myCollider = GetComponent<Collider2D>();

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = myCollider.bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = myCollider.bounds.size.x / (verticalRayCount - 1);
    }

    void Start()
    {
        movingRight = true;
        jumpDirection = true;

        jumpCount = 0;
        doubleJump = false;
        timeToJumpApex = .6f;
        accelerationTimeAirborne = .2f;
        accelerationTimeGrounded = .1f;

        maxClimbAngle = 45;

        gravity = -(2 * myStats.jumpSpeed) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = myStats.jumpSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpVelocity = myStats.jumpSpeed;
        if (collision.above || jumpCheck.collide)
        {
            velocity.y = 0;
        }

        if (collision.below)
        {
            if (!jumping && !recentlyJumped)
            {
                velocity.y = 0;
            }
        }

        if (movementCheck.collide > 0)
        {
            velocity.x = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        move(velocity);
    }

    //Set our velocity to our movespeed * direction (and keep our current y velocity)
    //Direction should be 1 or -1
    float velocityXSmooting;
    public void move(int direction)
    {
        if (movementCheck.collide == 0)
        {
            float targetVelocityX = (float)direction / 10 * myStats.speed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmooting, (collision.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        }

        anim.SetBool("Walking", true);
    }

    public bool grappling;
    public void grappleMove()
    {
        checkSpriteDirection((grapple.transform.position - transform.position).x > 0);
        grappling = true;
        Vector2 temp = (grapple.transform.position - transform.position).normalized;
        if (temp.y < 0)
        {
            temp.y += .1f;
        }

        velocity = temp / 3f;
    }

    public void resetGravity()
    {
        gravity = -(2 * myStats.jumpSpeed) / Mathf.Pow(timeToJumpApex, 2);
    }

    public void stopMove()
    {
        velocity.x = 0;
        anim.SetBool("Walking", false);
    }

    public void resetMovement()
    {
        velocity = new Vector2();
    }

    public void move(Vector2 velocity)
    {
        collision.reset();
        updateRaycastOrigins();
        if (velocity.y != 0)
        {
            verticalCollisions(ref velocity);
        }

        if (velocity.x != 0)
        {
            horizontalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void verticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y);

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? bottomLeft : topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);


            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                velocity.y = hit.distance * directionY;
                rayLength = hit.distance;

                if (collision.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collision.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collision.below = directionY == -1;
                collision.above = directionY == 1;
            }
        }

        /* Only Used if we have different slope angles
        if (collision.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x);
            Vector2 rayOrigin = ((directionX == -1)?bottomLeft:bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collision.slopeAngle)
                {
                    velocity.x = (hit.distance) * directionX;
                    collision.slopeAngle = slopeAngle;
                }
            }
        } */
    }

    void horizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x);

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? bottomLeft : bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            if (i == 0)
            {
                rayOrigin.y += 0.01f;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeCheckTop.slopeTopCheck == 0 && slopeCheckBottom.slopeBottomCheck)
                {
                    if (slopeAngle == 90)
                    {
                        slopeAngle = 45;
                    }
                }

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collision.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    climbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collision.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = hit.distance * directionX;
                    rayLength = hit.distance;

                    if (collision.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collision.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collision.left = directionX == -1;
                    collision.right = directionX == 1;
                }
            }
        }
    }

    void climbSlope(ref Vector2 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collision.below = true;
            collision.climbingSlope = true;
            collision.slopeAngle = slopeAngle;
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;

        public void reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    //Change our sprite facing direction
    //flipX is a bool for whether the sprite should be flipped or not
    public void checkSpriteDirection(bool direction)
    {
        if (direction != movingRight)
        {
            movingRight = direction;
            Vector3 charscale = transform.localScale;
            charscale.x *= -1;
            transform.localScale = charscale;

            weaponSprite.flip();
        }
    }

    //If jumpCount (where pressing space = 1 jumpCount) is less than max
    //Add upward velocity to the player
    public void jump()
    {
        anim.SetBool("Jumping", true);

        if (jumpCount == 0)
        {
            recentlyJumped = true;
            Invoke("setRecentJumpFalse", .1f);
            velocity.y = jumpVelocity;
            jumping = true;
            jumpCount++;
        }

        if (!doubleJump)
        {
            jumpCount = 0;
            doubleJump = true;
        }
    }

    bool recentlyJumped = false;
    void setRecentJumpFalse()
    {
        recentlyJumped = false;
    }

    public void checkJumpDirection()
    {
        jumpDirection = !r.flipX;
    }

    //Reset our jumpcount and our y velocity
    public void resetJumpCount()
    {
        anim.SetBool("Jumping", false);

        jumpCount = 0;
        doubleJump = false;
    }

    public void setJumpingFalse()
    {
        jumping = false;
    }
}
