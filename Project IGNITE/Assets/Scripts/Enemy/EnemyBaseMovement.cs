using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timetoJumpApex = .4f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    public float moveSpeed = 6;
    float defaultMoveSpeed;
    float gravity = -20;
    float jumpVelocity = 8;
    public Vector3 velocity;
    float velocityXSmoothing;
    public bool jumpThisFrame;
    public float lastDirection;
    public bool isFlying;

    public bool inKnockback = false;
    public bool canTakeKnockBack = true;
    public bool hitThisFrame = false;
    public bool inHitStun = false;
    float knockbackTimeOnGround;
    public bool inSpecialMovement = false;

    public int wallBounceCount;
    bool canWallBounce;

    bool inGrapple;

    public Controller2D controller;
    EnemySprite sprite;
    PlayerMovement player;
    EnemyBaseMelee melee;
    EnemyBaseBehaviour behaviour;

   

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        behaviour = GetComponent<EnemyBaseBehaviour>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //enemyBehaviour = GetComponent<EnemyBehaviour>();
        sprite = GetComponentInChildren<EnemySprite>();
        player = FindObjectOfType<PlayerMovement>();
        melee = GetComponentInChildren<EnemyBaseMelee>();

        gravity = -(2 * jumpHeight / Mathf.Pow(timetoJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timetoJumpApex;
        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inGrapple)
        {
            if ((controller.collisions.above || controller.collisions.below) && !inKnockback && !jumpThisFrame && !isFlying)
            {
                velocity.y = 0; //Sets y velocity to 0 if touching floor and no other effects in place
                moveSpeed = defaultMoveSpeed;
            }

            if (inKnockback && canWallBounce && (controller.collisions.left || controller.collisions.right))
            {
                velocity.x = (-velocity.x * 0.5f);
            }

            if (controller.collisions.below && inKnockback && !hitThisFrame && !inHitStun)
            {
                knockbackTimeOnGround += Time.deltaTime;
                if (knockbackTimeOnGround > 0.5f)
                {
                    StopKnockback();
                }

                //StartCoroutine("StartIFrames"); //Handles IFrames
                //StartCoroutine("FlashSpriteIFrames");
            }

            if (!inKnockback && !hitThisFrame && !inHitStun)
            {
                if (isFlying)
                {
                    RecieveVelocityFromBehaviour();
                }
                else
                {
                    if (!melee.inAttack)
                    {
                        RecieveVelocityFromBehaviour();
                    }
                }
                
            }
            
            


            float targetVelocityX;



            targetVelocityX = 0;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            if (!inHitStun)
            {
                if (!isFlying)
                {
                    velocity.y += gravity * Time.deltaTime;
                }
                else if (inKnockback)
                {
                    velocity.y += gravity * Time.deltaTime;
                }
                
            }
            else
            {

            }

            if (inSpecialMovement)
            {
                RecieveVelocityFromBehaviour();
            }

            controller.Move(velocity * Time.deltaTime, Vector2.zero);

            hitThisFrame = false;
            jumpThisFrame = false;
        }
        
    }

    public void TakeKnockback(Vector2 dir)
    {
        if (canTakeKnockBack)
        {
            hitThisFrame = true;
            inKnockback = true;
            velocity.x = dir.x;
            velocity.y = dir.y;
            inHitStun = true;
            gameObject.layer = 10;
            knockbackTimeOnGround = 0;
            CancelInvoke("EndHitStun");
            Invoke("EndHitStun", 0.2f / GameManager.Instance.ReturnEnemySpeed());

        }
        //StartCoroutine("FlashSprite");
    }

    public void TakeKnockback(Vector2 dir, MeleeHitbox.type type)
    {
        if (canTakeKnockBack)
        {
            //if (type == MeleeHitbox.type.Heavy)
            //{
            //    canWallBounce = true;
            //}
            //else
            //{
            //    canWallBounce = false;
            //}
            canWallBounce = true;
            melee.CancelAttacks();
            hitThisFrame = true;
            inKnockback = true;
            velocity.x = dir.x * (Random.Range(0.95f, 1.05f));
            velocity.y = dir.y;            
            inHitStun = true;
            gameObject.layer = 10;
            knockbackTimeOnGround = 0;
            CancelInvoke("EndHitStun");
            Invoke("EndHitStun", 0.1f / GameManager.Instance.ReturnEnemySpeed());

        }
        //StartCoroutine("FlashSprite");
    }

    public void StopKnockback()
    {
        inKnockback = false; //Stops knockback phase once the floor is hit
        gameObject.layer = 9;
        wallBounceCount = 0;
        StopAllCoroutines();
    }

    public void HitByCancel()
    {
        if (canTakeKnockBack && !behaviour.health.armoured)
        {
            hitThisFrame = true;
            inKnockback = true;
            velocity.x = 0;
            velocity.y = 0;
            inHitStun = true;
            melee.CancelAttacks();
            behaviour.gun.Cancel();
            gameObject.layer = 10;
            CancelInvoke("EndHitStun");
            Invoke("EndHitStun", 0.5f / GameManager.Instance.ReturnEnemySpeed());

        }
    }

    public void EndHitStun()
    {
        inHitStun = false;
    }

    public void StartInGrapple()
    {
        inGrapple = true;
    }

    public void EndInGrapple()
    {
        inGrapple = false;
    }

    public void RecieveVelocityFromBehaviour()
    {
        velocity.x = behaviour.velocity.x;
        if (isFlying || inSpecialMovement)
        {
            velocity.y = behaviour.velocity.y;
        }
    }

    public void Jump()
    {
        velocity.y = jumpVelocity;
        jumpThisFrame = true;
    }

    public void Jump(float xSpeed)
    {
        moveSpeed = xSpeed;
        velocity.y = jumpVelocity;
        
        jumpThisFrame = true;
        
    }


}
