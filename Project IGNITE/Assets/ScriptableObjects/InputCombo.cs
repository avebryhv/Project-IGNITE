using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Combo", menuName = "Input Combo")]
public class InputCombo : ScriptableObject
{
    public PlayerInput.ControlStickState[] stickMovementList;
}
