﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerScriptFinder finder;
    public bool allowPlayerInput;
    public enum ControlStickState { Neutral, Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight};
    ControlStickState leftStickState;
    ControlStickState lastState;
    public List<ControlStickState> leftStickInputList;
    public InputCombo testInput;



    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {        
        if (allowPlayerInput)
        {
            if (Gamepad.all.Count > 0)
            {
                ReadControllerInputs();
            }
            else
            {
                ReadKeyboardInputs();
            }        
            

        }
    }

    void ReadControllerInputs()
    {
        ReadControlStick();
        ReadRightStick();

        //Jump Inputs
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            finder.movement.OnJumpInputDown();
        }

        if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            finder.movement.OnJumpInputUp();
        }

        //Lock On Inputs
        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            finder.movement.OnLockOnDown();
        }

        if (Gamepad.current.rightShoulder.wasReleasedThisFrame)
        {
            finder.movement.OnLockOnUp();
        }

        //Block / Evade Inputs
        if (Gamepad.current.buttonEast.isPressed)
        {
            if (finder.movement.lockedOn && Gamepad.current.leftStick.ReadValue().x != 0)
            {
                finder.movement.OnDashInput();
            }
            else
            {
                //Guard
                finder.guard.OnGuardPress();
            }
        }

        //Light Attack Inputs
        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            finder.melee.LightAttackPressed();
        }

        //Heavy Attack Inputs
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            finder.melee.HeavyAttackPressed();
            
        }

        if (Gamepad.current.buttonNorth.wasReleasedThisFrame)
        {
            finder.melee.HeavyAttackReleased();
        }

        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            finder.stats.Burst();
        }

        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            finder.grapple.GrappleButtonPressed();
        }
    }

    void ReadKeyboardInputs()
    {
        float xInput;
        if (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed)
        {
            xInput = 1;
        }
        else if (!Keyboard.current.rightArrowKey.isPressed && Keyboard.current.leftArrowKey.isPressed)
        {
            xInput = -1;
        }
        else
        {
            xInput = 0;
        }

        float yInput;
        if (Keyboard.current.upArrowKey.isPressed && !Keyboard.current.downArrowKey.isPressed)
        {
            yInput = 1;
        }
        else if (!Keyboard.current.upArrowKey.isPressed && Keyboard.current.downArrowKey.isPressed)
        {
            yInput = -1;
        }
        else
        {
            yInput = 0;
        }
        Vector2 input = new Vector2(xInput, yInput);
        finder.movement.SetDirectionalInput(input);

        //Jump Inputs
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            finder.movement.OnJumpInputDown();
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            finder.movement.OnJumpInputUp();
        }

        //Lock On Inputs
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            finder.movement.OnLockOnDown();
        }

        if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
        {
            finder.movement.OnLockOnUp();
        }

        //Block / Evade Inputs
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (finder.movement.lockedOn && xInput != 0)
            {
                finder.movement.OnDashInput();
            }
            else
            {
                //Guard
                finder.guard.OnGuardPress();
            }
        }

        //Light Attack Inputs
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            finder.melee.LightAttackPressed();
        }

        //Heavy Attack Inputs
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            finder.melee.HeavyAttackPressed();
        }

        if (Keyboard.current.xKey.wasReleasedThisFrame)
        {
            finder.melee.HeavyAttackReleased();
        }
    }

    void ReadControlStick()
    {
        Vector2 input = Gamepad.current.leftStick.ReadValue();
        finder.movement.SetDirectionalInput(input);

        if (input.x > 0.2) //Right Side of the control stick
        {
            if (input.y > 0.2)
            {
                leftStickState = ControlStickState.UpRight;
            }
            else if (input.y < -0.2)
            {
                leftStickState = ControlStickState.DownRight;
            }
            else
            {
                leftStickState = ControlStickState.Right;
            }
        }
        else if (input.x < -0.2) //Left side of the control stick
        {
            if (input.y > 0.2)
            {
                leftStickState = ControlStickState.UpLeft;
            }
            else if (input.y < -0.2)
            {
                leftStickState = ControlStickState.DownLeft;
            }
            else
            {
                leftStickState = ControlStickState.Left;
            }
        }
        else //Stick is not horizontally pressed
        {
            if (input.y > 0.2)
            {
                leftStickState = ControlStickState.Up;
            }
            else if (input.y < -0.2)
            {
                leftStickState = ControlStickState.Down;
            }
            else
            {
                leftStickState = ControlStickState.Neutral;
            }
        }

        UpdateState(leftStickState);
    }

    void ReadRightStick()
    {
        Vector2 input = Gamepad.current.rightStick.ReadValue();
        finder.grapple.CStickInput(input);
    }

    void UpdateState(ControlStickState newState)
    {
        if (newState != lastState)
        {
            lastState = newState;
            leftStickInputList.Add(lastState);
            if (leftStickInputList.Count > 5)
            {
                leftStickInputList.RemoveAt(0);
            }
            
        }
    }

    public bool CheckStickInputs(InputCombo toTest)
    {
        bool tempBool = true;
        int listStartPoint = (5 - toTest.stickMovementList.Length);

        for (int i = 0; i < toTest.stickMovementList.Length; i++)
        {
            if (toTest.stickMovementList[i] != leftStickInputList[i + listStartPoint])
            {
                tempBool = false;
            }
        }

        return tempBool;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }

    
}
