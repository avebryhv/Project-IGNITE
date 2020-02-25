using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Image healthbar;
    public Image healthBarLerp;
    public Image dtBar;
    public Image dronesBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBarLerp.fillAmount = Mathf.Lerp(healthBarLerp.fillAmount, healthbar.fillAmount, Time.deltaTime);
    }

    public void SetHealthValue(int current, int max)
    {
        healthbar.fillAmount = (float)current / (float)max;
    }

    public void SetDTValue(float current, float max)
    {
        dtBar.fillAmount = current / max;
    }

    public void SetDronesValue(float current, float max)
    {
        dronesBar.fillAmount = current / max;
    }
}
