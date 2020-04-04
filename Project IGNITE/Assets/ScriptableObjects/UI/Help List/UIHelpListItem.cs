using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Help", menuName = "Help List Item")]
public class UIHelpListItem : ScriptableObject
{
    public string name;
    public string description = "Default Description";
}
