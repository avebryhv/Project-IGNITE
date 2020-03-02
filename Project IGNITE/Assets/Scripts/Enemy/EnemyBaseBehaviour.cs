using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseBehaviour : MonoBehaviour
{
    public EnemyBaseMovement movement;
    public EnemyBaseMelee melee;
    public EnemyBaseHealth health;
    public EnemySprite sprite;

    public PlayerMovement player;

    public enum Mode { None, Attack }
    public Mode currentMode;
    public float attackRange;

    public Vector2 velocity;

    public bool canJump;

    public enum State { Idle, Knockback, Attack, Moving, Jump}
    public State currentState;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

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

    protected void CheckState()
    {
        if (movement.inKnockback)
        {
            SetState(State.Knockback);
        }
        else if (melee.inAttack)
        {
            SetState(State.Attack);
        }
        else
        {
            SetState(State.Idle);
        }
    }

    void SetState(State s)
    {
        if (s != currentState)
        {
            currentState = s;
            sprite.CheckState();
        }
    }

    void SendVelocity()
    {
        movement.RecieveVelocityFromBehaviour();
    }
}
