using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseHealth : MonoBehaviour
{
    public EnemyBaseBehaviour behaviour;
    public EnemySprite sprite;
    public GameObject sliceEffect;
    public int maxHealth;
    public int currentHealth;
    public bool canTakeDamage;
    public bool trainingEnemy;
    public bool armoured;
    public bool armouredKnockback;
    public int currencyRewarded;
    public GameObject currencyParticles;
    float armourKnockbackHits;
    // Start is called before the first frame update
    void Start()
    {
        behaviour = GetComponent<EnemyBaseBehaviour>();
        sprite = GetComponentInChildren<EnemySprite>();
        currentHealth = maxHealth;
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int damage, Vector2 knockback, MeleeHitbox.type type)
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


            if (!armoured)
            {
                behaviour.movement.TakeKnockback(knockback, type);
            }
            else
            {
                if (armouredKnockback)
                {
                    behaviour.movement.TakeKnockback(knockback, type);                    
                }
            }
            
            sprite.FlashColour(Color.red, 0.1f);
            FindObjectOfType<EnemyStatsUI>().SetHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        GameObject sliceEff = Instantiate(sliceEffect, transform.position, transform.rotation);
        sliceEff.GetComponent<EnemySliceEffect>().SetFacingDirection(behaviour.movement.lastDirection);
        CombatManager.Instance.RemoveActiveEnemy(behaviour);
        Instantiate(currencyParticles, transform.position, transform.rotation);
        FindObjectOfType<PlayerUnlocks>().AddCurrency(currencyRewarded);
        Destroy(gameObject);
    }

    public void SetArmourKnockback(float time, int hitCount)
    {
        armouredKnockback = true;
        armourKnockbackHits = hitCount;
    }

    public void SetNewMaxHealth(int amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }


}
