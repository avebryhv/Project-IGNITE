﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    
    public bool countingTime;
    public float timeSpentInLevel;
    public bool usingCheckPoint;
    public float totalComboScore;
    public float totalDamage;

    public static LevelManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            //instance = this;
        }
    }

    public void OnLevelStart()
    {

    }

    private void Update()
    {
        if (countingTime)
        {
            timeSpentInLevel += Time.deltaTime;
        }
    }

    public void PauseRecordingTime()
    {
        countingTime = false;
    }

    public void ResumeRecordingTime()
    {
        countingTime = true;
    }

    public void RestartLevel()
    {
        usingCheckPoint = false;
        ResetLevelStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReloadLevelWithCheckpoint()
    {
        usingCheckPoint = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetLevelStats()
    {
        timeSpentInLevel = 0;
        totalComboScore = 0;
        totalDamage = 0;
    }

    public void AddDamage(int amount)
    {
        totalDamage += amount;
    }

    public void AddComboScore(float amount)
    {
        totalComboScore += amount;
    }

    public void LoadLevelFromStart(string levelName)
    {
        ResetLevelStats();
        usingCheckPoint = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
    }

    public void EndLevel()
    {
        GameManager.Instance.SetGamePaused(true);
        Time.timeScale = 0;
        GameManager.Instance.finder.input.allowPlayerInput = false;
        FindObjectOfType<EndLevelUI>().ShowEndingScores();
    }
}
