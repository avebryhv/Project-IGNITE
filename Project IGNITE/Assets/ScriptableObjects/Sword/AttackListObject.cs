using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack List", menuName = "Attack List")]
public class AttackListObject : ScriptableObject
{
    public AttackObject light1;
    public AttackObject light2;
    public AttackObject light3;

    public AttackObject heavy;

    public AttackObject airLight1;
    public AttackObject airLight2;
    public AttackObject airLight3;

    public AttackObject airHeavy;

    
}
