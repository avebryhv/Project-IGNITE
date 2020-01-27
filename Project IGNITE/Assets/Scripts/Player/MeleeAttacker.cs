using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    PlayerScriptFinder finder;
    public AttackObject testAttack;
    public AttackListObject attackList;
    public bool inAttack; //If player is currently in attack
    public int comboStage;
    public AttackObject currentAttack;
    public bool bufferedAttack;
    public float comboCooldown; //Time until combo is cancelled - allows for short gaps in attacks
    float comboCooldownTimer;
    public bool comboTimerPaused;
    public enum phase { None, Startup, Active, Endlag};
    public phase currentState;

    public float heavyChargeTime;
    public bool chargingHeavy;
    public float stingerRange;
    public bool inStinger;
    public float stingerTime;
    float stingerCounter;
    public LayerMask testMask;

    // Start is called before the first frame update
    void Start()
    {
        inAttack = false;
        comboStage = 0;
        chargingHeavy = false;
        inStinger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAttack && bufferedAttack == true)
        {
            bufferedAttack = false;
            LightAttackPressed();
        }

        if (!comboTimerPaused)
        {
            comboCooldownTimer += Time.deltaTime;
            if (comboCooldownTimer >= comboCooldown)
            {
                comboTimerPaused = true;
                comboStage = 0;
            }
        }

        if (inStinger)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(finder.movement.lastDirection, 0), 2.0f, testMask);
            Debug.DrawRay(transform.position, new Vector2(finder.movement.lastDirection * 2, 0), Color.red);
            if (hit.collider != null)
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);
                if (hit.collider.tag == "EnemyHurtbox")
                {
                    Debug.Log("Stinger Hit");
                    StopStinger();
                }
            }

            stingerCounter += Time.deltaTime;
            if (stingerCounter >= stingerTime)
            {
                StopStinger();
            }
        }
        
    }

    public void LightAttackPressed()
    {
        if (!inAttack)
        {
            comboTimerPaused = true;
            comboCooldownTimer = 0;
            comboStage++;
            if (comboStage > 3)
            {
                comboStage = 1;
            }
            DecideLightAttack();
        }
        else if (currentState == phase.Endlag)
        {
            bufferedAttack = true;
        }
        
    }

    public void HeavyAttackPressed()
    {
        if (!inAttack)
        {
            if (finder.controller.playerInput.x != 0 && finder.movement.lockedOn == true)
            {
                if (finder.movement.lastDirection == finder.controller.playerInput.x)
                {
                    //Forward Heavy
                    Debug.Log("Forward");
                    StartStinger();
                }
                else
                {
                    //Back Heavy
                    Debug.Log("Back");
                }
            }
            else
            {
                //Charge Attack
            }
        }
    }

    public void HeavyAttackReleased()
    {

    }

    void ChargeHeavy()
    {
        heavyChargeTime += Time.deltaTime;
    }

    void ReleaseHeavy()
    {

    }

    void StartStinger()
    {
        inAttack = true;
        Vector2 dir = new Vector2(finder.movement.lastDirection, 0);
        finder.movement.SetSpecialAttackMovement(dir, 15, 999);
        inStinger = true;
        stingerCounter = 0;
    }

    void StopStinger()
    {
        inStinger = false;
        currentAttack = attackList.heavy;
        finder.movement.StopSpecialAttackMovement();
        AttackStartup();
    }

    void DecideLightAttack()
    {
        if (finder.movement.framesInAir > 2) //Player is IN AIR
        {
            switch (comboStage)
            {
                case 1:
                    currentAttack = attackList.airLight1;
                    break;
                case 2:
                    currentAttack = attackList.airLight2;
                    break;
                case 3:
                    currentAttack = attackList.airLight3;
                    break;
                default:
                    Debug.Log("Invalid Attack Attempt");
                    break;
            }
            AirAttackStartup();
        }
        else //Player is ON GROUND
        {
            switch (comboStage)
            {
                case 1:
                    currentAttack = attackList.light1;
                    break;
                case 2:
                    currentAttack = attackList.light2;
                    break;
                case 3:
                    currentAttack = attackList.light3;
                    break;
                default:
                    Debug.Log("Invalid Attack Attempt");
                    break;
            }
            AttackStartup();
        }
    }

    void AttackStartup()
    {
        currentState = phase.Startup;
        inAttack = true;
        Invoke("CreateHitbox", currentAttack.startUpTime);
    }

    void CreateHitbox()
    {
        currentState = phase.Active;
        GameObject newHitbox = Instantiate(currentAttack.hitboxObject, transform.position, transform.rotation, transform);
        newHitbox.transform.localScale = new Vector3(newHitbox.transform.localScale.x * finder.movement.lastDirection, newHitbox.transform.localScale.y, newHitbox.transform.localScale.z);
        Invoke("StartEndLag", currentAttack.hitboxLingerTime);
    }

    void StartEndLag()
    {
        currentState = phase.Endlag;
        Invoke("EndAttack", currentAttack.endingTime);
    }

    public void EndAttack()
    {
        currentState = phase.None;
        inAttack = false;
        finder.movement.EndAirStall();
        comboTimerPaused = false;
    }

    public void CancelBuffer()
    {
        bufferedAttack = false;
    }

    void AirAttackStartup()
    {
        inAttack = true;
        Invoke("CreateHitbox", currentAttack.startUpTime);
        finder.movement.SetAirStall();
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
