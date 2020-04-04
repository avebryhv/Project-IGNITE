using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalBehaviour : EnemyBaseBehaviour
{
    public float currentCooldown;
    public float actionCooldown;
    public Collider2D activeRange;
    
    public float meleeRange;

    public Vector2 contactKnockback;
    public float contactDamage;
    enum Phase { Phase1, Phase2, Phase3};
    Phase currentPhase;

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
        if (!movement.inKnockback && !movement.inHitStun && movement.controller.collisions.below && !movement.hitThisFrame && !melee.inAttack)
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
            velocity.x = 15 * movement.lastDirection;
            velocity.y = 0;
            stingerTimer -= Time.deltaTime;
            if (stingerTimer <= 0)
            {
                sprite.currentAttackAnimName = "stingerB";
                melee.TriggerAttackWithCancel(attackList[4]);
                sprite.CheckState();
                inStinger = false;
                movement.inSpecialMovement = false;
                canTurn = true;
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
            case Phase.Phase1:
                if (!initialWalkDone)
                {
                    actionCooldown = 1f;
                    initialWalkDone = true;
                    walking = true;
                }
                else if (CheckOnScreen() && Mathf.Abs(xD) <= 4) //Within melee range
                {
                    if (yD > 3 && uppercutCooldown <= 0)
                    {
                        DoUppercut();
                        actionCooldown = 1f;
                        uppercutCooldown = 5f;
                    }
                    else
                    {
                        if (helmSplitterCooldown <= 0 && helmSplitterAltCounter >= 3)
                        {
                            StartCoroutine(DoHelmSplitterWithTP());
                            helmSplitterCooldown = 5;
                            actionCooldown = 1f;
                            helmSplitterAltCounter = 0;
                        }
                        else
                        {                            
                            StopCoroutine(phase1Combo());
                            StartCoroutine(phase1Combo());
                            actionCooldown = 1f;
                            helmSplitterAltCounter++;
                        }
                        
                    }
                    
                }
                else if (helmSplitterCooldown <= 0 && CheckOnScreen())
                {
                    StartCoroutine(DoHelmSplitterWithTP());
                    helmSplitterCooldown = 5;
                    actionCooldown = 1f;
                }
                else if (stingerCooldown <= 0)
                {
                    DoStinger();
                    stingerCooldown = 5;
                    actionCooldown = 1f;
                }
                else
                {
                    walking = true;
                }

                
                break;
            case Phase.Phase2:
                break;
            case Phase.Phase3:
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
    }

    IEnumerator phase1Combo()
    {
        canTurn = false;
        sprite.currentAttackAnimName = "combohit2";
        melee.TriggerAttack(attackList[0]);
        sprite.CheckState();
        yield return new WaitForSecondsRealtime(0.5f);
        sprite.currentAttackAnimName = "combohit1";
        melee.TriggerAttackWithCancel(attackList[1]);
        sprite.CheckState();
        yield return new WaitForSecondsRealtime(0.35f);
        sprite.currentAttackAnimName = "combohit3";
        melee.TriggerAttackWithCancel(attackList[2]);
        sprite.CheckState();
        canTurn = true;
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
        sprite.currentAttackAnimName = "helmSplitter";
        melee.TriggerAttackWithCancel(attackList[3]);
        sprite.CheckState();
        inHelmSplitter = true;
        movement.inSpecialMovement = true;
    }

    IEnumerator DoHelmSplitterWithTP()
    {
        CreateFadeObject();
        Vanish();        
        yield return new WaitForSecondsRealtime(0.4f);
        TeleportBehindPlayer(7);
        yield return new WaitForSecondsRealtime(0.05f);
        DecideDirection();
        DoHelmSplitter();
    }

    void DoUppercut()
    {
        Debug.Log("do uppercut");
        DecideDirection();
        canTurn = false;
        sprite.currentAttackAnimName = "uppercut";
        melee.TriggerAttackWithCancel(attackList[3]);
        sprite.CheckState();
        inUppercut = true;
        uppercutTimer = 0.15f;
        movement.inSpecialMovement = true;
    }

    void DoStinger()
    {
        DecideDirection();
        sprite.currentAttackAnimName = "stingerA";
        melee.inAttack = true;
        movement.inSpecialMovement = true;
        stingerTimer = 0.3f;
        inStinger = true;
    }

    void Vanish()
    {
        transform.position = new Vector3(-9999, -9999, 0);
    }
}
