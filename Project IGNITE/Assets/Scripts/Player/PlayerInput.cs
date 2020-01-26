using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerScriptFinder finder;
    public bool allowPlayerInput;
    




    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {        
        if (allowPlayerInput)
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
            if (Gamepad.current.buttonWest.isPressed)
            {
                finder.melee.LightAttackPressed();
            }
            





        }
    }

    void ReadControlStick()
    {
        Vector2 input = Gamepad.current.leftStick.ReadValue();
        finder.movement.SetDirectionalInput(input);
    }

    void ReadRightStick()
    {
        Vector2 input = Gamepad.current.rightStick.ReadValue();
        finder.grapple.CStickInput(input);
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
