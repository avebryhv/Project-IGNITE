using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack List", menuName = "Attack List")]
public class AttackListObject : ScriptableObject
{
    [Header("Light Combo A")]
    public AttackObject light1;
    public AttackObject light2;
    public AttackObject light3;

    [Header("Light Combo B")]
    public AttackObject lightB1;
    public AttackObject lightB2;
    public AttackObject lightB3;

    public AttackObject heavy;
    public AttackObject heavyCharged;

    public AttackObject airLight1;
    public AttackObject airLight2;
    public AttackObject airLight3;

    public AttackObject airPause;

    public AttackObject airHeavy;

    public AttackObject stingerRush;
    public AttackObject stinger;
    public AttackObject uppercut;
    public AttackObject helmsplitter;
    public AttackObject helmSplitterGround;

    public AttackObject parryPunch;
    public AttackObject halfCircleDown;

    
}
