using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        currencyUI = FindObjectOfType<CurrencyUI>();
        currencyUI.UpdateCurrencyUI(currentAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddCurrency(int amount)
    {
        currentAmount += amount;
        currencyUI.UpdateCurrencyUI(currentAmount);
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

    }

    public void PurchaseDTUp()
    {

    }
}
