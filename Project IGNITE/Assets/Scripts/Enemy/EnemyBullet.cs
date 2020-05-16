using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float xDirection;
    public Vector2 movementDirection;
    public int damage;
    public Vector2 knockbackDirection;
    public float knockbackStrength;
    public float speed;
    public bool trackingMovementMethod;
    public bool flipWithDirection;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = new Vector2(movementDirection.x * xDirection, movementDirection.y);
        
        if (trackingMovementMethod)
        {
            Vector2 dir = new Vector2(movementDirection.x, 0);
            transform.Translate(dir * Time.deltaTime * speed * Mathf.Sign(movementDirection.x));
        }
        else
        {
            Vector2 dir = new Vector2(movementDirection.x * xDirection, movementDirection.y);
            transform.Translate(dir * Time.deltaTime * speed);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            FindObjectOfType<PlayerHealth>().OnHit(this);
            GameManager.Instance.DoHitLag();
            Kill();
        }

        if (other.gameObject.layer == 8)
        {
            Kill();
        }
    }

    public void SetDirection(float x)
    {
        xDirection = x;
        if (flipWithDirection && xDirection < 0)
        {
            transform.Rotate(0, 0, 180);
            xDirection = 1;
        }
    }

    public void SetMovementDirection(Vector2 dir)
    {
        movementDirection = dir;
        xDirection = 1;
        if (trackingMovementMethod)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        
    }

    public void Kill()
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
