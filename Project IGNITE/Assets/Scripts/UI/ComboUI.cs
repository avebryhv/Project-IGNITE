using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    public Image comboBar;
    public float comboBuildup;
    public float maxComboBuildup;
    // Start is called before the first frame update
    void Start()
    {
        comboBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float fillPercent = comboBuildup / maxComboBuildup;
        comboBar.fillAmount = fillPercent;
        comboBuildup -= Time.deltaTime;
    }

    public void AddComboScore(float amount)
    {
        comboBuildup += amount;
    }
}
