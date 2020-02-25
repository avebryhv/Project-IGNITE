using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesBehaviour : MonoBehaviour
{
    public PlayerScriptFinder finder;
    public GameObject drone1;
    public GameObject drone2;
    public enum State { Idle, Recharge, Blade, Wall, Barrier, Beam}
    public State currentState;
    public Animator anim;
    public PlayerStatsUI ui;

    public float cooldownTime;
    float cooldownCounter;

    public GameObject wallPrefab;


    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        ui = FindObjectOfType<PlayerStatsUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Recharge)
        {
            cooldownCounter += Time.deltaTime;
            ui.SetDronesValue(cooldownCounter, cooldownTime);
            if (cooldownCounter >= cooldownTime)
            {
                ChangeState(State.Idle);
            }
        }
    }

    public void InputState(State newState)
    {
        if (currentState == State.Idle)
        {
            ChangeState(newState);
        }
    }

    void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                SetAnimationTrigger("idle");
                currentState = State.Idle;
                break;
            case State.Recharge:
                SetAnimationTrigger("recharge");
                currentState = State.Recharge;
                break;
            case State.Blade:
                SetAnimationTrigger("blade");
                currentState = State.Blade;
                Invoke("StartRecharge", 10);
                break;
            case State.Wall:
                SetAnimationTrigger("wall");
                currentState = State.Wall;
                float dir = transform.localScale.x;
                Vector2 spawnPoint = new Vector2(transform.position.x + (dir * 4), transform.position.y);
                Instantiate(wallPrefab, spawnPoint, transform.rotation);
                Invoke("StartRecharge", 10);
                break;
            case State.Barrier:
                SetAnimationTrigger("barrier");
                currentState = State.Barrier;
                Invoke("StartRecharge", 10);
                break;
            case State.Beam:
                SetAnimationTrigger("beam");
                currentState = State.Beam;
                Invoke("StartRecharge", 10);
                break;
            default:
                break;
        }
    }

    void StartRecharge()
    {
        ChangeState(State.Recharge);
        cooldownCounter = 0;
    }

    public void BreakGuard()
    {
        StartRecharge();
    }

    public void SetAnimationTrigger(string name)
    {
        anim.Play(name, 0, 0);
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
