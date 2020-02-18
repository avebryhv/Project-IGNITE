using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Move", menuName = "Move List Item")]
public class UIMoveListItem : ScriptableObject
{
    public string name;
    public Sprite image;
    public string description = "Default Description";
    public VideoClip video;
}
