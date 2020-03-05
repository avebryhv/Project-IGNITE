using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseHealth : MonoBehaviour
{
    EnemyBaseMovement movement;
    EnemySprite sprite;
    public GameObject sliceEffect;
    public int maxHealth;
    public int currentHealth;
    public bool canTakeDamage;
    public bool trainingEnemy;
    public bool armoured;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<EnemyBaseMovement>();
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
        if (canTakeDamage)
        {
            if (!trainingEnemy)
            {
                currentHealth -= damage;
            }
            if (!armoured)
            {
                movement.TakeKnockback(knockback, type);
            }
            
            sprite.FlashColour(Color.red, 0.1f);
            FindObjectOfType<EnemyStatsUI>().SetHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Instantiate(sliceEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    
}
