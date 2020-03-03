﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float xDirection;
    public Vector2 movementDirection;
    public int damage;
    public Vector2 knockbackDirection;
    public float knockbackStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(movementDirection.x * xDirection, movementDirection.y);
        transform.Translate(dir * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            FindObjectOfType<PlayerHealth>().OnHit(this);
            GameManager.Instance.DoHitLag();
            Destroy(gameObject);
        }
    }

    public void SetDirection(float x)
    {
        xDirection = x;

    }
}
