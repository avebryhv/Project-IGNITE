using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesBehaviour : MonoBehaviour
{
    public PlayerScriptFinder finder;
    public GameObject drone1;
    public GameObject drone2;
    public GameObject beamSpawnPoint;
    public GameObject beamSpawnPoint2;
    public enum State { Idle, Recharge, Blade, Wall, Barrier, Beam}
    public State currentState;
    public Animator anim;
    public PlayerStatsUI ui;

    public float cooldownTime;
    float cooldownCounter;

    public GameObject wallPrefab;
    public GameObject beamPrefab;


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
            cooldownCounter += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
            ui.SetDronesValue(cooldownCounter, cooldownTime);
            if (cooldownCounter >= cooldownTime)
            {
                finder.messages.CreateMinorMessage("Drones Ready", Color.green, 1f);
                string path = "SFX/Player/droneReady";
                AudioClip aud = Resources.Load<AudioClip>(path);
                AudioManager.Instance.PlaySFX(aud, 0.3f);
                ChangeState(State.Idle);
            }
        }
    }

    public void ReduceCooldown(float amount)
    {
        if (currentState == State.Recharge)
        {
            cooldownCounter += amount;
        }
    }

    public void InputState(State newState)
    {
        if (currentState == State.Idle)
        {
            ChangeState(newState);
        }
        else if (currentState == State.Wall && newState == State.Wall)
        {
            StartRecharge();
            Destroy(FindObjectOfType<DroneWall>().gameObject);
        }
        else if (currentState == State.Blade && newState == State.Blade)
        {
            StartRecharge();
        }
        else if (currentState == State.Barrier && newState == State.Barrier)
        {
            StartRecharge();
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
                cooldownTime = 30;
                Invoke("StartRecharge", 10);
                break;
            case State.Wall:
                SetAnimationTrigger("wall");
                currentState = State.Wall;
                cooldownTime = 30;
                float dir = transform.localScale.x;
                Vector2 spawnPoint = new Vector2(transform.position.x + (dir * 4), transform.position.y);
                Instantiate(wallPrefab, spawnPoint, transform.rotation);
                Invoke("StartRecharge", 10);
                break;
            case State.Barrier:
                SetAnimationTrigger("barrier");
                currentState = State.Barrier;
                cooldownTime = 30;
                Invoke("StartRecharge", 10);
                break;
            case State.Beam:
                SetAnimationTrigger("beam");
                currentState = State.Beam;
                cooldownTime = 10;
                float dir2 = transform.localScale.x;
                GameObject beamObj = Instantiate(beamPrefab, beamSpawnPoint.transform.position, transform.rotation);
                beamObj.transform.localScale = new Vector3(dir2, beamObj.transform.localScale.y, beamObj.transform.localScale.z);
                GameObject beamObj2 = Instantiate(beamPrefab, beamSpawnPoint2.transform.position, transform.rotation);
                beamObj2.transform.localScale = new Vector3(dir2, beamObj2.transform.localScale.y, beamObj2.transform.localScale.z);
                Invoke("StartRecharge", 0.25f);
                break;
            default:
                break;
        }
    }

    void StartRecharge()
    {
        CancelInvoke();
        ChangeState(State.Recharge);
        cooldownCounter = 0;
    }

    public void BreakGuard()
    {
        CancelInvoke();
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
