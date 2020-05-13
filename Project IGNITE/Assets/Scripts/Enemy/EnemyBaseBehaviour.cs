using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBehaviour : MonoBehaviour
{
    public EnemyBaseMovement movement;
    public EnemyBaseMelee melee;
    public EnemyBaseHealth health;
    public EnemySprite sprite;
    public EnemyBaseGun gun;
    

    public PlayerMovement player;

    public enum Mode { None, Attack }
    public Mode currentMode;
    public float attackRange;
    public bool isOnScreen;
    public bool activated;

    public Vector2 velocity;

    public bool canJump;

    public enum State { Idle, Knockback, Attack, Moving, Jump, SpecialKnockback, Evade}
    public State currentState;

    
    

    // Update is called once per frame
    void Update()
    {
        //CheckState();

        //if (currentMode == Mode.Attack)
        //{
        //    if (!movement.inKnockback && !movement.inHitStun && movement.controller.collisions.below && !movement.hitThisFrame && !melee.inAttack)
        //    {
        //        float xDifference = player.transform.position.x - transform.position.x;
        //        float yDifference = player.transform.position.y - transform.position.y;
        //        if (movement.lastDirection != Mathf.Sign(xDifference))
        //        {
        //            movement.lastDirection = Mathf.Sign(xDifference);
        //            sprite.TurnSprite();
        //        }

                

        //        if (Mathf.Abs(xDifference) <= attackRange)
        //        {
        //            melee.TriggerAttack();
        //        }
        //        else
        //        {
        //           velocity.x = movement.moveSpeed * movement.lastDirection;
        //        }


        //        //if (yDifference >= 2 && canJump && movement.controller.collisions.below && !melee.inAttack)
        //        //{
        //        //    movement.Jump();
        //        //}

        //    }
        //}
        //else
        //{
        //    velocity.x = 0;
        //}
    }

    public virtual void Actions()
    {

    }

    public virtual void CheckState()
    {
        if (movement.inSpecialKnockback) //Triggered when a boss breaks out of a combo or is parried
        {
            SetState(State.SpecialKnockback);
        }
        else if (movement.inKnockback) //State after getting hit by the player
        {
            SetState(State.Knockback);
        }
        else if (melee.inAttack) //Enemy is in the process of an attack
        {
            SetState(State.Attack);
        }
        else if (!movement.isFlying && !movement.controller.collisions.below) //Enemy is in the air, not performing any other action
        {
            SetState(State.Jump);
        }
        else if (Mathf.Abs(movement.velocity.x) >= 0.01) //Enemy is moving along the ground
        {
            SetState(State.Moving);
        }
        else //Enemy is not performing any actions
        {
            SetState(State.Idle);
        }
    }

    public void SetState(State s)
    {
        if (s != currentState) //Prevents duplicate animation triggers
        {
            currentState = s;
            sprite.CheckState(); //Set the animation of the enemy based on state
        }
    }

    void SendVelocity()
    {
        movement.RecieveVelocityFromBehaviour();
    }

    public bool CheckOnScreen()
    {
        Vector2 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if ((screenPoint.x >= 0 && screenPoint.x <= 1) && (screenPoint.y >= 0 && screenPoint.y <= 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Activate()
    {
        activated = true;
        CombatManager.Instance.AddActiveEnemy(this);
    }

    public void Deactivate()
    {
        activated = false;
        CombatManager.Instance.RemoveActiveEnemy(this);
    }
}
