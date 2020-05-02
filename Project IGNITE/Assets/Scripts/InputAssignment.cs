using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class InputAssignment : MonoBehaviour
{
    public PlayerInput playerInput;
    public enum ButtonAction { Jump, Block, LightAttack, HeavyAttack, LockOn, Grapple, Burst, DT}
    public enum AssignableButton { NorthFace, EastFace, SouthFace, WestFace, R1, R2, L1, L2, L3, R3}

    //[Header("Face Buttons")]
    //public ButtonAction northButton;
    //public ButtonAction eastButton;
    //public ButtonAction southButton;
    //public ButtonAction westButton;

    //[Header("Shoulder Buttons")]
    //public ButtonAction leftBumper;
    //public ButtonAction leftTrigger;
    //public ButtonAction rightBumper;
    //public ButtonAction rightTrigger;

    
    public AssignableButton jumpButtonSelection;
    public AssignableButton blockButtonSelection;
    public AssignableButton lightAttackButtonSelection;
    public AssignableButton heavyAttackButtonSelection;

    public AssignableButton toggleDTButtonSelection;
    public AssignableButton burstButtonSelection;
    public AssignableButton lockOnButtonSelection;
    public AssignableButton grappleButtonSelection;

    public ButtonControl heavyAttackButton;
    public ButtonControl blockButton;
    public ButtonControl jumpButton;
    public ButtonControl lightAttackButton;

    public ButtonControl toggleDTButton;
    public ButtonControl burstButton;
    public ButtonControl lockOnButton;
    public ButtonControl grappleButton;

    private void Awake()
    {
        LoadInputs();
        //if (GameManager.Instance.usingController)
        //{
            
        //    AssignJump();
        //    AssignBlock();
        //    AssignLightAttack();
        //    AssignHeavyAttack();
        //    AssignDTToggle();
        //    AssignBurst();
        //    AssignLockOn();
        //    AssignGrapple();
        //}
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.usingController)
        {

            AssignJump();
            AssignBlock();
            AssignLightAttack();
            AssignHeavyAttack();
            AssignDTToggle();
            AssignBurst();
            AssignLockOn();
            AssignGrapple();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReAssignButtons()
    {
        SaveInputs();
        AssignJump();
        AssignBlock();
        AssignLightAttack();
        AssignHeavyAttack();
        AssignDTToggle();
        AssignBurst();
        AssignLockOn();
        AssignGrapple();
    }

    void AssignJump()
    {
        switch (jumpButtonSelection)
        {
            case AssignableButton.NorthFace:
                jumpButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                jumpButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                jumpButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                jumpButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                jumpButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                jumpButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                jumpButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                jumpButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                jumpButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                jumpButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignBlock()
    {
        switch (blockButtonSelection)
        {
            case AssignableButton.NorthFace:
                blockButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                blockButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                blockButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                blockButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                blockButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                blockButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                blockButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                blockButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                blockButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                blockButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignLightAttack()
    {
        switch (lightAttackButtonSelection)
        {
            case AssignableButton.NorthFace:
                lightAttackButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                lightAttackButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                lightAttackButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                lightAttackButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                lightAttackButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                lightAttackButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                lightAttackButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                lightAttackButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                lightAttackButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                lightAttackButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignHeavyAttack()
    {
        switch (heavyAttackButtonSelection)
        {
            case AssignableButton.NorthFace:
                heavyAttackButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                heavyAttackButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                heavyAttackButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                heavyAttackButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                heavyAttackButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                heavyAttackButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                heavyAttackButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                heavyAttackButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                heavyAttackButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                heavyAttackButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignDTToggle()
    {
        switch (toggleDTButtonSelection)
        {
            case AssignableButton.NorthFace:
                toggleDTButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                toggleDTButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                toggleDTButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                toggleDTButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                toggleDTButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                toggleDTButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                toggleDTButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                toggleDTButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                toggleDTButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                toggleDTButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignBurst()
    {
        switch (burstButtonSelection)
        {
            case AssignableButton.NorthFace:
                burstButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                burstButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                burstButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                burstButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                burstButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                burstButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                burstButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                burstButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                burstButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                burstButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignLockOn()
    {
        switch (lockOnButtonSelection)
        {
            case AssignableButton.NorthFace:
                lockOnButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                lockOnButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                lockOnButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                lockOnButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                lockOnButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                lockOnButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                lockOnButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                lockOnButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                lockOnButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                lockOnButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    void AssignGrapple()
    {
        switch (grappleButtonSelection)
        {
            case AssignableButton.NorthFace:
                grappleButton = Gamepad.current.buttonNorth;
                break;
            case AssignableButton.EastFace:
                grappleButton = Gamepad.current.buttonEast;
                break;
            case AssignableButton.SouthFace:
                grappleButton = Gamepad.current.buttonSouth;
                break;
            case AssignableButton.WestFace:
                grappleButton = Gamepad.current.buttonWest;
                break;
            case AssignableButton.R1:
                grappleButton = Gamepad.current.rightShoulder;
                break;
            case AssignableButton.R2:
                grappleButton = Gamepad.current.rightTrigger;
                break;
            case AssignableButton.L1:
                grappleButton = Gamepad.current.leftShoulder;
                break;
            case AssignableButton.L2:
                grappleButton = Gamepad.current.leftTrigger;
                break;
            case AssignableButton.L3:
                grappleButton = Gamepad.current.leftStickButton;
                break;
            case AssignableButton.R3:
                grappleButton = Gamepad.current.rightStickButton;
                break;
            default:
                break;
        }
    }

    private SavedInputs CreateSaveObject()
    {
        SavedInputs save = new SavedInputs();
        save.jumpButtonSelection = jumpButtonSelection;
        save.blockButtonSelection = blockButtonSelection;
        save.lightAttackButtonSelection = lightAttackButtonSelection;
        save.heavyAttackButtonSelection = heavyAttackButtonSelection;
        save.toggleDTButtonSelection = toggleDTButtonSelection;
        save.burstButtonSelection = burstButtonSelection;
        save.lockOnButtonSelection = lockOnButtonSelection;
        save.grappleButtonSelection = grappleButtonSelection;        

        return save;
    }

    public void SaveInputs()
    {
        SavedInputs save = CreateSaveObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/inputs.file");
        formatter.Serialize(file, save);
        file.Close();

        Debug.Log("Unlocks Saved");
        //finder.messages.CreateMajorMessage("Checkpoint Reached", Color.blue, 2.0f);

    }

    public void LoadInputs()
    {
        if (File.Exists(Application.persistentDataPath + "/inputs.file"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/inputs.file", FileMode.Open);
            SavedInputs save = (SavedInputs)formatter.Deserialize(file);
            file.Close();

            jumpButtonSelection = save.jumpButtonSelection;
            blockButtonSelection = save.blockButtonSelection;
            lightAttackButtonSelection = save.lightAttackButtonSelection;
            heavyAttackButtonSelection = save.heavyAttackButtonSelection;
            toggleDTButtonSelection = save.toggleDTButtonSelection;
            burstButtonSelection = save.burstButtonSelection;
            lockOnButtonSelection = save.lockOnButtonSelection;
            grappleButtonSelection = save.grappleButtonSelection;
            

        }
        else //Assign default controls
        {
            jumpButtonSelection = AssignableButton.SouthFace;
            blockButtonSelection = AssignableButton.EastFace;
            lightAttackButtonSelection = AssignableButton.WestFace;
            heavyAttackButtonSelection = AssignableButton.NorthFace;
            toggleDTButtonSelection = AssignableButton.L1;
            burstButtonSelection = AssignableButton.L2;
            lockOnButtonSelection = AssignableButton.R1;
            grappleButtonSelection = AssignableButton.R2;
        }
    }


}
