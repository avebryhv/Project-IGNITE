using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBeam : MonoBehaviour
{
    public float moveSpeed;
    public int damage;
    public Vector2 knockbackDirection;
    public Vector2 knockbackStrength;
    float currentDirection;
    public float comboWeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = new Vector2(currentDirection * moveSpeed, 0);
        transform.Translate(velocity * Time.deltaTime);

        Vector2 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if ((screenPoint.x >= 0 && screenPoint.x <= 1) && (screenPoint.y >= 0 && screenPoint.y <= 1))
        {
            //On Screen
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHurtbox")
        {
            collision.GetComponentInParent<EnemyBaseHealth>().TakeDamage(damage, new Vector2(), MeleeHitbox.type.Special);
            FindObjectOfType<ComboUI>().AddComboScore(comboWeight, name);
            GameManager.Instance.DoHitLag();
            Destroy(gameObject);
        }
    }

    public void SetDirection(float dir)
    {
        currentDirection = dir;
        transform.localScale = new Vector3(dir, 1, 1);
    }
}
