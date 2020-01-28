using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerScriptFinder finder;
    public int health;
    public int maxHealth;

    public int dtCharge;
    public int dtMax;

    PlayerStatsUI ui;
    //Burst Variables
    public GameObject burstPrefab;
    public float burstCooldown;
    float burstTimer;
    bool canBurst;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<PlayerStatsUI>();
        health = maxHealth;
        dtCharge = dtMax;
        ui.SetHealthValue(health, maxHealth);
        ui.SetDTValue(dtCharge, dtMax);
        canBurst = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canBurst)
        {
            burstTimer += Time.deltaTime;
            if (burstTimer >= burstCooldown)
            {
                canBurst = true;
                burstTimer = 0;
            }
        }
    }

    public void ChangeHealth(int amount)
    {

    }

    public void ChangeDT(int amount)
    {

    }

    public void Burst()
    {
        if (canBurst)
        {
            GameObject burst = Instantiate(burstPrefab, transform.position, transform.rotation, transform);
            finder.melee.CancelAttacks();
            if (!finder.controller.collisions.below)
            {
                finder.movement.SetAirStall();
            }
            finder.melee.inAttack = true;
            finder.melee.currentState = MeleeAttacker.phase.Endlag;
            burstTimer = 0;
            canBurst = false;
            Invoke("EndAirStall", 0.4f);
        }
        
    }

    void EndAirStall()
    {
        finder.melee.inAttack = false;
        finder.melee.currentState = MeleeAttacker.phase.None;
        finder.movement.EndAirStall();
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
