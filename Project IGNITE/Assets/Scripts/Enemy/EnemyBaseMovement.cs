using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timetoJumpApex = .4f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float moveSpeed = 6;
    float gravity = -20;
    float jumpVelocity = 8;
    public Vector3 velocity;
    float velocityXSmoothing;
    public bool jumpThisFrame;

    public bool inKnockback = false;
    public bool canTakeKnockBack = true;
    bool hitThisFrame = false;
    bool inHitStun = false;
    float knockbackTimeOnGround;

    public int wallBounceCount;
    bool canWallBounce;

    bool inGrapple;

    Controller2D controller;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //enemyBehaviour = GetComponent<EnemyBehaviour>();
        sprite = GetComponent<SpriteRenderer>();

        gravity = -(2 * jumpHeight / Mathf.Pow(timetoJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timetoJumpApex;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inGrapple)
        {
            if ((controller.collisions.above || controller.collisions.below) && !inKnockback && !jumpThisFrame)
            {
                velocity.y = 0; //Sets y velocity to 0 if touching floor and no other effects in place
            }

            if (inKnockback && canWallBounce && (controller.collisions.left || controller.collisions.right))
            {
                velocity.x = (-velocity.x * 0.5f);
            }

            if (controller.collisions.below && inKnockback && !hitThisFrame)
            {
                knockbackTimeOnGround += Time.deltaTime;
                if (knockbackTimeOnGround > 0.5f)
                {
                    StopKnockback();
                }

                //StartCoroutine("StartIFrames"); //Handles IFrames
                //StartCoroutine("FlashSpriteIFrames");
            }


            float targetVelocityX;



            targetVelocityX = 0;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            if (!inHitStun)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {

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
            Invoke("EndHitStun", 0.1f);

        }
        //StartCoroutine("FlashSprite");
    }

    public void TakeKnockback(Vector2 dir, MeleeHitbox.type type)
    {
        if (canTakeKnockBack)
        {
            if (type == MeleeHitbox.type.Heavy)
            {
                canWallBounce = true;
            }
            else
            {
                canWallBounce = false;
            }
            hitThisFrame = true;
            inKnockback = true;
            velocity.x = dir.x;
            velocity.y = dir.y;
            inHitStun = true;
            gameObject.layer = 10;
            knockbackTimeOnGround = 0;
            CancelInvoke("EndHitStun");
            Invoke("EndHitStun", 0.1f);

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
        if (canTakeKnockBack)
        {
            hitThisFrame = true;
            inKnockback = true;
            velocity.x = 0;
            velocity.y = 0;
            inHitStun = true;
            gameObject.layer = 10;
            CancelInvoke("EndHitStun");
            Invoke("EndHitStun", 0.5f);

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

    //IEnumerator StartIFrames()
    //{
    //    //enemyHealth.canTakeDamage = false;
    //    //yield return new WaitForSeconds(enemyBehaviour.invTime);
    //    //enemyHealth.canTakeDamage = true;
    //}

    //IEnumerator FlashSpriteIFrames()
    //{
    //    do
    //    {
    //        sprite.color = Color.red;
    //        yield return new WaitForSeconds(.1f);
    //        sprite.color = Color.white;
    //        yield return new WaitForSeconds(.1f);
    //    } while (enemyHealth.canTakeDamage == false);
    //}

    //IEnumerator FlashSprite()
    //{

    //    sprite.color = Color.red;
    //    yield return new WaitForSeconds(.1f);
    //    sprite.color = Color.white;
    //    yield return new WaitForSeconds(.1f);

    //}
}
