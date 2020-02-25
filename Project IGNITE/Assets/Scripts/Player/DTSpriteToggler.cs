using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTSpriteToggler : MonoBehaviour
{
    SpriteRenderer spr;
    public Sprite normalSprite;
    public Sprite DTSprite;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDTSprite()
    {
        spr.sprite = DTSprite;
    }

    public void SetNormalSprite()
    {
        spr.sprite = normalSprite;
    }
}
