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
    public Image dtBorder;
    public Image dronesBar;
    public TextMeshProUGUI dronesText;

    //Second Health Bar
    public Image secondHealthBar;
    public Image secondHealthLerp;
    public Image secondHealthBorder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBarLerp.fillAmount = Mathf.Lerp(healthBarLerp.fillAmount, healthbar.fillAmount, Time.deltaTime);
        secondHealthLerp.fillAmount = Mathf.Lerp(secondHealthLerp.fillAmount, secondHealthBar.fillAmount, Time.deltaTime);
        if (dronesBar.fillAmount == 1)
        {
            Color droneColour = dronesBar.color;
            droneColour.a -= Time.deltaTime / 2;
            dronesBar.color = droneColour;
            dronesText.color = droneColour;
        }
    }

    public void SetHealthValue(int current, int max)
    {
        //Set First Health Bar Values
        int firstBarValue = Mathf.Clamp(current, 0, 100);

        healthbar.fillAmount = (float)firstBarValue / (float)100;

        //Set Second Bar Values
        if (max > 100) //Only relevant if max health has increased
        {
            int maxOver = max - 100;
            secondHealthBorder.fillAmount = (float)maxOver / (float)100;

            int currentOver = current - 100;
            if (currentOver > 0)
            {
                secondHealthBar.fillAmount = (float)currentOver / (float)100;
            }
            else
            {
                secondHealthBar.fillAmount = 0;
            }
        }
        else
        {
            secondHealthBorder.fillAmount = 0;
            secondHealthBar.fillAmount = 0;
            secondHealthLerp.fillAmount = 0;
        }
    }

    public void SetDTValue(float current, float max)
    {
        dtBar.fillAmount = current / 100f;
        dtBorder.fillAmount = max / 100f;
    }

    public void SetDronesValue(float current, float max)
    {
        dronesBar.fillAmount = current / max;
        Color droneColour = dronesBar.color;
        droneColour.a = 1;
        dronesBar.color = droneColour;
        dronesText.color = droneColour;
    }
}
