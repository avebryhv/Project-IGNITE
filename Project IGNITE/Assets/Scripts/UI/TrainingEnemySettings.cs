﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingEnemySettings : MonoBehaviour
{
    TMP_Dropdown dropdown;
    public EnemyBaseBehaviour trainingEnemy;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        switch (dropdown.value)
        {
            case 0:
                trainingEnemy.currentMode = EnemyBaseBehaviour.Mode.None;
                break;
            case 1:
                trainingEnemy.currentMode = EnemyBaseBehaviour.Mode.Attack;
                break;
            default:
                break;
        }
    }
}
