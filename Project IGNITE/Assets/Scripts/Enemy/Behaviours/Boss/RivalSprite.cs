using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalSprite : EnemySprite
{
    public override void CheckState()
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
            case EnemyBaseBehaviour.State.Evade:
                currentAnim = "evade";
                break;
            case EnemyBaseBehaviour.State.SpecialKnockback:
                currentAnim = "knockback_special";
                break;
            case EnemyBaseBehaviour.State.Idle:
                currentAnim = "idle";
                break;
            case EnemyBaseBehaviour.State.Knockback:
                currentAnim = "knockback";
                break;
            case EnemyBaseBehaviour.State.Attack:
                currentAnim = currentAttackAnimName;
                break;
            case EnemyBaseBehaviour.State.Moving:
                currentAnim = "walk";
                break;
            case EnemyBaseBehaviour.State.Jump:
                currentAnim = "jump";
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
}
