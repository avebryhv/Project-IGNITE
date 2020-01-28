using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{
    PlayerScriptFinder finder;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timetoJumpApex = .4f;

    float accelerationTimeAirborne = .05f;
    float accelerationTimeGrounded = .1f;

    public float moveSpeed = 6;

    public float wallSlideSpeedMax = 3;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallJumpLeap;
    public float wallStickTime = .25f;
    public float timeToWallUnstick;

    public float gravity = -20;
    float maxJumpVelocity = 8;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    public float lastDirection;

    Controller2D controller;
    //public MeleeAttack meleeController;
    //public PlayerSpriteController playerSpriteController;
    //PlayerUnlocks unlocks;
    //public PlayerHealth playerHealth;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    public bool jumpPressedThisFrame;
    public bool doubleJumpPressedThisFrame;
    public bool crouching;
    public float framesInAir; //Used to fix issue where player leaves the ground at the peak of a slope. Treating player as in air after 2 frames seems to work consistently
    int airStallCount; //Counts the number of times the player has performed actions that stall them in midair
    bool inAirStall;

    public bool inKnockback = false;
    bool hitThisFrame = false;
    Vector2 knockbackVelocity;

    public GameObject currentTile;

    //Double Jump Variables
    public bool canDoubleJump;
    public GameObject airHike;

    //Divekick Variables
    public bool inDiveKick = false;
    public Vector2 divekickDirection;
    float diveDir;    
    public bool canDiveKick;
    public float diveKickCooldown;

    //Dash Variables
    public bool inDash = false;
    bool canDash = true;
    float dashDir;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    float airEvadeCount;

    //Lock On
    public bool lockedOn = false;
    public float lockOnSpeedModifier = 0.5f;

    //Attack Special Movement
    public bool inAttackMovement;
    public Vector2 attackMovementDirection;
    public float attackMovementSpeed;
    public float attackMovementTime;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight / Mathf.Pow(timetoJumpApex, 2));
        maxJumpVelocity = Mathf.Abs(gravity) * timetoJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        lastDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        jumpPressedThisFrame = false;
        doubleJumpPressedThisFrame = false;

        wallSliding = false;

        //Player cannot input movement if in an attack or crouching
        float targetVelocityX;
        targetVelocityX = directionalInput.x * moveSpeed;

        if (crouching || inKnockback || finder.guard.isGuarding || (finder.melee.inAttack) || inAirStall)
        {
            targetVelocityX = 0;
        }

        //Sets x velocity: all moves that change x velocity come after this
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (!inKnockback)
        {
            HandleWallSliding();
        }
        
        //Sets y velocity: all moves that change y velocity come after this
        velocity.y += gravity * Time.deltaTime;

        if (!inKnockback)
        {
            HandleDash();
        }

        if (!inKnockback)
        {
            HandleDiveKick();
        }

        if (!inKnockback && lockedOn && framesInAir <= 2)
        {
            velocity.x = velocity.x * lockOnSpeedModifier;
        }

        if (inAirStall)
        {
            float tempV = velocity.y;
            //float velocityMod = tempV * (((airStallCount - 3) * 0.1f));
            //velocityMod = Mathf.Clamp(velocityMod, 0, 1);
            //velocity.y = velocityMod;
            int count = (airStallCount - 3);
            count = Mathf.Clamp(count, 0, 10);
            float velocityModifier = count * 0.1f;
            velocity.y = velocity.y * velocityModifier;
        }

        if (finder.melee.inAttack && inAttackMovement)
        {
            velocity = attackMovementDirection * attackMovementSpeed;
        }

        OnLanding();

        CheckKnockback();
        //Moves player: all moves that affect movement come before this
        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }

        }

        hitThisFrame = false;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        if (input.x != 0 && !lockedOn)
        {
            ChangeDirection(Mathf.Sign(input.x));
        }
        
    }

    void ChangeDirection(float dir)
    {
        //Check conditions that would disallow player from changing direction
        if (!inKnockback && !inDiveKick && !finder.melee.inAttack)
        {
            lastDirection = dir;
        }
    }

    public void OnJumpInputDown()
    {        
        //Wall Jump
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallJumpLeap.x;
                velocity.y = wallJumpLeap.y;
            }
        }
        //Normal Jump
        if ((controller.collisions.below || framesInAir <= 2) && !finder.guard.isGuarding)
        {
            if (finder.melee.inAttack)
            {
                if (finder.melee.currentState == MeleeAttacker.phase.Endlag)
                {
                    finder.melee.EndAttack();
                    finder.melee.CancelBuffer();
                    if (controller.collisions.slidingDownMaxSlope)
                    {
                        if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) //Not jumping against max slope
                        {
                            velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                            velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                            jumpPressedThisFrame = true;
                        }
                    }
                    else
                    {
                        velocity.y = maxJumpVelocity;
                        jumpPressedThisFrame = true;
                    }
                }
            }
            else
            {
                if (controller.collisions.slidingDownMaxSlope)
                {
                    if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) //Not jumping against max slope
                    {
                        velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                        velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                        jumpPressedThisFrame = true;
                    }
                }
                else
                {
                    velocity.y = maxJumpVelocity;
                    jumpPressedThisFrame = true;
                }
            }
            

        }
        //Double Jump
        if (!controller.collisions.below && framesInAir > 2 && canDoubleJump && !controller.collisions.fallingThroughPlatform && !wallSliding && !inDash && !inDiveKick)
        {

            velocity.y = maxJumpVelocity;
            canDoubleJump = false;
            doubleJumpPressedThisFrame = true;
            GameObject airHikeEffect = Instantiate(airHike, transform.position, Quaternion.identity);
            airHikeEffect.transform.position -= new Vector3(0, 1.8f, 0);
            airHikeEffect.transform.Rotate(new Vector3(0, 0, -45 * directionalInput.x));

        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity && canDiveKick)
        {
            velocity.y = minJumpVelocity;

        }
    }

    public void OnLockOnDown()
    {
        lockedOn = true;
    }

    public void OnLockOnUp()
    {
        lockedOn = false;
    }

    public void OnDashInput()
    {
        if (!inDash && canDash && !finder.guard.isGuarding)
        {
            if (controller.collisions.below)
            {
                Evade();
            }
            else
            {
                if (airEvadeCount < 1)
                {
                    airEvadeCount++;
                    Evade();
                }
            }
            
        }
    }

    void Evade()
    {
        inDash = true;
        canDash = false;
        inDiveKick = false;
        //playerHealth.canTakeDamage = false;
        //meleeController.canAttack = false;
        if (directionalInput.x == 0)
        {
            dashDir = lastDirection;
        }
        else
        {
            dashDir = Mathf.Sign(directionalInput.x);
        }

        Invoke("CancelDash", dashTime);
        Invoke("SetDashCooldown", dashCooldown);
    }

    void CancelDash()
    {
        inDash = false;
        //playerHealth.canTakeDamage = true;
        //meleeController.canAttack = true;
    }

    void SetDashCooldown()
    {
        canDash = true;
    }

    void HandleDash()
    {
        if (inDash)
        {
            velocity.x = dashDir * dashSpeed;
            velocity.y = 0;
        }
    }

    public void OnDiveKickInput()
    {
        if (!controller.collisions.below && !inDiveKick && !inDash && !controller.collisions.fallingThroughPlatform && canDiveKick)
        {
            inDiveKick = true;            
            if (directionalInput.x == 0)
            {
                diveDir = lastDirection;
            }
            else
            {
                diveDir = Mathf.Sign(directionalInput.x);
            }
        }
    }

    void HandleDiveKick()
    {
        if (inDiveKick)
        {
            velocity.x = divekickDirection.x * diveDir;
            velocity.y = divekickDirection.y;
        }
    }

    public void DiveKickBounce()
    {
        inDiveKick = false;
        velocity.y = maxJumpVelocity;
        canDoubleJump = true;
        canDiveKick = false;
        StopCoroutine(ResetDiveCooldown());
        StartCoroutine(ResetDiveCooldown());
    }

    public void HandleWallSliding()
    {
        //Wall Sliding
        wallDirX = (controller.collisions.left) ? -1 : 1;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void OnLanding()
    {
        if (controller.collisions.below)
        {
            canDoubleJump = true;
            inDiveKick = false;
            canDiveKick = true;
            airStallCount = 0;
            framesInAir = 0;
            airEvadeCount = 0;
            if (inKnockback && !hitThisFrame)
            {
                inKnockback = false; //Stops knockback phase once the floor is hit
                controller.ignoreDirectionChange = false;
                //meleeController.canAttack = true;
                //StopAllCoroutines();
                //StartCoroutine("StartIFrames"); //Handles IFrames
                //StartCoroutine("FlashSprite");
            }
            if (finder.melee.inHelmSplitter)
            {
                finder.melee.EndHelmSplitter();
            }
        }
        else
        {
            framesInAir++;
        }
    }    

    public void TakeKnockback(Vector2 dir)
    {
        hitThisFrame = true;
        inKnockback = true;
        knockbackVelocity = dir;
        controller.ignoreDirectionChange = true;
        //meleeController.canAttack = false;
    }

    void CheckKnockback()
    {
        if (hitThisFrame && inKnockback)
        {
            velocity = knockbackVelocity;
        }
    }

    IEnumerator ResetDiveCooldown()
    {
        yield return new WaitForSeconds(diveKickCooldown);
        canDiveKick = true;
    }

    public void SetAirStall()
    {
        inAirStall = true;
        airStallCount += 1;
    }

    public void EndAirStall()
    {
        inAirStall = false;
    }

    public void SetSpecialAttackMovement(Vector2 dir, float sp, float t)
    {
        inAttackMovement = true;
        attackMovementDirection = dir;
        attackMovementSpeed = sp;
        attackMovementTime = t;
    }

    public void StopSpecialAttackMovement()
    {
        inAttackMovement = false;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }


}
