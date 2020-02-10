using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptFinder : MonoBehaviour
{
    public PlayerInput input;
    public InputAssignment inputAssignment;
    public PlayerMovement movement;
    public Controller2D controller;
    public PlayerSprite sprite;
    public PlayerState state;
    public PlayerGuard guard;
    public GrappleGun grapple;
    public MeleeAttacker melee;
    public PlayerStats stats;
    public PlayerHealth health;

    void Awake()
    {
        FindScripts();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetFinder(this);
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
        grapple = FindObjectOfType<GrappleGun>();
        grapple.SetFinder(this);
        melee = FindObjectOfType<MeleeAttacker>();
        melee.SetFinder(this);
        stats = FindObjectOfType<PlayerStats>();
        stats.SetFinder(this);
        health = FindObjectOfType<PlayerHealth>();
        health.SetFinder(this);
        inputAssignment = FindObjectOfType<InputAssignment>();
    }
    
}
