using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
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

    public int foundHealthUps;
    public bool foundHealth1;
    public bool foundHealth2;
    public int foundDTUps;


    [Header("Currency Variables")]
    public int currentAmount;
    public float checkPointPositionX;
    public float checkPointPositionY;
}
