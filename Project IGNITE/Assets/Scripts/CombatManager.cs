using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;

    public List<EnemyBaseBehaviour> activeEnemies;


    public static CombatManager Instance { get => instance; set => instance = value; }
    public PlayerScriptFinder finder;

    public float scanTime = 10;
    float scanTimer;


    private void Awake() //Creates Singleton
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
        finder = FindObjectOfType<PlayerScriptFinder>();
        activeEnemies = new List<EnemyBaseBehaviour>();
        scanTime = scanTimer;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            CheckListForEmpty();
            scanTimer = scanTime;
        }
    }

    public void AddActiveEnemy(EnemyBaseBehaviour toAdd)
    {
        if (!activeEnemies.Contains(toAdd))
        {
            activeEnemies.Add(toAdd);
        }
    }

    public void RemoveActiveEnemy(EnemyBaseBehaviour toRemove)
    {
        if (activeEnemies.Contains(toRemove))
        {
            activeEnemies.Remove(toRemove);
        }
    }

    void CheckListForEmpty()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies[i].Deactivate();
            }
        }
    }

    public int ActiveEnemyCount()
    {
        return activeEnemies.Count;
    }
}
