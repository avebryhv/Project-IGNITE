using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    PlayerScriptFinder finder;
    float lastDirection;
    bool canTurnSprite;
    public GameObject spriteHolder;
    SpriteRenderer[] spriteRendererList;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        canTurnSprite = true;
        lastDirection = 1;
        spriteRendererList = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastDirection != finder.movement.lastDirection)
        {
            if (canTurnSprite)
            {
                lastDirection = finder.movement.lastDirection;
                TurnPlayerSprite();
            }

        }
    }

    void TurnPlayerSprite()
    {
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * -1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }

    public void ChangeSpriteColour(Color col)
    {
        foreach (SpriteRenderer spr in spriteRendererList)
        {
            spr.color = col;
        }
    }

    public void SetAnimationTrigger(string name)
    {        
        animator.Play(name, 0, 0);
    }

    public void OnStateChanged(PlayerState.State newState)
    {
        switch (newState)
        {
            case PlayerState.State.Idle:
                SetAnimationTrigger("idle");
                break;
            case PlayerState.State.Walk:
                SetAnimationTrigger("walk");
                break;
            case PlayerState.State.Run:
                SetAnimationTrigger("run");
                break;
            case PlayerState.State.Jump:
                SetAnimationTrigger("jump");
                break;
            case PlayerState.State.Parry:
                SetAnimationTrigger("parry");
                break;
            case PlayerState.State.Block:
                SetAnimationTrigger("block");
                break;
            case PlayerState.State.Evade:
                SetAnimationTrigger("evade");
                break;
            case PlayerState.State.Attack:
                DecideMeleeAnimation();
                break;
            case PlayerState.State.Fall:
                SetAnimationTrigger("fall");
                break;
            case PlayerState.State.Knockback:
                break;
            default:
                break;
        }
    }

    void DecideMeleeAnimation()
    {
        string anim = finder.melee.currentAttack.animationName;
        SetAnimationTrigger(anim);
        Debug.Log(anim);
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }

    public void FlashColour(Color col, float time)
    {
        ChangeSpriteColour(col);
        Invoke("ResetSpriteColour", time);
    }

    void ResetSpriteColour()
    {
        ChangeSpriteColour(Color.white);
    }
}
