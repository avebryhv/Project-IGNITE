﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public bool inIntro;
    public Animator introAnim;

    // Start is called before the first frame update
    void Start()
    {
        inIntro = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame && !inIntro)
        {
            SkipIntro();
        }
    }

    public void SkipIntro()
    {
        introAnim.Play("spaceShot", 0, 0);
        inIntro = true;
    }
}


