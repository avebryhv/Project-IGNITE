using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalHealth : EnemyBaseHealth
{
    public int maxKnockbacks;
    int knockbackCounter;
    public bool canKnockback;
    public bool inJuggle;
    public RivalBehaviour rivalB;
    public float phase2boundary;
    public float phase3boundary;
    public bool endLevelOnDeath;


    public override void TakeDamage(int damage, Vector2 knockback, MeleeHitbox.type type)
    {
        if (!behaviour.activated)
        {
            behaviour.Activate();
        }
        if (canTakeDamage)
        {
            if (!trainingEnemy)
            {
                currentHealth -= damage;
            }

            if (currentHealth <= phase2boundary && rivalB.currentPhase == RivalBehaviour.Phase.Phase1)
            {
                rivalB.SetPhase2();
            }

            if (currentHealth <= phase3boundary && rivalB.currentPhase == RivalBehaviour.Phase.Phase2)
            {
                rivalB.SetPhase3();
            }

            if (!armoured)
            {
                behaviour.movement.TakeKnockback(knockback, type);
            }
            else
            {
                if (canKnockback || inJuggle)
                {
                    

                    if (type == MeleeHitbox.type.Special)
                    {
                        ResetKnockback();
                        behaviour.movement.TakeSpecialKnockback(new Vector2(Mathf.Sign(knockback.x) * 20, 10));
                    }
                    else
                    {
                        if (!inJuggle)
                        {
                            inJuggle = true;
                            knockbackCounter = 1;
                            behaviour.movement.TakeKnockback(knockback, type);
                        }
                        else
                        {
                            knockbackCounter++;
                            if (knockbackCounter >= maxKnockbacks)
                            {
                                //rivalB.KnockbackEscape();
                                ResetKnockback();
                                behaviour.movement.TakeSpecialKnockback(new Vector2(Mathf.Sign(knockback.x) * 20, 10));
                            }
                            else
                            {
                                behaviour.movement.TakeKnockback(knockback, type);
                            }
                        }
                    }

                }
            }

            sprite.FlashColour(Color.red, 0.1f);
            FindObjectOfType<EnemyStatsUI>().SetHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                if (endLevelOnDeath)
                {
                    FindObjectOfType<PlayerUnlocks>().powerG = true; //Unlocks a move
                    LevelManager.Instance.EndLevelOnBossKill();
                }
                
                Kill();
            }
        }
    }

    public void ResetKnockback()
    {
        canKnockback = false;
        inJuggle = false;
        knockbackCounter = 0;
    }

    
}
