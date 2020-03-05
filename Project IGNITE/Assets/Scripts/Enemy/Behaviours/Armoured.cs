using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armoured : EnemyBaseBehaviour
{
    public float gunCooldown;
    public float gunRange;
    float gunTimer;

    public float rushCooldown;

    public float meleeCooldown;
    public float meleeRange;
    float meleeTimer;

    public Vector2 contactKnockback;
    public float contactDamage;

    public float jumpCooldown;
    float jumpTimer;
    bool inJump;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        //gunTimer = gunCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        Actions();
    }

    public override void Actions()
    {
        base.Actions();
        if (currentMode == Mode.Attack)
        {
            if (!movement.inKnockback && !movement.inHitStun && movement.controller.collisions.below && !movement.hitThisFrame && !melee.inAttack && !inJump)
            {
                float xDifference = player.transform.position.x - transform.position.x;
                float yDifference = player.transform.position.y - transform.position.y;
                if (movement.lastDirection != Mathf.Sign(xDifference))
                {
                    movement.lastDirection = Mathf.Sign(xDifference);
                    sprite.TurnSprite();
                }

                if (Mathf.Abs(xDifference) >= gunRange && CheckOnScreen())
                {
                    gunTimer -= Time.deltaTime;
                    if (gunTimer <= 0)
                    {
                        sprite.currentAttackAnimName = "shoot";
                        gun.TriggerShot(0.25f);
                        gun.TriggerShot(0.4f);

                        gunTimer = gunCooldown;
                    }
                }




                if (Mathf.Abs(xDifference) <= meleeRange && CheckOnScreen())
                {
                    sprite.currentAttackAnimName = "attack";
                    melee.TriggerAttack();
                }
                else
                {
                    velocity.x = movement.moveSpeed * movement.lastDirection;
                }


                //if (yDifference >= 2 && canJump && movement.controller.collisions.below && !melee.inAttack)
                //{
                //    movement.Jump();
                //}
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    inJump = true;
                    movement.Jump();
                    jumpTimer = jumpCooldown;
                }

            }
            else if (inJump)
            {
                if (movement.controller.collisions.below)
                {
                    inJump = false;
                }
            }
        }
        else
        {
            velocity.x = 0;
        }
    }
}
