using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboUI : MonoBehaviour
{
    public Image comboBar;
    public float comboBuildup;
    public float maxComboBuildup;
    public List<string> previousAttackList;
    PlayerStats stats;

    public EnemyBaseMovement testEnemyMovement;
    public TextMeshProUGUI knockbackText;
    public TextMeshProUGUI highestText;
    bool countingKnockback;
    float knockbackCounter;
    float highestKnockback;

    // Start is called before the first frame update
    void Start()
    {
        comboBar.fillAmount = 0;
        stats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float fillPercent = comboBuildup / maxComboBuildup;
        comboBar.fillAmount = fillPercent;
        if (comboBuildup > 0)
        {
            comboBuildup -= Time.deltaTime * 2;
        }
        comboBuildup = Mathf.Clamp(comboBuildup, 0, maxComboBuildup);


        if (testEnemyMovement.inKnockback)
        {
            if (!countingKnockback)
            {
                countingKnockback = true;
            }
        }
        else
        {
            if (countingKnockback)
            {
                countingKnockback = false;
                if (knockbackCounter > highestKnockback)
                {
                    highestKnockback = knockbackCounter;
                    highestText.text = "Longest: " + highestKnockback.ToString("F1");
                }
                knockbackCounter = 0;
            }
        }

        if (countingKnockback)
        {
            knockbackCounter += Time.deltaTime;
            knockbackText.text = "Combo Time: " + knockbackCounter.ToString("F1");
        }
        
    }

    public void AddComboScore(float amount, string name)
    {        
        AddToList(name);
        comboBuildup += (amount / ReturnAmountInList(name));
        stats.IncreaseDT(amount / ReturnAmountInList(name) / 3);
    }

    void AddToList(string toAdd)
    {
        previousAttackList.Add(toAdd);
        if (previousAttackList.Count > 10)
        {
            previousAttackList.RemoveAt(0);
        }
    }

    int ReturnAmountInList(string name)
    {
        int num = 0;
        for (int i = 0; i < previousAttackList.Count; i++)
        {
            if (previousAttackList[i] == name)
            {
                num++;
            }
        }
        return num;
    }
}
