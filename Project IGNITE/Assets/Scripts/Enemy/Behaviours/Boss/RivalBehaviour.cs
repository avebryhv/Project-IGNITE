using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalBehaviour : EnemyBaseBehaviour
{
    public float currentCooldown;
    public float actionCooldown;
    public Collider2D activeRange;
    public RivalHealth rHealth;
    public float meleeRange;

    public Vector2 contactKnockback;
    public float contactDamage;
    public enum Phase { Phase1, Phase2, Phase3};
    public Phase currentPhase;
    public enum Difficulty { Easy, Medium, Hard};
    public Difficulty selectedDifficulty;

    //Action Decision Variables
    public Collider2D tpRange;
    bool initialWalkDone;
    public List<AttackObject> attackList;
    bool walking;
    bool canTurn;
    bool inHelmSplitter;
    float helmSplitterCooldown;
    int helmSplitterAltCounter;
    bool inUppercut;
    float uppercutCooldown;
    float uppercutTimer;
    float stingerCooldown;
    float stingerTimer;
    bool inStinger;
    public GameObject fadeObject;
    bool inEvade;
    public GameObject floorBeamObject;
    public GameObject beamWaveObject;
    public GameObject beamWaveMarker;
    public bool phase2introDone;
    float beamTimer;
    bool inSpecialAction;
    int backstepCounter;
    public ParticleSystem embers;
    public AudioSource bossMusicPlayer;
    public GameObject swordBeam;
    float swordBeamCooldown;
    bool phase3firstAttack;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        //gunTimer = gunCooldown;
        currentPhase = Phase.Phase1;
        canTurn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            CheckState();
            Actions();
        }

    }

    public override void Actions()
    {
        base.Actions();
        if (!movement.inKnockback && !movement.inHitStun && movement.controller.collisions.below && !movement.hitThisFrame && !melee.inAttack && !inEvade && !inSpecialAction)
        {
            float xDifference = player.transform.position.x - transform.position.x;
            float yDifference = player.transform.position.y - transform.position.y;
            if (movement.lastDirection != Mathf.Sign(xDifference) && canTurn)
            {
                movement.lastDirection = Mathf.Sign(xDifference);
                sprite.TurnSprite();
            }
            ReduceCooldowns();
            actionCooldown -= Time.deltaTime;
            if (actionCooldown <= 0)
            {
                DecideNextAction(xDifference, yDifference);
            }



            if (walking)
            {
                velocity.x = movement.moveSpeed * movement.lastDirection;
            }
            else
            {
                velocity.x = 0;
            }
            


            


        }
        if (inHelmSplitter)
        {
            velocity.x = 0;
            velocity.y = -25;
            if (movement.controller.collisions.below)
            {
                inHelmSplitter = false;
                melee.inAttack = false;
                melee.CancelAttacks();
                movement.inSpecialMovement = false;
                canTurn = true;
            }
        }
        if (inUppercut)
        {
            velocity.x = 0;
            velocity.y = 20;
            uppercutTimer -= Time.deltaTime;
            if (uppercutTimer <= 0)
            {
                velocity.y = 0;
                inUppercut = false;
                melee.CancelAttacks();
                movement.inSpecialMovement = false;
                canTurn = true;
            }
        }
        if (inStinger)
        {
            stingerTimer -= Time.deltaTime;
            if (stingerTimer <= 0.3f)
            {
                velocity.x = 15 * movement.lastDirection;
                velocity.y = 0;
            }
            else
            {
                velocity.x = 0;
            }
            
            
            if (stingerTimer <= 0)
            {
                sprite.currentAttackAnimName = "stingerB";
                melee.TriggerAttackWithCancel(attackList[4]);
                AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
                sprite.CheckState();
                inStinger = false;
                movement.inSpecialMovement = false;
                canTurn = true;
                rHealth.canKnockback = true;
            }
        }
        if (inEvade)
        {
            velocity.x = -10 * movement.lastDirection;
        }

        if (currentPhase == Phase.Phase3 && !inSpecialAction && selectedDifficulty != Difficulty.Easy)
        {
            beamTimer -= Time.deltaTime;
            if (beamTimer <= 0)
            {
                Instantiate(floorBeamObject, new Vector2(player.transform.position.x, -3.0f), new Quaternion());
                if (selectedDifficulty == Difficulty.Hard)
                {
                    beamTimer = 6f;
                }
                else
                {
                    beamTimer = 10.0f;
                }
                
            }
        }
        
    }

    void DecideDirection()
    {
        float xDifference = player.transform.position.x - transform.position.x;
        float yDifference = player.transform.position.y - transform.position.y;
        if (movement.lastDirection != Mathf.Sign(xDifference) && canTurn)
        {
            movement.lastDirection = Mathf.Sign(xDifference);
            sprite.TurnSprite();
        }
    }

    public void DecideNextAction(float xD, float yD)
    {
        walking = false;
        switch (currentPhase)
        {
            case Phase.Phase1: //-----------------------------------------------------PHASE ONE-----------------------------------------------------
                if (!initialWalkDone)
                {
                    actionCooldown = 1f;
                    initialWalkDone = true;
                    walking = true;
                }
                else if (CheckOnScreen() && Mathf.Abs(xD) <= 4) //Within melee range
                {
                    
                    if (helmSplitterCooldown <= 0 && helmSplitterAltCounter >= 2) //After three consecutive melee combos, perform a helm splitter
                    {
                        StartCoroutine(DoHelmSplitterWithTP());
                        helmSplitterCooldown = 5;
                        actionCooldown = 0.5f;
                        helmSplitterAltCounter = 0;
                    }
                    else //Perform a melee combo
                    {                            
                        StopCoroutine(phase1Combo());
                        StartCoroutine(phase1Combo());
                        actionCooldown = 0.3f;
                        helmSplitterAltCounter++;
                    }
                        
                    
                    
                }
                else if (helmSplitterCooldown <= 0 && CheckOnScreen()) //Perform a helm splitter
                {
                    StartCoroutine(DoHelmSplitterWithTP());
                    helmSplitterCooldown = 5;
                    actionCooldown = 0.5f;
                }
                else if (stingerCooldown <= 0) //Perform a stinger
                {
                    DoStinger();
                    stingerCooldown = 5;
                    actionCooldown = 0.5f;
                }
                else //Default state: walks towards the player
                {
                    rHealth.ResetKnockback();
                    walking = true;
                }

                
                break;
            case Phase.Phase2: //-----------------------------------------------------PHASE TWO-----------------------------------------------------
                if (!phase2introDone)
                {
                    phase2introDone = true;
                    StartCoroutine(DoBeamWave());
                }
                else if (CheckOnScreen() && Mathf.Abs(xD) <= 4) //Within melee range
                {
                    if (yD > 3 && uppercutCooldown <= 0)
                    {
                        DoUppercut();
                        actionCooldown = 1f;
                        uppercutCooldown = 5f;
                    }
                    else if (player.finder.melee.inAttack)
                    {
                        if (yD > 3)
                        {
                            DoUppercut();
                            actionCooldown = 0.5f;
                            uppercutCooldown = 1f;
                        }
                        else
                        {
                            if (backstepCounter >= 1)
                            {
                                backstepCounter = 0;
                                DoStingerCombo();
                                actionCooldown = 0.5f;
                            }
                            else
                            {
                                StartCoroutine(DoEvade(0.4f));
                                actionCooldown = 0.3f;
                                backstepCounter++;
                            }
                            
                        }
                                               
                        
                    }
                    else
                    {
                        if (Random.Range(0.0f, 1.0f) <= 0.2f)
                        {
                            StartCoroutine(DoEvade(0.4f));
                            actionCooldown = 0.3f;
                        }
                        else
                        {
                            if (helmSplitterCooldown <= 0 && helmSplitterAltCounter >= 2)
                            {
                                StartCoroutine(DoHelmSplitterWithTP());
                                helmSplitterCooldown = 5;
                                actionCooldown = 0.5f;
                                helmSplitterAltCounter = 0;
                            }
                            else
                            {
                                StopCoroutine(phase2Combo());
                                StartCoroutine(phase2Combo());
                                actionCooldown = 0.3f;
                                helmSplitterAltCounter++;
                            }
                        }
                        

                    }

                }
                else if (selectedDifficulty == Difficulty.Hard && swordBeamCooldown <= 0 && CheckOnScreen())
                {
                    StartCoroutine(DoSwordBeam());
                    swordBeamCooldown = 5f;
                    actionCooldown = 0.2f;
                }
                else if (helmSplitterCooldown <= 0 && CheckOnScreen())
                {
                    StartCoroutine(DoHelmSplitterWithTP());
                    helmSplitterCooldown = 5;
                    actionCooldown = 0.5f;
                }
                else if (stingerCooldown <= 0)
                {
                    StartCoroutine(DoStingerCombo());
                    stingerCooldown = 5;
                    actionCooldown = 0.5f;
                }
                else
                {
                    rHealth.ResetKnockback();
                    walking = true;
                }
                break;
            case Phase.Phase3: //-----------------------------------------------------PHASE THREE-----------------------------------------------------
                if (!phase3firstAttack)
                {
                    StartCoroutine(TripleSwordBeamAfterEvade());
                    swordBeamCooldown = 10f;
                    phase3firstAttack = true;
                    actionCooldown = 1f;
                }
                else if (CheckOnScreen() && Mathf.Abs(xD) <= 4) //Within melee range
                {
                    if (yD > 3 && uppercutCooldown <= 0) //If the player is above the boss, do an uppercut
                    {
                        DoUppercut();
                        actionCooldown = 1f;
                        if (selectedDifficulty == Difficulty.Hard)
                        {
                            uppercutCooldown = 1f;
                        }
                        else
                        {
                            uppercutCooldown = 5f;
                        }
                        
                    }
                    else if (player.finder.melee.inAttack) //If the player is performing an attack
                    {
                        if (player.finder.melee.inStinger)
                        {
                            StartCoroutine(DoEvadeWithStinger()); //Backdash into stinger
                            actionCooldown = 0.5f;
                        }
                        else if (player.finder.melee.inHelmSplitter)
                        {
                            DoUppercut();
                            actionCooldown = 0.5f;
                        }
                        else if (player.finder.melee.inUpperCut)
                        {
                            StartCoroutine(DoHelmSplitterWithTP());
                            actionCooldown = 0.5f;
                        }
                        else
                        {
                            if (movement.controller.collisions.left || movement.controller.collisions.right) //If in corner, escape using helm splitter
                            {
                                StartCoroutine(DoHelmSplitterWithTP());
                                actionCooldown = 0.5f;
                            }
                            else //Evade Backwards
                            {
                                StartCoroutine(DoEvade(0.4f));
                                actionCooldown = 0.3f;
                            }
                            
                        }

                    }
                    else
                    {
                        if (Random.Range(0.0f, 1.0f) <= 0.2f)
                        {
                            //StartCoroutine(DoEvade(0.4f));
                            //actionCooldown = 0.3f;
                            StartCoroutine(DoBeamWave());
                            actionCooldown = 1f;
                        }
                        else
                        {
                            if (helmSplitterCooldown <= 0 && helmSplitterAltCounter >= 2)
                            {
                                StartCoroutine(DoHelmSplitterWithTP());
                                helmSplitterCooldown = 5;
                                actionCooldown = 0.5f;
                                helmSplitterAltCounter = 0;
                            }
                            else
                            {
                                StopCoroutine(phase2Combo());
                                StartCoroutine(phase2Combo());
                                actionCooldown = 0.3f;
                                helmSplitterAltCounter++;
                            }
                        }


                    }

                }
                else if (swordBeamCooldown <= 0 && CheckOnScreen())
                {
                    if (selectedDifficulty == Difficulty.Hard)
                    {
                        StartCoroutine(TripleSwordBeam());
                        swordBeamCooldown = 5f;
                        actionCooldown = 0.2f;
                    }
                    else
                    {
                        StartCoroutine(DoSwordBeam());
                        swordBeamCooldown = 5f;
                        actionCooldown = 0.2f;
                    }
                }
                else if (helmSplitterCooldown <= 0 && CheckOnScreen())
                {
                    if (Random.Range(0.0f, 1.0f) <= 0.5f && selectedDifficulty == Difficulty.Hard)
                    {
                        StartCoroutine(TripleHelmSplitter());
                        helmSplitterCooldown = 5;
                    }
                    else
                    {
                        StartCoroutine(DoHelmSplitterWithTP());
                        helmSplitterCooldown = 5;
                        actionCooldown = 0.5f;
                    }
                    
                }
                else if (stingerCooldown <= 0)
                {
                    StartCoroutine(DoStingerCombo());
                    stingerCooldown = 5;
                    actionCooldown = 0.5f;
                }
                else
                {
                    rHealth.ResetKnockback();
                    walking = true;
                }
                break;
            default:
                break;
        }
    }

    void ReduceCooldowns()
    {
        helmSplitterCooldown -= Time.deltaTime;
        uppercutCooldown -= Time.deltaTime;
        stingerCooldown -= Time.deltaTime;
        swordBeamCooldown -= Time.deltaTime;
    }

    IEnumerator phase1Combo()
    {
        rHealth.ResetKnockback();        
        canTurn = false;
        sprite.currentAttackAnimName = "combohit2";
        melee.TriggerAttack(attackList[0]);
        sprite.CheckState();
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);        
        sprite.currentAttackAnimName = "combohit3";
        melee.TriggerAttackWithCancel(attackList[2]);
        sprite.CheckState();
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        canTurn = true;
        rHealth.canKnockback = true;
    }

    IEnumerator phase2Combo()
    {
        rHealth.ResetKnockback();
        canTurn = false;
        sprite.currentAttackAnimName = "combohit2";
        melee.TriggerAttack(attackList[0]);
        sprite.CheckState();
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);
        sprite.currentAttackAnimName = "combohit1";
        melee.TriggerAttackWithCancel(attackList[1]);
        sprite.CheckState();
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.35f);
        sprite.currentAttackAnimName = "combohit3";
        melee.TriggerAttackWithCancel(attackList[2]);
        sprite.CheckState();
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        canTurn = true;
        rHealth.canKnockback = true;
    }

    void Teleport(Vector2 newPos)
    {
        if (tpRange.OverlapPoint(newPos))
        {
            transform.position = newPos;            
        }
        else
        {
            newPos = tpRange.ClosestPoint(newPos);
            transform.position = newPos;            
        }
        
    }

    void CreateFadeObject()
    {
        GameObject fade = Instantiate(fadeObject, transform.position, transform.rotation);
        fade.transform.localScale = new Vector3(fade.transform.localScale.x * movement.lastDirection, fade.transform.localScale.y, 1);
    }

    void TeleportBehindPlayer(float yDiff)
    {
        Vector2 playerPos = player.transform.position;
        float playerDir = Mathf.Sign(player.transform.position.x - transform.position.x);
        Vector2 tpLocation = playerPos + new Vector2(playerDir * 2, yDiff);
        Teleport(tpLocation);
    }

    void DoHelmSplitter()
    {
        canTurn = false;
        melee.inAttack = true;
        sprite.ResetState();
        sprite.currentAttackAnimName = "helmSplitter";
        melee.TriggerAttackWithCancel(attackList[3]);
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        sprite.CheckState();
        inHelmSplitter = true;
        movement.inSpecialMovement = true;
        rHealth.canKnockback = true;
    }

    IEnumerator DoHelmSplitterWithTP()
    {
        CreateFadeObject();
        Vanish();        
        yield return new WaitForSeconds(0.4f);
        TeleportBehindPlayer(7);
        yield return new WaitForSeconds(0.05f);
        DecideDirection();
        DoHelmSplitter();
    }

    void DoUppercut()
    {
        //Debug.Log("do uppercut");
        DecideDirection();
        canTurn = false;
        sprite.currentAttackAnimName = "uppercut";
        melee.TriggerAttackWithCancel(attackList[3]);
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        sprite.CheckState();
        inUppercut = true;
        uppercutTimer = 0.15f;
        movement.inSpecialMovement = true;
        rHealth.canKnockback = true;
    }

    void DoStinger()
    {
        rHealth.ResetKnockback();
        DecideDirection();
        sprite.currentAttackAnimName = "stingerA";
        melee.inAttack = true;
        movement.inSpecialMovement = true;
        stingerTimer = 0.4f;
        inStinger = true;
        
    }

    void Vanish()
    {
        transform.position = new Vector3(-9999, -9999, 0);
    }

    public void KnockbackEscape()
    {
        StartCoroutine(DoHelmSplitterWithTP());
    }

    public void OnTakeKnockback()
    {
        StopAllCoroutines();
    }

    IEnumerator DoStingerCombo()
    {
        DoStinger();
        yield return new WaitForSeconds(0.45f);
        DoUppercut();
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(DoHelmSplitterWithTP());
        actionCooldown = 0.8f;
    }

    IEnumerator DoEvade(float t)
    {
        inEvade = true;
        movement.inSpecialMovement = true;

        yield return new WaitForSeconds(t);

        inEvade = false;
        movement.inSpecialMovement = false;
    }

    IEnumerator DoEvadeWithStinger()
    {
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        DoStinger();
        stingerCooldown = 5;
        actionCooldown = 0.5f;
    }

    IEnumerator TripleHelmSplitter()
    {
        StartCoroutine(DoHelmSplitterWithTP());
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(DoHelmSplitterWithTP());
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(DoHelmSplitterWithTP());        
        helmSplitterCooldown = 5;
        actionCooldown = 0.5f;
        helmSplitterAltCounter = 0;
    }

    IEnumerator DoBeamWave()
    {
        inSpecialAction = true;
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);

        sprite.currentAttackAnimName = "floorBeamWave";
        melee.inAttack = true;
        rHealth.ResetKnockback();
        Instantiate(beamWaveObject, beamWaveMarker.transform);
        yield return new WaitForSeconds(2f);
        melee.inAttack = false;
        inSpecialAction = false;
    }

    IEnumerator Phase3IntroBeams()
    {
        inSpecialAction = true;
        movement.inSpecialMovement = true;
        rHealth.ResetKnockback();
        CreateFadeObject();
        Vanish();
        FindObjectOfType<ComboUI>().PauseComboBar();
        yield return new WaitForSeconds(1f);
        Instantiate(beamWaveObject, beamWaveMarker.transform);
        yield return new WaitForSeconds(1.5f);
        Instantiate(beamWaveObject, new Vector3(beamWaveMarker.transform.position.x + 2.0f, beamWaveMarker.transform.position.y, beamWaveMarker.transform.position.z), new Quaternion());
        yield return new WaitForSeconds(1.5f);
        Instantiate(beamWaveObject, beamWaveMarker.transform);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 10; i++)
        {
            Instantiate(floorBeamObject, new Vector2(player.transform.position.x, -3.0f), new Quaternion());
            yield return new WaitForSeconds(0.25f);
        }
        inSpecialAction = false;
        sprite.ResetState();
        if (selectedDifficulty == Difficulty.Hard)
        {
            yield return new WaitForSeconds(1.0f);
            for (int i = 2; i < 10; i++)
            {
                Instantiate(floorBeamObject, new Vector2(player.transform.position.x + i, -3.0f), new Quaternion());
                Instantiate(floorBeamObject, new Vector2(player.transform.position.x - i, -3.0f), new Quaternion());
            }
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<ComboUI>().ResumeComboBar();
            StartCoroutine(DoHelmSplitterWithTP());

        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<ComboUI>().ResumeComboBar();
            StartCoroutine(DoHelmSplitterWithTP());
        }
        
        
        
    }

    IEnumerator DoSwordBeam()
    {
        sprite.currentAttackAnimName = "chargeBeam";
        melee.inAttack = true;
        rHealth.ResetKnockback();
        yield return new WaitForSeconds(0.5f);
        GameObject currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.2f);
        melee.inAttack = false;
    }

    IEnumerator TripleSwordBeam()
    {
        //inSpecialAction = true;
        //StopCoroutine(DoSwordBeam());
        //StartCoroutine(DoSwordBeam());
        //yield return new WaitForSeconds(0.6f);
        //StopCoroutine(DoSwordBeam());
        //StartCoroutine(DoSwordBeam());
        //yield return new WaitForSeconds(0.5f);
        //StopCoroutine(DoSwordBeam());
        //StartCoroutine(DoSwordBeam());
        //inSpecialAction = false;
        sprite.currentAttackAnimName = "tripleSwordBeam";
        melee.inAttack = true;
        rHealth.ResetKnockback();
        yield return new WaitForSeconds(0.5f);
        GameObject currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);
        currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);
        currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.2f);
        melee.inAttack = false;
    }

    IEnumerator TripleSwordBeamAfterEvade()
    {
        inSpecialAction = true;
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        StartCoroutine(DoEvade(0.3f));
        yield return new WaitForSeconds(0.33f);
        
        
        inSpecialAction = false;
        sprite.currentAttackAnimName = "tripleSwordBeam";
        melee.inAttack = true;
        rHealth.ResetKnockback();
        yield return new WaitForSeconds(0.5f);
        GameObject currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);
        currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.5f);
        currentBullet = Instantiate(swordBeam, transform.position, Quaternion.identity);
        currentBullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(player.transform.position.x - transform.position.x));
        AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/swing01", 2f);
        yield return new WaitForSeconds(0.2f);
        melee.inAttack = false;
    }

    public void SetPhase2()
    {
        currentPhase = Phase.Phase2;
    }

    public void SetPhase3()
    {
        currentPhase = Phase.Phase3;
        embers.Play();
        StartCoroutine(Phase3IntroBeams());
        bossMusicPlayer.pitch = 1.05f;
    }

    public void SetDifficulty(Difficulty d)
    {
        switch (d)
        {
            case Difficulty.Easy:
                selectedDifficulty = Difficulty.Easy;                
                movement.defaultMoveSpeed = 4f;
                break;
            case Difficulty.Medium:
                selectedDifficulty = Difficulty.Medium;
                movement.defaultMoveSpeed = 4f;
                break;
            case Difficulty.Hard:
                selectedDifficulty = Difficulty.Hard;
                movement.defaultMoveSpeed = 6f;
                rHealth.SetNewMaxHealth(400);
                rHealth.phase2boundary = 280;
                rHealth.phase3boundary = 160;
                break;
            default:
                break;
        }
    }

    public override void CheckState()
    {
        if (movement.inSpecialKnockback)
        {
            SetState(State.SpecialKnockback);
        }
        else if (inEvade)
        {
            SetState(State.Evade);
        }
        else if (movement.inKnockback)
        {
            SetState(State.Knockback);
        }
        else if (melee.inAttack)
        {
            SetState(State.Attack);
        }
        else if (!movement.isFlying && !movement.controller.collisions.below)
        {
            SetState(State.Jump);
        }
        else if (Mathf.Abs(movement.velocity.x) >= 0.01)
        {
            SetState(State.Moving);
        }
        else
        {
            SetState(State.Idle);
        }
    }
}
