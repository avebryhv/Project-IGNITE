﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public float gameSpeed;
    public float playerSpeed;
    public float enemySpeed;
    public bool gamePaused;
    public bool usingController;
    int gamepadCount;
    public PlayerScriptFinder finder;    

    public static GameManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckGamePads();
    }    

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count != gamepadCount)
        {
            CheckGamePads();
        }
    }

    public float ReturnPlayerSpeed()
    {
        return playerSpeed * gameSpeed;
    }

    public float ReturnEnemySpeed()
    {
        return enemySpeed * gameSpeed;
    }

    public void SetGameSpeed(float newValue)
    {
        gameSpeed = newValue;
    }

    public void SetGamePaused(bool newValue)
    {
        gamePaused = newValue;        
    }

    public void DoHitLag()
    {
        Time.timeScale = 0.1f;
        CancelInvoke();
        Invoke("EndHitLag", 0.01f);
    }

    void EndHitLag()
    {
        Time.timeScale = 1f;
    }

    void CheckGamePads()
    {
        if (Gamepad.all.Count > 0)
        {
            usingController = true;
            FindObjectOfType<InputAssignment>().ReAssignButtons();
        }
        else
        {
            usingController = false;
        }
        gamepadCount = Gamepad.all.Count;
    }   

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
