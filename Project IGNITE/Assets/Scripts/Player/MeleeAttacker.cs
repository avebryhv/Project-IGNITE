using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    PlayerScriptFinder finder;
    public bool useAlternateInputs;
    public AttackObject testAttack;
    public AttackListObject attackList;
    public bool inAttack; //If player is currently in attack
    public int comboStage;
    public int airComboStage;
    public AttackObject currentAttack;
    public bool bufferedAttack;
    public bool bufferedHeavy;
    public float comboCooldown; //Time until combo is cancelled - allows for short gaps in attacks
    float comboCooldownTimer;
    public bool comboTimerPaused;
    public enum phase { None, Startup, Active, Endlag};
    public phase currentState;
    GameObject currentHitbox;

    public float heavyChargeTime;
    public bool chargingHeavy;
    public GameObject swordBeam;

    //Stinger Variables
    public float stingerRange;
    public bool inStinger;
    public float stingerTime;
    float stingerCounter;
    public LayerMask testMask;
    int airStingerCount;

    //Uppercut Variables
    public float uppercutTime;
    float uppercutCounter;
    public bool inUpperCut;

    //Helm Splitter Variables
    public bool inHelmSplitter;

    //Pause Combo Variables
    public float timeSinceLastLightAttackEnded;


    // Start is called before the first frame update
    void Start()
    {
        inAttack = false;
        comboStage = 0;
        airComboStage = 0;
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

        if (!inAttack && bufferedHeavy == true)
        {
            bufferedHeavy = false;
            HeavyAttackPressed();
        }

        if (!comboTimerPaused)
        {
            comboCooldownTimer += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            timeSinceLastLightAttackEnded += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            if (comboCooldownTimer >= comboCooldown)
            {
                comboTimerPaused = true;
                comboStage = 0;
                airComboStage = 0;
            }
        }

        if (inStinger)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(finder.movement.lastDirection, 0), 3.0f, testMask);         
            Debug.DrawRay(transform.position, new Vector2(finder.movement.lastDirection * 3, 0), Color.red);
            if (hit.collider != null)
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);
                if (hit.collider.tag == "EnemyHurtbox")
                {
                    Debug.Log("Stinger Hit");
                    StopStinger();
                }
            }

            stingerCounter += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            if (stingerCounter >= stingerTime)
            {
                StopStinger();
            }
        }

        if (inUpperCut)
        {
            uppercutCounter += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            if (uppercutCounter >= uppercutTime)
            {
                StopUppercut();
            }
        }

        if (chargingHeavy)
        {
            heavyChargeTime += Time.deltaTime;
            if (heavyChargeTime > 1)
            {
                HeavyAttackReleased();
            }
        }

        if (finder.controller.collisions.below)
        {
            airStingerCount = 0;

        }
        
    }

    public void LightAttackPressed()
    {
        if (finder.guard.inParry)
        {
            finder.guard.ExitParry();
        }
       
        if (finder.state.DecideCanAct())
        {
            comboTimerPaused = true;
            comboCooldownTimer = 0;
            comboStage++;
            airComboStage++;
            //if (comboStage > 3)
            //{
            //    comboStage = 1;
            //}
            DecideLightAttack();
        }
        else if (currentState == phase.Endlag || currentState == phase.Active)
        {
            bufferedAttack = true;
            bufferedHeavy = false;
            finder.movement.CancelJumpBuffer();
            finder.guard.CancelBuffer();
        }
        
        
        
    }

    public void HeavyAttackPressed()
    {
        if (finder.guard.inParry) //Attack to cancel a parry
        {
            currentAttack = attackList.parryPunch;
            finder.guard.ExitParry();
            AttackStartup();
        }
        else
        {
            if (finder.state.DecideCanAct())
            {
                DecideHeavyAttack();
            }
            else if (currentState == phase.Endlag || currentState == phase.Active)
            {
                bufferedHeavy = true;
                bufferedAttack = false;
                finder.movement.CancelJumpBuffer();
                finder.guard.CancelBuffer();
            }
        }
        
    }

    public void HeavyAttackHeld()
    {
        if (finder.state.DecideCanAct() && !(finder.controller.playerInput.x != 0 && finder.movement.lockedOn == true))
        {
            if (finder.controller.collisions.below)
            {
                inAttack = true;
                chargingHeavy = true;
            }
            else
            {
                currentAttack = attackList.airHeavy;
                AttackStartup();
            }
            
        }
    }

    public void HeavyAttackReleased()
    {
        if (chargingHeavy)
        {
            if (heavyChargeTime >= 1)
            {
                currentAttack = attackList.heavyCharged;
                GameObject beam = Instantiate(swordBeam, transform.position, transform.rotation);
                beam.GetComponent<SwordBeam>().SetDirection(finder.movement.lastDirection);
            }
            else
            {
                currentAttack = attackList.heavy;
            }
            chargingHeavy = false;
            heavyChargeTime = 0;
            AttackStartup();
        }
        
    }

    void ChargeHeavy()
    {
        heavyChargeTime += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
    }

    void ReleaseHeavy()
    {

    }

    void StartStinger()
    {
        finder.state.ResetStateForAttack();
        inAttack = true;
        currentAttack = attackList.stingerRush;
        Vector2 dir = new Vector2(finder.movement.lastDirection, 0);
        finder.movement.SetSpecialAttackMovement(dir, 25, 999);
        inStinger = true;
        stingerCounter = 0;
    }

    void StopStinger()
    {
        if (finder.controller.collisions.below)
        {
            finder.state.ResetStateForAttack();

            inStinger = false;
            currentAttack = attackList.stinger;
            finder.movement.StopSpecialAttackMovement();
            AttackStartup();
        }
        else
        {
            finder.state.ResetStateForAttack();

            inStinger = false;
            currentAttack = attackList.stinger;
            finder.movement.StopSpecialAttackMovement();
            AirAttackStartup();
        }
        
    }

    void StartUppercut()
    {
        currentAttack = attackList.uppercut;
        Vector2 dir = new Vector2(0, 1);
        finder.movement.SetSpecialAttackMovement(dir, 20, 999);
        inUpperCut = true;
        uppercutCounter = 0;
        AttackStartup();
    }

    void StopUppercut()
    {
        inUpperCut = false;
        finder.movement.StopSpecialAttackMovement();
    }

    void StartHelmSplitter()
    {
        finder.state.ResetStateForAttack();
        currentAttack = attackList.helmsplitter;
        Vector2 dir = new Vector2(0, -1);
        finder.movement.SetSpecialAttackMovement(dir, 40, 999);
        inHelmSplitter = true;
        AttackStartup();
    }

    public void EndHelmSplitter()
    {
        finder.state.ResetStateForAttack();
        inHelmSplitter = false;
        CancelInvoke();
        inAttack = false;
        finder.movement.StopSpecialAttackMovement();

        currentAttack = attackList.helmSplitterGround;
        AttackStartup();
    }

    void DecideLightAttack()
    {
        if (finder.movement.wallSliding)
        {
            currentAttack = attackList.wallSlideLight;
            AttackStartup();
        }
        else if (/*finder.movement.framesInAir > 2*/!finder.controller.collisions.below && !finder.movement.jumpPressedThisFrame) //Player is IN AIR
        {
            comboStage = 0;
            switch (airComboStage)
            {
                case 1:
                    currentAttack = attackList.airLight1;
                    break;
                case 2:
                    currentAttack = attackList.airLight2;                    
                    break;
                case 3:
                    if (timeSinceLastLightAttackEnded >= 0.1f)
                    {
                        finder.movement.SetSpecialAttackMovement(new Vector2(0,1), 3, 999);
                        currentAttack = attackList.airPause;
                    }
                    else
                    {
                        currentAttack = attackList.airLight3;
                    }                    
                    break;
                default:
                    Debug.Log("Invalid Attack Attempt");
                    break;
            }
            AirAttackStartup();
        }
        else //Player is ON GROUND
        {
            airComboStage = 0;
            switch (comboStage)
            {
                case 1:
                    currentAttack = attackList.light1;
                    break;
                case 2:
                    currentAttack = attackList.light2;
                    break;
                case 3:
                    if (timeSinceLastLightAttackEnded >= 0.1f)
                    {
                        currentAttack = attackList.lightB1;
                    }
                    else
                    {
                        currentAttack = attackList.light3;
                    }                    
                    break;
                case 4:
                    currentAttack = attackList.lightB2;
                    break;
                case 5:
                    currentAttack = attackList.lightB3;
                    break;
                default:
                    Debug.Log("Invalid Attack Attempt");
                    break;
            }
            AttackStartup();
        }
        if (currentAttack.endsCombo)
        {
            comboStage = 0;
            airComboStage = 0;
        }
    }

    void DecideHeavyAttack()
    {
        if (!useAlternateInputs)
        {
            if (finder.movement.wallSliding)
            {
                StartHelmSplitter();
            }
            else if (finder.controller.playerInput.x != 0 && finder.movement.lockedOn == true) //Lock-On Inputs
            {
                if (finder.input.CheckStickInputs(finder.input.inputComboList.powerGLeft) || finder.input.CheckStickInputs(finder.input.inputComboList.powerGRight))
                {
                    currentAttack = attackList.halfCircleDown;
                    AttackStartup();
                }
                else if (finder.movement.lastDirection == Mathf.Sign(finder.controller.playerInput.x))
                {
                    //Forward Heavy
                    if (finder.controller.collisions.below) //Grounded
                    {
                        StartStinger();
                    }
                    else //In Air
                    {
                        if (airStingerCount < 1) //Can only air stinger once in air
                        {
                            airStingerCount++;
                            StartStinger();
                        }

                    }
                    Debug.Log("Forward");
                }
                else
                {
                    //Back Heavy
                    if (finder.controller.collisions.below) //Grounded
                    {
                        StartUppercut();
                    }
                    else //In Air
                    {
                        StartHelmSplitter();
                    }
                    Debug.Log("Back");

                }
            }
            //else
            //{
            //    if (!finder.controller.collisions.below) //Grounded
            //    {
            //        currentAttack = attackList.airHeavy;
            //        AttackStartup();
            //    }                
            //}
            
        }        
        
    }

    void AttackStartup()
    {
        currentState = phase.Startup;
        finder.state.ResetStateForAttack();
        inAttack = true;
        Invoke("CreateHitbox", currentAttack.startUpTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    void CreateHitbox()
    {
        currentState = phase.Active;
        currentHitbox = Instantiate(currentAttack.hitboxObject, transform.position, transform.rotation, transform);
        currentHitbox.transform.localScale = new Vector3(currentHitbox.transform.localScale.x * finder.movement.lastDirection, currentHitbox.transform.localScale.y, currentHitbox.transform.localScale.z);
        currentHitbox.GetComponent<MeleeHitbox>().SetDirection(finder.movement.lastDirection);
        Invoke("StartEndLag", currentAttack.hitboxLingerTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    void StartEndLag()
    {
        currentState = phase.Endlag;
        Invoke("EndAttack", currentAttack.endingTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    public void EndAttack()
    {
        currentState = phase.None;
        inAttack = false;
        finder.movement.EndAirStall();
        comboTimerPaused = false;
        timeSinceLastLightAttackEnded = 0;
        finder.movement.StopSpecialAttackMovement();
    }

    public void CancelBuffer()
    {
        bufferedAttack = false;
        bufferedHeavy = false;
    }

    void AirAttackStartup()
    {
        currentState = phase.Startup;
        //finder.state.ResetStateForAttack();
        inAttack = true;
        Invoke("CreateHitbox", currentAttack.startUpTime / GameManager.Instance.ReturnPlayerSpeed());
        finder.movement.SetAirStall();
    }

    public void CancelAttacks()
    {
        inAttack = false;
        CancelInvoke();
        currentState = phase.None;
        finder.movement.EndAirStall();
        comboTimerPaused = false;
        if (currentHitbox != null)
        {
            currentHitbox.GetComponent<MeleeHitbox>().DestroyHitbox();
        }        
        inHelmSplitter = false;
        inUpperCut = false;
        inStinger = false;
        stingerCounter = 0;
        comboStage = 0;
        airComboStage = 0;
        chargingHeavy = false;
        heavyChargeTime = 0;
        finder.movement.StopSpecialAttackMovement();
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
