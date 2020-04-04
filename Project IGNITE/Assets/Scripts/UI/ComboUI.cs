using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboUI : MonoBehaviour
{
    public Image comboBar;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboTitle;
    public Image panelBG;
    public Image comboBarBG;
    public float comboBuildup;
    public float maxComboBuildup;
    public List<string> previousAttackList;
    PlayerStats stats;

    public EnemyBaseMovement testEnemyMovement;
    public TextMeshProUGUI knockbackText;
    public TextMeshProUGUI highestText;
    public bool trainingMode;
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
        CalculateLetter();

        if (trainingMode)
        {
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
                knockbackCounter += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
                knockbackText.text = "Combo Time: " + knockbackCounter.ToString("F1");
            }
        }

        if (comboBuildup <= 0)
        {
            Fade();
        }
        else
        {
            Show();
        }
        
        
    }

    public void AddComboScore(float amount, string name)
    {        
        AddToList(name);
        comboBuildup += (amount / ReturnAmountInList(name));
        stats.IncreaseDT(amount / ReturnAmountInList(name) / 3);
        LevelManager.Instance.AddComboScore(amount);
    }

    public void AddComboScore(float amount, string name, bool addsToList)
    {
        if (addsToList)
        {
            AddToList(name);
            comboBuildup += (amount / ReturnAmountInList(name));
            LevelManager.Instance.AddComboScore(amount / ReturnAmountInList(name));
            stats.IncreaseDT(amount / ReturnAmountInList(name) / 3);
        }
        else
        {
            comboBuildup += (amount);
            LevelManager.Instance.AddComboScore(amount);
            stats.IncreaseDT(amount / 3);
        }
        
        
    }

    public void ReduceComboScore(float amount)
    {
        comboBuildup -= amount;
    }

    void AddToList(string toAdd)
    {
        previousAttackList.Add(toAdd);
        if (previousAttackList.Count > 5)
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

    void CalculateLetter()
    {
        float percentage = comboBuildup / maxComboBuildup;
        string newLetter = "";
        if (percentage >= 0.95)
        {
            newLetter = "SSS";
        }
        else if (percentage >= 0.80)
        {
            newLetter = "SS";
        }
        else if (percentage >= 0.65)
        {
            newLetter = "S";
        }
        else if (percentage >= 0.5)
        {
            newLetter = "A";
        }
        else if (percentage >= 0.4)
        {
            newLetter = "B";
        }
        else if (percentage >= 0.3)
        {
            newLetter = "C";
        }
        else if (percentage >= 0.2)
        {
            newLetter = "D";
        }
        comboText.text = newLetter;
    }

    void Fade()
    {
        Color col = comboBar.color;
        Color bgCol = new Color(0.5f, 0.5f, 0.5f, comboBar.color.a);
        col.a -= Time.deltaTime;
        comboBar.color = col;
        comboBarBG.color = bgCol;
        panelBG.color = col;
        comboTitle.color = col;

    }

    void Show()
    {
        Color col = new Color(1,1,1,1);
        Color bgCol = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        col.a -= Time.deltaTime;
        comboBar.color = col;
        comboBarBG.color = bgCol;
        panelBG.color = col;
        comboTitle.color = col;
    }
}
