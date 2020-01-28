using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Image healthbar;
    public Image dtBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthValue(int current, int max)
    {
        healthbar.fillAmount = current / max;
    }

    public void SetDTValue(float current, float max)
    {
        dtBar.fillAmount = current / max;
    }
}
