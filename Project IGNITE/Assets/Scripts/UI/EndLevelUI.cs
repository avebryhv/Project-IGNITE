using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    public Canvas canvas;
    public LevelScoreRanking currentLevelRanks;

    public TextMeshProUGUI styleText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI totalText;

    int styleRank;
    int damageRank;
    int timeRank;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowEndingScores()
    {
        
        int style = CalculateStyle();
        int damage = CalculateDamage();
        int time = CalculateTime();
        styleText.text = RankingToLetter(style);
        damageText.text = RankingToLetter(damage);
        timeText.text = RankingToLetter(time);
        int total = CalculateTotal(style + time + damage);
        totalText.text = RankingToLetter(total);

        canvas.enabled = true;
    }

    public int CalculateStyle()
    {
        for (int i = 0; i < currentLevelRanks.styleRanks.Count; i++)
        {
            if (LevelManager.Instance.totalComboScore >= currentLevelRanks.styleRanks[i])
            {
                return i;                
            }
        }
        return currentLevelRanks.styleRanks.Count - 1;
    }

    public int CalculateDamage()
    {
        for (int i = 0; i < currentLevelRanks.styleRanks.Count; i++)
        {
            if (LevelManager.Instance.totalDamage <= currentLevelRanks.damageRanks[i])
            {
                return i;
            }
        }
        return currentLevelRanks.damageRanks.Count - 1;
    }

    public int CalculateTime()
    {
        for (int i = 0; i < currentLevelRanks.timeRanks.Count; i++)
        {
            if (LevelManager.Instance.timeSpentInLevel <= currentLevelRanks.timeRanks[i])
            {
                return i;
            }
        }
        return currentLevelRanks.timeRanks.Count - 1;
    }

    public int CalculateTotal(int totalRank)
    {
        if (totalRank <= 1)
        {
            return 0;
        }
        else if (totalRank <= 4)
        {
            return 1;
        }
        else if (totalRank <= 7)
        {
            return 2;
        }
        else if (totalRank <= 10)
        {
            return 3;
        }
        else if (totalRank <= 13)
        {
            return 4;
        }
        else if (totalRank <= 16)
        {
            return 5;
        }
        else
        {
            return 6;
        }
        
    }

    public string RankingToLetter(int ranking)
    {
        switch (ranking)
        {
            case 0:
                return "SSS";
            case 1:
                return "SS";
            case 2:
                return "S";
            case 3:
                return "A";
            case 4:
                return "B";
            case 5:
                return "C";
            case 6:
                return "D";

            default:
                return "-";
        }
    }
}
