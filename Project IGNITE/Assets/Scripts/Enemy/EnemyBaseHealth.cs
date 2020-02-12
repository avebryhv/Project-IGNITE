using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseHealth : MonoBehaviour
{
    EnemyBaseMovement movement;
    EnemySprite sprite;
    public float maxHealth;
    public float currentHealth;
    public bool canTakeDamage;
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
            //currentHealth -= damage;
            movement.TakeKnockback(knockback, type);
            sprite.FlashColour(Color.red, 0.1f);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
