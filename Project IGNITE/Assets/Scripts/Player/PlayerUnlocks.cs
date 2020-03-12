using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class PlayerUnlocks : MonoBehaviour
{
    [Header("Move Unlocks")]
    public bool stinger;
    public bool uppercut;
    public bool lightComboB;
    public bool airComboB;
    public bool burst;

    [Header("Movement Unlocks")]
    public bool doubleJump;
    public bool airDash;
    public bool directionDash;

    [Header("Stat Increases")]
    public int healthUps;
    public int DTUps;


    [Header("Currency Variables")]
    public int currentAmount;
    CurrencyUI currencyUI;

    [Header("CheckPoint")]
    public Vector2 checkpointPosition;

    public PlayerScriptFinder finder;
    public string filePath;

    // Start is called before the first frame update
    void Start()
    {
        currencyUI = FindObjectOfType<CurrencyUI>();
        currencyUI.UpdateCurrencyUI(currentAmount);
        LoadUnlocks();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveUnlocks();
        //}
    }


    public void AddCurrency(int amount)
    {
        currentAmount += amount;
        currencyUI.UpdateCurrencyUI(currentAmount);
        currencyUI.ShowNewCurrency(amount);
    }

    public void SubtractCurrency(int amount)
    {
        currentAmount -= amount;
        currencyUI.UpdateCurrencyUI(currentAmount);
    }

    public void SetCurrency(int amount)
    {
        currentAmount = amount;
        currencyUI.UpdateCurrencyUI(currentAmount);
    }

    public void PurchaseHealthUp()
    {
        healthUps++;
        finder.health.IncreaseMaxHealth(10);
    }

    public void PurchaseDTUp()
    {
        DTUps++;
        finder.stats.IncreaseMaxDT(10);
    }

    private Save CreateSaveObject()
    {
        Save save = new Save();
        save.stinger = stinger;
        save.lightComboB = lightComboB;
        save.airComboB = airComboB;
        save.doubleJump = doubleJump;
        save.uppercut = uppercut;
        save.airDash = airDash;
        save.directionDash = directionDash;
        save.burst = burst;

        save.healthUps = healthUps;
        save.DTUps = DTUps;
        save.currentAmount = currentAmount;
        save.checkPointPositionX = checkpointPosition.x;
        save.checkPointPositionY = checkpointPosition.y;

        return save;
    }

    public void SaveUnlocks()
    {
        Save save = CreateSaveObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath);
        formatter.Serialize(file, save);
        file.Close();

        Debug.Log("Unlocks Saved");
        //finder.messages.CreateMajorMessage("Checkpoint Reached", Color.blue, 2.0f);
        
    }

    public void SaveUnlocksWithCheckpoint()
    {
        StorePosition();
        Save save = CreateSaveObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath);
        formatter.Serialize(file, save);
        file.Close();
        
        Debug.Log("Unlocks Saved With Checkpoint");
        finder.messages.CreateMajorMessage("Checkpoint Reached", Color.blue, 2.0f);
    }

    public void LoadUnlocks()
    {
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            Save save = (Save)formatter.Deserialize(file);
            file.Close();

            stinger = save.stinger;
            lightComboB = save.lightComboB;
            airComboB = save.airComboB;
            uppercut = save.uppercut;
            doubleJump = save.doubleJump;
            airDash = save.airDash;
            directionDash = save.directionDash;
            burst = save.burst;

            healthUps = save.healthUps;
            DTUps = save.DTUps;
            currentAmount = save.currentAmount;
            checkpointPosition.x = save.checkPointPositionX;
            checkpointPosition.y = save.checkPointPositionY;
            currencyUI.UpdateCurrencyUI(currentAmount);
            finder.health.LoadMaxHealth(100 + healthUps * 10);
            finder.stats.LoadMaxDT(50 + healthUps * 10);

            if (LevelManager.Instance.usingCheckPoint)
            {
                SetPosition();
            }
            
        }
    }

    public void StorePosition()
    {
        checkpointPosition = transform.parent.transform.position;
        Debug.Log(checkpointPosition);

    }

    void SetPosition()
    {
        transform.parent.transform.position = checkpointPosition;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
