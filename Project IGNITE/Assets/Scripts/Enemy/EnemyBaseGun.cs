﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseGun : MonoBehaviour
{
    public EnemyBaseBehaviour behaviour;
    public GameObject baseBullet;
    public bool shootNow;
    public float cooldownAnimLength;
    public bool inShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    public void TriggerShot(float windUp)
    {
        behaviour.melee.inAttack = true;
        Invoke("Shoot", windUp);
    }

    public void Cancel()
    {
        CancelInvoke();
        behaviour.melee.inAttack = false;
    }

    public virtual void Shoot()
    {
        GameObject currentBullet = Instantiate(baseBullet, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(behaviour.player.transform.position.x - behaviour.transform.position.x));
        shootNow = false;
        AudioManager.Instance.PlaySFX("SFX/Enemies/Drone/shoot", 1f);
        Invoke("Cancel", cooldownAnimLength);
    }

    public void CancelShot()
    {

    }
}
