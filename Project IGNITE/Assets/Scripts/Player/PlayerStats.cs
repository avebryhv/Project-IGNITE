using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerScriptFinder finder;
    public int health;
    public int maxHealth;

    public float dtCharge;
    public float dtMax;

    PlayerStatsUI ui;
    //Burst Variables
    public GameObject burstPrefab;
    public float burstCooldown;
    float burstTimer;
    bool canBurst;

    //DT Variables
    public float DTDrainRate;
    public bool inDT;
    public float minTimeInDT;
    float dtTimeCounter;
    public bool unlimitedDT;
    float dtHealthGainTimer;
    public ParticleSystem burstParticles;

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
            burstTimer += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            if (burstTimer >= burstCooldown)
            {
                canBurst = true;
                burstTimer = 0;
            }
        }

        if (inDT)
        {
            dtTimeCounter += Time.deltaTime;
            IncreaseDT(Time.deltaTime * -DTDrainRate);

            dtHealthGainTimer += Time.deltaTime;
            if (dtHealthGainTimer > 0.5f)
            {
                finder.health.IncreaseHealth(1);
                dtHealthGainTimer = 0;
            }

            if (dtCharge <= 0)
            {
                ExitDT();
            }
        }

        if (unlimitedDT)
        {
            IncreaseDT(10);
        }
    }

    public void ChangeHealth(int amount)
    {

    }

    public void IncreaseDT(float amount)
    {
        dtCharge += amount;
        dtCharge = Mathf.Clamp(dtCharge, 0, dtMax);
        ui.SetDTValue(dtCharge, dtMax);
    }

    public void IncreaseMaxDT(float amount)
    {
        dtMax += amount;
        dtCharge = dtMax;
        ui.SetDTValue(dtCharge, dtMax);
    }

    public void LoadMaxDT(float amount)
    {
        ui = FindObjectOfType<PlayerStatsUI>();
        dtMax = amount;
        dtCharge = amount;
        ui.SetDTValue(dtCharge, dtMax);
    }

    public void Burst()
    {
        if (canBurst && dtCharge >= 10 && finder.unlocks.burst)
        {
            GameObject burst = Instantiate(burstPrefab, transform.position, transform.rotation, transform);
            burstParticles.Play();
            IncreaseDT(-10);
            finder.melee.CancelAttacks();
            if (!finder.controller.collisions.below)
            {
                finder.movement.SetAirStall();
            }
            finder.melee.inAttack = true;
            finder.melee.currentState = MeleeAttacker.phase.Endlag;
            //finder.movement.canDoubleJump = true;
            finder.movement.ResetDash();
            burstTimer = 0;
            canBurst = false;
            Invoke("EndAirStall", 0.2f);
        }
        
    }

    public void DTButtonPressed()
    {
        if (inDT)
        {
            if (dtTimeCounter >= minTimeInDT)
            {
                ExitDT();
            }
        }
        else
        {
            if (dtCharge >= 30)
            {
                EnterDT();
            }
        }
    }

    void EnterDT()
    {
        inDT = true;
        dtTimeCounter = 0;
        //finder.movement.dtMoveSpeedModifier = 1.2f;
        GameManager.Instance.playerSpeed = 1.2f;
        //finder.sprite.animator.speed = 0.2f;
        finder.sprite.SetDTSprites();      
    }

    void ExitDT()
    {
        inDT = false;
        //finder.movement.dtMoveSpeedModifier = 1.0f;
        GameManager.Instance.playerSpeed = 1f;
        finder.sprite.SetNormalSprites();
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
