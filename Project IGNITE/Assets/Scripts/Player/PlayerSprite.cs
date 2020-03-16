using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerSprite : MonoBehaviour
{
    PlayerScriptFinder finder;
    float lastDirection;
    bool canTurnSprite;
    public GameObject spriteHolder;
    public GameObject dronesHolder;
    SpriteRenderer[] spriteRendererList;
    public Animator animator;
    DTSpriteToggler[] dtSpriteList;
    public ParticleSystem DTParticles;
    public Light2D visorLight;

    // Start is called before the first frame update
    void Start()
    {
        canTurnSprite = true;
        lastDirection = 1;
        spriteRendererList = GetComponentsInChildren<SpriteRenderer>();
        dtSpriteList = GetComponentsInChildren<DTSpriteToggler>();
        visorLight.color = Color.blue;
        SetAnimationTrigger("idle");
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
        dronesHolder.transform.localScale = new Vector3(dronesHolder.transform.localScale.x * -1, dronesHolder.transform.localScale.y, dronesHolder.transform.localScale.z);
    }

    public void ChangeSpriteColour(Color col)
    {
        foreach (SpriteRenderer spr in spriteRendererList)
        {
            spr.color = col;
        }
    }

    public void SetDTSprites()
    {
        for (int i = 0; i < dtSpriteList.Length; i++)
        {
            dtSpriteList[i].SetDTSprite();
        }
        DTParticles.Play();
        visorLight.color = Color.red;
    }

    public void SetNormalSprites()
    {
        for (int i = 0; i < dtSpriteList.Length; i++)
        {
            dtSpriteList[i].SetNormalSprite();
        }
        DTParticles.Stop();
        visorLight.color = Color.blue;
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
            case PlayerState.State.WallSlide:
                SetAnimationTrigger("wallSlide");
                break;
            case PlayerState.State.Evade:
                SetAnimationTrigger("dashForward");
                break;
            case PlayerState.State.Attack:
                DecideMeleeAnimation();
                break;
            case PlayerState.State.Fall:
                SetAnimationTrigger("fall");
                break;
            case PlayerState.State.Knockback:
                SetAnimationTrigger("knockback");
                break;
            case PlayerState.State.Dead:
                SetAnimationTrigger("death");
                break;
            default:
                break;
        }
    }

    void DecideMeleeAnimation()
    {
        string anim;
        if (finder.melee.chargingHeavy)
        {
            anim = "chargingHeavy";
        }
        else
        {
            anim = finder.melee.currentAttack.animationName;
        }
        
        SetAnimationTrigger(anim);
        //Debug.Log(anim);
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
