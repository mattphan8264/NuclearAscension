using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    public MovementCheck movementCheck;
    public EnemyJump myJump;

    public CollisionInfo collision;
    public float playerWidth;
    float playerHeight;
    Vector2 bottomLeft;
    Vector2 bottomRight;
    Vector2 topLeft;
    public Vector2 myPosition;
    Collider2D myCollider;
    int horizontalRayCount = 3;
    int verticalRayCount = 3;
    float verticalRaySpacing;
    float horizontalRaySpacing;
    LayerMask collisionMask;
    float maxClimbAngle;
    public Vector3 velocity;
    public float gravity;
    public float timeToJumpApex;

    void Awake()
    {
        movementCheck = GetComponentInChildren<MovementCheck>();
        myJump = GetComponentInChildren<EnemyJump>();
        collisionMask = LayerMask.GetMask("Ground");
        calculateRays();
    }

    void Start()
    {
        maxClimbAngle = 90;
        timeToJumpApex = .6f;
        gravity = -(2 * .6f) / Mathf.Pow(timeToJumpApex, 2);
    }

    void FixedUpdate()
    {
        if (collision.above)
        {
            velocity.y = 0;
        }

        if (myJump != null)
        {
            if (collision.below && myJump.jump == true)
            {
                velocity.y = 0;
            }
        }
        else
        {
            if (collision.below)
            {
                velocity.y = 0;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        move(velocity);
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
        myCollider = GetComponent<BoxCollider2D>();

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = myCollider.bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = myCollider.bounds.size.x / (verticalRayCount - 1);
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

                if (i == 0 && slopeAngle <= maxClimbAngle && movementCheck.collide == 0)
                {
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collision.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    if (slopeAngle == 90)
                    {
                        slopeAngle /= 2;
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
}