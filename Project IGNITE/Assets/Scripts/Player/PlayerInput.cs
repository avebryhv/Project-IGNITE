using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour
{
    PlayerScriptFinder finder;
    public InputStream inputStream;
    public bool allowPlayerInput;
    public enum ControlStickState { Neutral, Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight};
    ControlStickState leftStickState;
    ControlStickState lastState;
    public List<ControlStickState> leftStickInputList;
    public InputComboList inputComboList;
    public float timeSinceLastInput;
    public float inputDecayTime;
    float decayCounter;
    

    public float deadZone;
    public bool usingDeadZone;

    public ButtonControl lightAttackButton;

    
    // Start is called before the first frame update
    void Start()
    {
        lightAttackButton = Gamepad.current.buttonWest;
    }

    // Update is called once per frame
    void Update()
    {        
        if (allowPlayerInput)
        {
            if (!GameManager.Instance.gamePaused)
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
        StickInputDecay();
        ReadPauseInputs();
    } 

    void ReadControllerInputs()
    {
        ReadControlStick();
        //ReadRightStick();

        //Jump Inputs
        if (finder.inputAssignment.jumpButton.wasPressedThisFrame)
        {
            finder.controller.jumpPressed = true;
            finder.movement.OnJumpInputDown();
            
        }

        if (finder.inputAssignment.jumpButton.wasReleasedThisFrame)
        {
            finder.movement.OnJumpInputUp();
        }

        //Lock On Inputs
        if (finder.inputAssignment.lockOnButton.wasPressedThisFrame)
        {
            finder.movement.OnLockOnDown();
        }

        if (finder.inputAssignment.lockOnButton.wasReleasedThisFrame)
        {
            finder.movement.OnLockOnUp();
        }

        //Block / Evade Inputs
        if (finder.inputAssignment.blockButton.wasPressedThisFrame)
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
        if (finder.inputAssignment.lightAttackButton.wasPressedThisFrame)
        {
            finder.melee.LightAttackPressed();
        }


        //Heavy Attack Inputs
        if (finder.inputAssignment.heavyAttackButton.wasPressedThisFrame)
        {
            finder.melee.HeavyAttackPressed();
            
        }

        if (finder.inputAssignment.heavyAttackButton.isPressed)
        {
            finder.melee.HeavyAttackHeld();
        }

        if (finder.inputAssignment.heavyAttackButton.wasReleasedThisFrame)
        {
            finder.melee.HeavyAttackReleased();
        }

        //if (finder.inputAssignment.burstButton.wasPressedThisFrame)
        //{
        //    finder.stats.Burst();
        //}

        if (finder.inputAssignment.grappleButton.wasPressedThisFrame)
        {
            //finder.grapple.GrappleButtonPressed();
            finder.movement.OnDashInput();
        }

        //if (finder.inputAssignment.toggleDTButton.wasPressedThisFrame)
        //{
        //    finder.stats.DTButtonPressed();
        //}

        //D-Pad Inputs
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            finder.drones.InputState(DronesBehaviour.State.Blade);
        }

        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            finder.drones.InputState(DronesBehaviour.State.Beam);
        }

        if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            finder.drones.InputState(DronesBehaviour.State.Barrier);
        }

        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            finder.drones.InputState(DronesBehaviour.State.Wall);
        }


    }

    void ReadPauseInputs()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            FindObjectOfType<PauseMenu>().PauseButtonPressed();
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
        if (usingDeadZone)
        {
            if (Mathf.Abs(input.x) <= deadZone)
            {
                input.x = 0;
            }
            if (Mathf.Abs(input.y) <= deadZone)
            {
                input.y = 0;
            }
        }
        finder.movement.SetDirectionalInput(input);

        if (input.x > 0.4) //Right Side of the control stick
        {
            if (input.y > 0.4)
            {
                leftStickState = ControlStickState.UpRight;
            }
            else if (input.y < -0.4)
            {
                leftStickState = ControlStickState.DownRight;
            }
            else
            {
                leftStickState = ControlStickState.Right;
            }
        }
        else if (input.x < -0.4) //Left side of the control stick
        {
            if (input.y > 0.4)
            {
                leftStickState = ControlStickState.UpLeft;
            }
            else if (input.y < -0.4)
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
            if (input.y > 0.4)
            {
                leftStickState = ControlStickState.Up;
            }
            else if (input.y < -0.4)
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
            timeSinceLastInput = 0;
            if (leftStickInputList.Count > 10)
            {
                leftStickInputList.RemoveAt(0);
            }
            inputStream.RecieveInput(lastState);
            
        }

        timeSinceLastInput += Time.deltaTime;

        
    }

    public bool CheckStickInputs(InputCombo toTest)
    {
        if (leftStickInputList.Count >= toTest.stickMovementList.Length)
        {
            bool tempBool = true;
            int listStartPoint = (leftStickInputList.Count - toTest.stickMovementList.Length);

            for (int i = 0; i < toTest.stickMovementList.Length; i++)
            {
                if (toTest.stickMovementList[i] != leftStickInputList[i + listStartPoint])
                {
                    tempBool = false;
                }
            }

            if (tempBool)
            {
                leftStickInputList.Clear();
            }

            return tempBool;
        }
        else
        {
            return false;
        }
        
    }

    void StickInputDecay()
    {
        if (leftStickInputList.Count > 0)
        {
            decayCounter += Time.deltaTime;
            if (decayCounter >= inputDecayTime)
            {
                leftStickInputList.RemoveAt(0);
                decayCounter = 0;
            }
        }
        else
        {
            decayCounter = 0;
        }
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }

    
}
