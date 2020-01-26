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

    // Start is called before the first frame update
    void Start()
    {
        inAttack = false;
        comboStage = 0;
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
            DecideAttack();
        }
        else if (currentState == phase.Endlag)
        {
            bufferedAttack = true;
        }
        
    }

    void DecideAttack()
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
