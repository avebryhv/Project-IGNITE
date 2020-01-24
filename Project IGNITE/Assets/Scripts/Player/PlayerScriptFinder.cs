using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptFinder : MonoBehaviour
{
    public PlayerInput input;
    public PlayerMovement movement;
    public Controller2D controller;
    public PlayerSprite sprite;

    void Awake()
    {
        FindScripts();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindScripts()
    {
        input = FindObjectOfType<PlayerInput>();
        input.SetFinder(this);
        movement = FindObjectOfType<PlayerMovement>();

        sprite = FindObjectOfType<PlayerSprite>();
        sprite.SetFinder(this);
    }
}
