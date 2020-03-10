﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SaveStatue : MonoBehaviour
{
    public bool playerInFront;
    public StatueMenu menu;
    public Image interactImage;
    // Start is called before the first frame update
    void Start()
    {
        menu = FindObjectOfType<StatueMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInFront)
        {
            interactImage.enabled = true;
            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                OpenSaveStatue();
            }
            
        }
        else
        {
            interactImage.enabled = false; ;

        }


    }

    private void LateUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            playerInFront = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            playerInFront = false;
        }
    }

    public void OpenSaveStatue()
    {
        GameManager.Instance.SetGamePaused(true);
        Time.timeScale = 0;
        GameManager.Instance.finder.input.allowPlayerInput = false;
        menu.OpenMenu();
    }
}
