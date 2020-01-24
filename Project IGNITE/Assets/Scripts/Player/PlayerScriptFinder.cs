using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptFinder : MonoBehaviour
{
    public PlayerInput input;
    public PlayerMovement movement;
    public Controller2D controller;
    public PlayerSprite sprite;
    public PlayerState state;
    public PlayerGuard guard;

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
        movement.SetFinder(this);
        sprite = FindObjectOfType<PlayerSprite>();
        sprite.SetFinder(this);
        state = FindObjectOfType<PlayerState>();
        state.SetFinder(this);
        controller = GetComponent<Controller2D>();
        guard = FindObjectOfType<PlayerGuard>();
        guard.SetFinder(this);
    }
    
}
