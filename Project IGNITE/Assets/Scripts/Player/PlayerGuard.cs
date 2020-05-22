using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    PlayerScriptFinder finder;

    public float timeHeld;
    public float parryTiming;
    public float guardDuration;
    public bool isGuarding;
    public bool inParry;
    public float inParryTime;
    float parryTimer;
    public bool bufferedBlock;

    public Color parryColour;
    public Color guardColour;

    public ParticleSystem guardParticles;

    // Start is called before the first frame update
    void Start()
    {
        timeHeld = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bufferedBlock && finder.state.DecideCanAct())
        {
            bufferedBlock = false;
            OnGuardPress();
        }
        if (isGuarding)
        {
            OnGuardHold();
        }
        else if (inParry)
        {
            InParry();
        }
        else
        {
            //finder.sprite.ChangeSpriteColour(Color.white);
        }
    }

    public void InputGuardHeld()
    {
        
    }

    public void OnGuardPress()
    {
        if (!finder.movement.inDash && !isGuarding && !finder.melee.inAttack && !finder.movement.inKnockback)
        {
            isGuarding = true;
            timeHeld = 0;
        }
        else
        {
            if (finder.melee.currentState == MeleeAttacker.phase.Active || finder.melee.currentState == MeleeAttacker.phase.Endlag)
            {
                bufferedBlock = true;
                finder.melee.CancelBuffer();
                finder.movement.CancelJumpBuffer();
            }
        }
        
    }

    public void OnGuardHold()
    {
        if (timeHeld <= parryTiming)
        {
            finder.sprite.ChangeSpriteColour(parryColour);
        }
        else
        {
            finder.sprite.ChangeSpriteColour(guardColour);
        }

        if (timeHeld >= guardDuration)
        {
            OnGuardRelease();
        }
        timeHeld += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
    }

    public void InParry()
    {
        if (parryTimer >= inParryTime)
        {
            ExitParry();
        }

        parryTimer += Time.deltaTime;
    }

    public void ExitParry()
    {
        inParry = false;
        isGuarding = false;
        FindObjectOfType<InputPrompt>().HidePrompt();
        finder.sprite.ChangeSpriteColour(Color.white);
    }

    public void OnGuardRelease()
    {
        timeHeld = 0;
        isGuarding = false;
        finder.sprite.ChangeSpriteColour(Color.white);
    }

    public void OnBlockAttack(int damage, Vector2 knockback, EnemyMeleeHitbox.type type, Vector2 position)
    {
        float xDifference = position.x - transform.position.x;
        //Debug.Log(Mathf.Sign(xDifference));
        if (Mathf.Sign(xDifference) != finder.movement.lastDirection)
        {
            finder.movement.ForceChangeDirection(Mathf.Sign(xDifference));
        }
        if (timeHeld <= parryTiming) //On Parry
        {
            //Debug.Log("SICK PARRY");
            finder.messages.CreateMinorMessage("PARRY", Color.blue, 1f);
            isGuarding = false;
            inParry = true;
            finder.health.StartIFrames(0.5f);
            parryTimer = 0;
            GameManager.Instance.DoHitLag();
            FindObjectOfType<InputPrompt>().ShowPrompt();
            finder.sprite.ChangeSpriteColour(Color.white);
            string path = "SFX/Player/parry";
            AudioClip aud = Resources.Load<AudioClip>(path);
            AudioManager.Instance.PlaySFX(aud, 0.3f);
        }
        else //Normally blocked
        {
            guardParticles.Play();
            AudioManager.Instance.PlaySFX("SFX/Player/block", 1f);
            GameManager.Instance.TriggerSmallRumble(0.1f);
        }
    }

    public void OnBlockAttack(int damage, Vector2 knockback, Vector2 position)
    {
        float xDifference = position.x - transform.position.x;
        //Debug.Log(Mathf.Sign(xDifference));
        if (Mathf.Sign(xDifference) != finder.movement.lastDirection)
        {
            finder.movement.ForceChangeDirection(Mathf.Sign(xDifference)); //Make the player face the direction of incoming damage
        }
        if (timeHeld <= parryTiming) //On Parry
        {            
            finder.messages.CreateMinorMessage("PARRY", Color.blue, 1f);
            isGuarding = false;
            inParry = true;
            parryTimer = 0;
            GameManager.Instance.DoHitLag();
            FindObjectOfType<InputPrompt>().ShowPrompt();
            finder.sprite.ChangeSpriteColour(Color.white);
            string path = "SFX/Player/parry";
            AudioClip aud = Resources.Load<AudioClip>(path);
            AudioManager.Instance.PlaySFX(aud, 0.3f);
        }
        else //Normally blocked
        {
            guardParticles.Play();
            AudioManager.Instance.PlaySFX("SFX/Player/block", 1f);
            GameManager.Instance.TriggerSmallRumble(0.1f);
            //if (finder.stats.dtCharge >= damage)
            //{
            //    finder.stats.IncreaseDT(-damage);
            //    Debug.Log("Normal Block");
            //}
            //else //Guard Broken
            //{
            //    finder.health.TakeDamage(damage, knockback, type); //Forces the player to take the damage
            //}

        }
    }

    public void CancelBuffer()
    {
        bufferedBlock = false;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
