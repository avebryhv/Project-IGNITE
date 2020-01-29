﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstHitbox : MonoBehaviour
{
    public int damage;
    public float lingerTime;
    float destroyTimer;
    public float comboWeight; //How much landing this move will contribute to the combo meter
    public string name; //Name of attack
    List<GameObject> hitList; //Stores enemies that have already been hit, to prevent duplicate collisions

    // Start is called before the first frame update
    void Start()
    {
        hitList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
        if (destroyTimer >= lingerTime)
        {
            DestroyHitbox();
        }
    }

    public void DestroyHitbox()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyHurtbox")
        {
            if (!hitList.Contains(other.gameObject)) //Check if enemy has already been hit
            {
                Debug.Log("Hit Enemy Hurtbox");
                hitList.Add(other.gameObject);
                other.GetComponentInParent<EnemyBaseMovement>().HitByCancel();
                FindObjectOfType<ComboUI>().AddComboScore(comboWeight, name);
            }
        }
    }
   
}
