using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    PlayerScriptFinder finder;
    public enum State { Idle, Walk, Run, Jump, Block, Evade, Knockback};
    public State currentState;
    public bool canBufferInput;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void DecideState()
    {
        if (finder.movement.inKnockback)
        {
            SetState(State.Knockback);
        }
        //else if (/*finder.melee.inAttack*/)
        //{

        //}
        else if (finder.movement.jumpPressedThisFrame)
        {
            SetState(State.Jump);
        }
        else if (finder.movement.inDash)
        {
            SetState(State.Evade);
        }
        else if (finder.guard.isGuarding)
        {
            SetState(State.Block);
        }
        else if (finder.controller.collisions.below && finder.controller.playerInput.x != 0 && finder.movement.lockedOn)
        {
            SetState(State.Walk);
        }
        else if (finder.controller.collisions.below && finder.controller.playerInput.x != 0)
        {
            SetState(State.Run);
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
