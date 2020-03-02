using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic : EnemyBaseBehaviour
{
    

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
            if (!movement.inKnockback && !movement.inHitStun && movement.controller.collisions.below && !movement.hitThisFrame && !melee.inAttack)
            {
                float xDifference = player.transform.position.x - transform.position.x;
                float yDifference = player.transform.position.y - transform.position.y;
                if (movement.lastDirection != Mathf.Sign(xDifference))
                {
                    movement.lastDirection = Mathf.Sign(xDifference);
                    sprite.TurnSprite();
                }



                if (Mathf.Abs(xDifference) <= attackRange)
                {
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

                if ((movement.controller.collisions.left && xDifference < 0) || (movement.controller.collisions.right && xDifference > 0))
                {
                    movement.Jump();
                }

            }
        }
        else
        {
            velocity.x = 0;
        }
    }
}
