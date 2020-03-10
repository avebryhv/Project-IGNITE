using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseHealth : MonoBehaviour
{
    EnemyBaseBehaviour behaviour;
    EnemySprite sprite;
    public GameObject sliceEffect;
    public int maxHealth;
    public int currentHealth;
    public bool canTakeDamage;
    public bool trainingEnemy;
    public bool armoured;
    public int currencyRewarded;
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

    public void TakeDamage(int damage, Vector2 knockback, MeleeHitbox.type type)
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
            
            sprite.FlashColour(Color.red, 0.1f);
            FindObjectOfType<EnemyStatsUI>().SetHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    private void Kill()
    {
        Instantiate(sliceEffect, transform.position, transform.rotation);
        CombatManager.Instance.RemoveActiveEnemy(behaviour);
        FindObjectOfType<PlayerUnlocks>().AddCurrency(currencyRewarded);
        Destroy(gameObject);
    }


}
