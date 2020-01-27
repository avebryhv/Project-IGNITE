using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int dtCharge;
    public int dtMax;

    PlayerStatsUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<PlayerStatsUI>();
        health = maxHealth;
        dtCharge = dtMax;
        ui.SetHealthValue(health, maxHealth);
        ui.SetDTValue(dtCharge, dtMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(int amount)
    {

    }

    public void ChangeDT(int amount)
    {

    }
}
