using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    PlayerScriptFinder finder;
    PlayerStatsUI ui;
    public CinemachineImpulseSource impulse;
    public int maxHealth;
    public int currentHealth;
    public bool canTakeDamage;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<PlayerStatsUI>();
        UpdateHealth(maxHealth);
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth(int newHealth)
    {
        currentHealth = newHealth;
        ui.SetHealthValue(currentHealth, maxHealth);
    }

    //public void OnHit(int damage, Vector2 knockback, EnemyMeleeHitbox.type type)
    //{
    //    if (finder.guard.isGuarding)
    //    {
    //        finder.guard.OnBlockAttack(damage, knockback, type);
    //    }
    //    else
    //    {
    //        Debug.Log("Hit By Enemy for " + damage);
    //    }
        
    //}

    public void OnHit(EnemyMeleeHitbox hitbox)
    {
        if (canTakeDamage)
        {
            if (finder.drones.currentState == DronesBehaviour.State.Barrier)
            {
                finder.drones.BreakGuard();
            }
            else
            {
                if (finder.guard.isGuarding)
                {
                    finder.guard.OnBlockAttack(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength, hitbox.attackType, hitbox.transform.position);
                }
                else
                {
                    Debug.Log("Hit By Enemy for " + hitbox.damage);
                    TakeDamage(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength, hitbox.attackType);
                }
            }
            
        }
        else if (finder.movement.inDash) //Dodging attack as a result of evading
        {
            FindObjectOfType<ComboUI>().AddComboScore(hitbox.damage, name, false);
        }
        
    }

    public void OnHit(EnemyBullet hitbox)
    {
        if (canTakeDamage)
        {
            if (finder.drones.currentState == DronesBehaviour.State.Barrier)
            {
                finder.drones.BreakGuard();
            }
            else
            {
                if (finder.guard.isGuarding)
                {
                    finder.guard.OnBlockAttack(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength, hitbox.transform.position);
                }
                else
                {
                    Debug.Log("Hit By Enemy for " + hitbox.damage);
                    TakeDamage(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength);
                }
            }

        }
        else if (finder.movement.inDash) //Dodging attack as a result of evading
        {
            FindObjectOfType<ComboUI>().AddComboScore(hitbox.damage, name, false);
        }

    }

    public void TakeDamage(int damage, Vector2 knockback, EnemyMeleeHitbox.type type)
    {
        if (finder.stats.inDT)
        {
            finder.sprite.FlashColour(Color.red, 0.1f);
            currentHealth -= Mathf.RoundToInt(damage / 2);
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
        }
        else
        {
            finder.movement.TakeKnockback(knockback);
            finder.melee.CancelAttacks();
            finder.sprite.FlashColour(Color.red, 0.1f);
            currentHealth -= damage;
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
            impulse.GenerateImpulse();
            StartIFrames(1);
        }
        
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        if (finder.stats.inDT)
        {
            finder.sprite.FlashColour(Color.red, 0.1f);
            currentHealth -= Mathf.RoundToInt(damage / 2);
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
        }
        else
        {
            finder.movement.TakeKnockback(knockback);
            finder.melee.CancelAttacks();
            finder.sprite.FlashColour(Color.red, 0.1f);
            currentHealth -= damage;
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
            impulse.GenerateImpulse();
            StartIFrames(1);
        }

    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        ui.SetHealthValue(currentHealth, maxHealth);
    }

    public void StartIFrames(float time)
    {
        canTakeDamage = false;
        Invoke("EndIFrames", time);
    }

    void EndIFrames()
    {
        CancelInvoke("EndIFrames");
        canTakeDamage = true;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
