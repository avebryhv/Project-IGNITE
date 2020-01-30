using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public float gameSpeed;
    public float playerSpeed;
    public float enemySpeed;
    public bool gamePaused;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
