using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Melee Attack")]
public class AttackObject : ScriptableObject
{
    //Holds Data for a Single Attack
    public GameObject hitboxObject; //Prefab containing the hitbox to be spawned
    public float startUpTime; //Time before the hitbox appears
    public float hitboxLingerTime; //Duration of the hitbox - starts after startUpTime
    public float endingTime; //Time before the player can act - starts after the hitbox ends
    public bool endsCombo; //If this hit is the last hit in the combo string - resets counter
    public string animationName; //Name of the animation to be played with this attack
}
