using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsUI : MonoBehaviour
{
    public Image healthBar;
    public Image overlay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color col = healthBar.color;
        col.a -= Time.deltaTime;
        healthBar.color = col;
        overlay.color = col;
    }

    public void SetHealthBar(int current, int max)
    {
        Color col = healthBar.color;
        col.a = 1f;
        healthBar.color = col;
        overlay.color = col;
        healthBar.fillAmount = (float)current / (float)max;
    }
}
