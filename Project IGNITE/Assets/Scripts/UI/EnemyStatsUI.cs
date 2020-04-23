using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStatsUI : MonoBehaviour
{
    public Image healthBar;
    public Image overlay;
    public TextMeshProUGUI text;
    public bool bossMode;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossMode)
        {
            Color col = healthBar.color;
            col.a -= Time.deltaTime;
            healthBar.color = col;
            overlay.color = col;
            text.color = col;
        }
        else
        {

        }
        
    }

    public void SetHealthBar(int current, int max)
    {
        Color col = healthBar.color;
        col.a = 1f;
        healthBar.color = col;
        overlay.color = col;
        text.color = col;
        healthBar.fillAmount = (float)current / (float)max;
    }
}
