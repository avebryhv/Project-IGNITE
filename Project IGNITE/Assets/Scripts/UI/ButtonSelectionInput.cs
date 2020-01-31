using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelectionInput : MonoBehaviour
{
    public TMP_Dropdown dropDown;
    public InputAssignment inputAssignment;
    public InputAssignment.AssignableButton selection;
    public InputAssignment.AssignableButton assignmentButton;
    public InputAssignment.ButtonAction action;
    // Start is called before the first frame update
    void Start()
    {
        dropDown = GetComponent<TMP_Dropdown>();
        ReadCurrentInputSelection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadCurrentInputSelection()
    {
        switch (action)
        {
            case InputAssignment.ButtonAction.Jump:
                assignmentButton = inputAssignment.jumpButtonSelection;
                break;
            case InputAssignment.ButtonAction.Block:
                assignmentButton = inputAssignment.blockButtonSelection;
                break;
            case InputAssignment.ButtonAction.LightAttack:
                assignmentButton = inputAssignment.lightAttackButtonSelection;
                break;
            case InputAssignment.ButtonAction.HeavyAttack:
                assignmentButton = inputAssignment.heavyAttackButtonSelection;
                break;
            case InputAssignment.ButtonAction.LockOn:
                assignmentButton = inputAssignment.lockOnButtonSelection;
                break;
            case InputAssignment.ButtonAction.Grapple:
                assignmentButton = inputAssignment.grappleButtonSelection;
                break;
            case InputAssignment.ButtonAction.Burst:
                assignmentButton = inputAssignment.burstButtonSelection;
                break;
            case InputAssignment.ButtonAction.DT:
                assignmentButton = inputAssignment.toggleDTButtonSelection;
                break;
            default:
                break;
        }

        int value = 0;

        switch (assignmentButton)
        {
            case InputAssignment.AssignableButton.NorthFace:
                value = 3;
                break;
            case InputAssignment.AssignableButton.EastFace:
                value = 1;
                break;
            case InputAssignment.AssignableButton.SouthFace:
                value = 0;
                break;
            case InputAssignment.AssignableButton.WestFace:
                value = 2;
                break;
            case InputAssignment.AssignableButton.R1:
                value = 6;
                break;
            case InputAssignment.AssignableButton.R2:
                value = 7;
                break;
            case InputAssignment.AssignableButton.L1:
                value = 4;
                break;
            case InputAssignment.AssignableButton.L2:
                value = 5;
                break;
            case InputAssignment.AssignableButton.L3:
                value = 8;
                break;
            case InputAssignment.AssignableButton.R3:
                value = 9;
                break;
            default:
                break;
        }

        dropDown.value = value;
    }

    public void Test()
    {
        Debug.Log("chef");
    }

    public void OnChanged()
    {
        GetSelectedInput();
        SetSelection();
    }

    void GetSelectedInput()
    {
        switch (dropDown.value)
        {
            case 0:
                selection = InputAssignment.AssignableButton.SouthFace;
                break;
            case 1:
                selection = InputAssignment.AssignableButton.EastFace;
                break;
            case 2:
                selection = InputAssignment.AssignableButton.WestFace;
                break;
            case 3:
                selection = InputAssignment.AssignableButton.NorthFace;
                break;
            case 4:
                selection = InputAssignment.AssignableButton.L1;
                break;
            case 5:
                selection = InputAssignment.AssignableButton.L2;
                break;
            case 6:
                selection = InputAssignment.AssignableButton.R1;
                break;
            case 7:
                selection = InputAssignment.AssignableButton.R2;
                break;
            case 8:
                selection = InputAssignment.AssignableButton.L3;
                break;
            case 9:
                selection = InputAssignment.AssignableButton.R3;
                break;
            default:
                break;
        }
        Debug.Log(selection);
    }

    void SetSelection()
    {
        switch (action)
        {
            case InputAssignment.ButtonAction.Jump:
                inputAssignment.jumpButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.Block:
                inputAssignment.blockButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.LightAttack:
                inputAssignment.lightAttackButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.HeavyAttack:
                inputAssignment.heavyAttackButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.LockOn:
                inputAssignment.lockOnButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.Grapple:
                inputAssignment.grappleButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.Burst:
                inputAssignment.burstButtonSelection = selection;
                break;
            case InputAssignment.ButtonAction.DT:
                inputAssignment.toggleDTButtonSelection = selection;
                break;
            default:
                break;
        }
        inputAssignment.ReAssignButtons();
    }
}
