using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Move List Item")]
public class UIMoveListItem : ScriptableObject
{
    public string name;
    public Sprite image;
    public string description = "Default Description";
}
