using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlocksUI : MonoBehaviour
{
    PlayerUnlocks unlocks;
    CurrencyUI currency;

    public TextMeshProUGUI currencyText;
    public Selectable firstSelectable;


    //Cost Texts
    [Header("Cost Texts")]
    public TextMeshProUGUI stingerCost;
    public TextMeshProUGUI groundBCost;
    public TextMeshProUGUI airBCost;
    public TextMeshProUGUI doubleJumpCost;
    public TextMeshProUGUI airDashCost;
    public TextMeshProUGUI directionDashCost;

    // Start is called before the first frame update
    void Start()
    {
        unlocks = FindObjectOfType<PlayerUnlocks>();
        currency = FindObjectOfType<CurrencyUI>();
        UpdateCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenu()
    {
        UpdateCurrency();
        UpdateCosts();
        firstSelectable.Select();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void UnlockStinger()
    {
        if (!unlocks.stinger && unlocks.currentAmount >= 500)
        {
            unlocks.stinger = true;
            unlocks.SubtractCurrency(500);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UnlockGroundB()
    {
        if (!unlocks.lightComboB && unlocks.currentAmount >= 1000)
        {
            unlocks.lightComboB = true;
            unlocks.SubtractCurrency(1000);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UnlockAirB()
    {
        if (!unlocks.airComboB && unlocks.currentAmount >= 2000)
        {
            unlocks.airComboB = true;
            unlocks.SubtractCurrency(2000);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UnlockDoubleJump()
    {
        if (!unlocks.doubleJump && unlocks.currentAmount >= 5000)
        {
            unlocks.doubleJump = true;
            unlocks.SubtractCurrency(5000);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UnlockAirDash()
    {
        if (!unlocks.airDash && unlocks.currentAmount >= 2500)
        {
            unlocks.airDash = true;
            unlocks.SubtractCurrency(2500);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UnlockDirectionDash()
    {
        if (!unlocks.directionDash && unlocks.currentAmount >= 10000)
        {
            unlocks.directionDash = true;
            unlocks.SubtractCurrency(10000);
            UpdateCurrency();
            UpdateCosts();
        }
    }

    public void UpdateCurrency()
    {
        currencyText.text = unlocks.currentAmount.ToString();
    }

    void UpdateCosts()
    {
        if (unlocks.stinger)
        {
            stingerCost.text = "---";
        }

        if (unlocks.lightComboB)
        {
            groundBCost.text = "---";
        }

        if (unlocks.airComboB)
        {
            airBCost.text = "---";
        }

        if (unlocks.doubleJump)
        {
            doubleJumpCost.text = "---";
        }

        if (unlocks.airDash)
        {
            airDashCost.text = "---";
        }

        if (unlocks.directionDash)
        {
            directionDashCost.text = "---";
        }

    }
}
