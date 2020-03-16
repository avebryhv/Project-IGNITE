using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    PlayerScriptFinder finder;
    public enum State { Idle, Walk, Run, Jump, WallSlide, Fall, Block, Evade, Knockback, Attack, Parry, Dead};
    public State currentState;
    public bool canBufferInput;

    // Start is called before the first frame update
    void Start()
    {
        SetState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        DecideState();
    }

    public void SetState(State s)
    {
        if (s != currentState)
        {
            currentState = s;
            finder.sprite.OnStateChanged(currentState);
        }        
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }

    public void DecideCanBuffer()
    {

    }

    public bool DecideCanAct()
    {
        if (finder.melee.inAttack || finder.movement.jumpPressedThisFrame || finder.movement.inAttackMovement || finder.movement.inDash || finder.guard.isGuarding || finder.movement.inKnockback)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ResetStateForAttack()
    {
        if (finder.controller.collisions.below)
        {
            SetState(State.Idle);
        }
        else
        {
            SetState(State.Fall);
        }
        
        //currentState = State.Idle;
    }

    public void DecideState()
    {
        if (currentState != State.Dead)
        {
            if (finder.movement.inKnockback)
            {
                SetState(State.Knockback);
            }
            else if (finder.melee.inAttack)
            {
                SetState(State.Attack);
            }
            else if (finder.movement.jumpPressedThisFrame)
            {
                SetState(State.Jump);
            }
            else if (finder.movement.inDash)
            {
                SetState(State.Evade);
            }
            else if (finder.guard.inParry)
            {
                SetState(State.Parry);
            }
            else if (finder.guard.isGuarding)
            {
                SetState(State.Block);
            }
            else if (finder.movement.wallSliding)
            {
                SetState(State.WallSlide);
            }
            else if (finder.controller.collisions.below && finder.controller.playerInput.x != 0 && finder.movement.lockedOn)
            {
                SetState(State.Walk);
            }
            else if (finder.controller.collisions.below && finder.controller.playerInput.x != 0)
            {
                SetState(State.Run);
            }
            else if (!finder.controller.collisions.below)
            {
                SetState(State.Fall);
            }
            else if (finder.controller.collisions.below)
            {
                SetState(State.Idle);
            }
            else
            {
                SetState(currentState);
            }
        }
        
    }

    public void SetDeathState()
    {
        SetState(State.Dead);
    }
}
