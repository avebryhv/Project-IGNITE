﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller2D : RaycastController
{
    

    public float maxSlopeAngle = 80;

    public bool ignoreDirectionChange;

    
    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;
    public bool jumpPressed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        collisions.faceDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {



    }

    private void LateUpdate()
    {
        jumpPressed = false;
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount,Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins(); //Calculate raycast starting point based on the size of actor collider
        collisions.Reset();
        collisions.moveAmountOld = moveAmount; //Stores the intended move distance
        playerInput = input; //Takes in the player's input for the purpose of falling through platforms
        if (moveAmount.y <= 0 )
        {
            DescendSlope(ref moveAmount); //Handles slope collisions
        }
        if (moveAmount.x != 0)
        {
            //if (!ignoreDirectionChange)
            //{
            //    lastDirection = Mathf.Sign(moveAmount.x);

            //}
            collisions.faceDirection = (int)Mathf.Sign(moveAmount.x); //Turns the raycasts towards movement direction
        }
        HorizontalCollisions(ref moveAmount); //Handles horizontal collisions       

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount); //Handles vertical collisions
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.below = true; //When standing on a platform, act as if standing on the ground
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            //Collision Raycast
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                //Allow fall-through platforms
                if (hit.collider.tag == "FallThrough")
                {
                    //Pass through upwards
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    //Drop down
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (GameManager.Instance.usingController)
                    {
                        if (playerInput.y <= -0.9f && Gamepad.current.buttonSouth.wasPressedThisFrame)
                        {
                            collisions.fallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatform", .1f); //Time until player stops falling through
                            continue;
                        }
                    }
                    if (playerInput.y <= -0.9f && Keyboard.current.spaceKey.wasPressedThisFrame)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .1f); //Time until player stops falling through
                        continue;
                    }

                }                
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }
                if (hit.collider.gameObject.tag == "FallThrough") //Fall-through platforms have no horizontal collision detection
                {
                    continue;
                }
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }

                
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
        
        
    }

    void DescendSlope(ref Vector2 moveAmount)
    {

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);

        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }
        

        if (!collisions.slidingDownMaxSlope)
        {
            //Walkable slopes
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }

        
    }

    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle)
            {
                moveAmount.x = hit.normal.x * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);
                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;
        public Vector2 slopeNormal;

        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmountOld;

        public int faceDirection;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            slopeNormal = Vector2.zero;
        }
    }

    void OnChangeBelow()
    {

    }
}
