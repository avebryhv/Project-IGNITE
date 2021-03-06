﻿using System.Collections;
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
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<PlayerStatsUI>();
        //UpdateHealth(maxHealth);
        impulse = GetComponent<CinemachineImpulseSource>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth(int newHealth)
    {
        ui = FindObjectOfType<PlayerStatsUI>();
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
            if (finder.guard.isGuarding)
            {
                finder.guard.OnBlockAttack(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength, hitbox.attackType, hitbox.transform.position);
            }
            else
            {
                
                if (finder.drones.currentState == DronesBehaviour.State.Barrier)
                {
                    finder.drones.BreakGuard();
                }
                else
                {
                    //Debug.Log("Hit By Enemy for " + hitbox.damage);
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
            if (finder.guard.isGuarding)
            {
                finder.guard.OnBlockAttack(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength, hitbox.transform.position);
            }
            else
            {
                
                if (finder.drones.currentState == DronesBehaviour.State.Barrier)
                {
                    finder.drones.BreakGuard();
                }
                else
                {
                    //Debug.Log("Hit By Enemy for " + hitbox.damage);
                    TakeDamage(hitbox.damage, hitbox.knockbackDirection * hitbox.knockbackStrength);
                }
            }

        }
        else if (finder.movement.inDash) //Dodging attack as a result of evading
        {
            FindObjectOfType<ComboUI>().AddComboScore(hitbox.damage, name, false);
        }

    }

    public void OnHit(int damage, Vector2 knockbackDir, float knockbackStr, Vector2 hitboxPos)
    {
        if (canTakeDamage)
        {
            if (finder.guard.isGuarding)
            {
                finder.guard.OnBlockAttack(damage, knockbackDir * knockbackStr, hitboxPos);
            }
            else
            {
                
                if (finder.drones.currentState == DronesBehaviour.State.Barrier)
                {
                    finder.drones.BreakGuard();
                }
                else
                {
                    //Debug.Log("Hit By Enemy for " + hitbox.damage);
                    TakeDamage(damage, knockbackDir * knockbackStr);
                }
            }

        }
        else if (finder.movement.inDash) //Dodging attack as a result of evading
        {
            FindObjectOfType<ComboUI>().AddComboScore(damage, name, false);
        }
    }

    public void TakeDamage(int damage, Vector2 knockback, EnemyMeleeHitbox.type type)
    {
        if (finder.stats.inDT)
        {
            finder.sprite.FlashColour(Color.red, 0.1f);
            SubtractHealth(Mathf.RoundToInt(damage / 2));
            LevelManager.Instance.AddDamage(damage / 2);
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
            ui.OnHealthDamage();
            AudioManager.Instance.PlaySFX("SFX/Player/playerDamage", 0.5f);
            GameManager.Instance.TriggerRumble(0.1f);
        }
        else
        {
            finder.movement.TakeKnockback(knockback);
            finder.melee.CancelAttacks();
            finder.sprite.FlashColour(Color.red, 0.1f);
            SubtractHealth(damage);
            LevelManager.Instance.AddDamage(damage);
            FindObjectOfType<ComboUI>().ReduceComboScore(25);
            ui.SetHealthValue(currentHealth, maxHealth);
            ui.OnHealthDamage();
            AudioManager.Instance.PlaySFX("SFX/Player/playerDamage", 0.5f);
            GameManager.Instance.TriggerRumble(0.1f);
            impulse.GenerateImpulse();
            if (type != EnemyMeleeHitbox.type.ComboMid)
            {
                finder.sprite.StartIFrameFlash();
                StartIFrames(1);
            }
            
        }
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        if (finder.stats.inDT)
        {
            finder.sprite.FlashColour(Color.red, 0.1f);
            SubtractHealth(Mathf.RoundToInt(damage / 2));
            LevelManager.Instance.AddDamage(damage / 2);
            FindObjectOfType<ComboUI>().ReduceComboScore(damage);
            ui.SetHealthValue(currentHealth, maxHealth);
            ui.OnHealthDamage();
            AudioManager.Instance.PlaySFX("SFX/Player/playerDamage", 0.5f);
            GameManager.Instance.TriggerRumble(0.1f);
        }
        else
        {
            finder.movement.TakeKnockback(knockback);
            finder.melee.CancelAttacks();
            finder.sprite.FlashColour(Color.red, 0.1f);
            finder.sprite.StartIFrameFlash();
            SubtractHealth(damage);
            LevelManager.Instance.AddDamage(damage);
            FindObjectOfType<ComboUI>().ReduceComboScore(25);
            ui.SetHealthValue(currentHealth, maxHealth);
            ui.OnHealthDamage();
            AudioManager.Instance.PlaySFX("SFX/Player/playerDamage", 0.5f);
            GameManager.Instance.TriggerRumble(0.1f);
            impulse.GenerateImpulse();
            StartIFrames(1);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

    }

    void SubtractHealth(int amount)
    {
        int lastHealth = currentHealth;
        currentHealth -= amount;

        if (currentHealth <= 30 && lastHealth > 30)
        {
            finder.messages.CreateMinorMessage("LOW HEALTH", Color.red, 1f);
            AudioManager.Instance.PlaySFX("SFX/Player/lowHealth", 1f);
        }
    }
    

    void Die()
    {
        finder.state.SetDeathState();
        finder.input.allowPlayerInput = false;
        CombatManager.Instance.DeactivateAll();
        FindObjectOfType<DeathScreen>().ShowScreen();
        isDead = true;
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

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        UpdateHealth(maxHealth);
    }

    public void LoadMaxHealth(int amount)
    {
        maxHealth = amount;
        UpdateHealth(maxHealth);
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
