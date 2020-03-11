using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    
    public bool countingTime;
    public float timeSpentInLevel;
    public bool usingCheckPoint;

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

    public void ReloadLevel()
    {
        usingCheckPoint = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReloadLevelWithCheckpoint()
    {
        usingCheckPoint = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
