using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    EnemyBaseMovement baseMovement;
    public EnemyBaseBehaviour behaviour;
    public EnemyBaseMelee melee;
    Animator anim;
    SpriteRenderer[] spriteRendererList;
    public GameObject spriteHolder;
    string currentAnim;
    string lastAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        baseMovement = GetComponentInParent<EnemyBaseMovement>();
        anim = GetComponent<Animator>();
        //melee = getc
        spriteRendererList = GetComponentsInChildren<SpriteRenderer>();
        //ChangeSpriteColour(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckState();
    }

    public void CheckState()
    {
        //if (baseMovement.inKnockback)
        //{
        //    currentAnim = "knockback";
        //}
        //else if (melee.inAttack)
        //{
        //    currentAnim = "attack";
        //}
        //else
        //{
        //    currentAnim = "idle";
        //}

        switch (behaviour.currentState)
        {
            case EnemyBaseBehaviour.State.Idle:
                currentAnim = "idle";
                break;
            case EnemyBaseBehaviour.State.Knockback:
                currentAnim = "knockback";
                break;
            case EnemyBaseBehaviour.State.Attack:
                currentAnim = "attack";
                break;
            case EnemyBaseBehaviour.State.Moving:
                currentAnim = "idle";
                break;
            case EnemyBaseBehaviour.State.Jump:
                break;
            default:
                break;
        }

        if (currentAnim != lastAnim)
        {
            lastAnim = currentAnim;
            PlayAnimation(currentAnim);
        }
    }

    void PlayAnimation(string toPlay)
    {
        anim.Play(toPlay, 0, 0);
    }

    public void ChangeSpriteColour(Color col)
    {
        foreach (SpriteRenderer spr in spriteRendererList)
        {
            spr.color = col;
        }
    }

    public void ResetState()
    {

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

    public void TurnSprite()
    {
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * -1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }

    public void SetDirection(float dir)
    {
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * dir, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }
}
