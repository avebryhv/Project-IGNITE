using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsabilityData : MonoBehaviour
{
    public bool sendingEnabled;

    float comboScoreTotal;
    float damageTaken;
    float timeToClear;

    

    string url;

    // Start is called before the first frame update
    void Start()
    {
        url = "https://docs.google.com/forms/d/e/1FAIpQLSdAMRSvW_sh-2ucaJgGeMusGqkHiO_TT2cM-W9Y3Rhs-VJraQ/formResponse";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSending()
    {
        if (sendingEnabled)
        {            
            RetrieveData();
        }
    }


    void RetrieveData()
    {
        comboScoreTotal = LevelManager.Instance.totalComboScore;
        damageTaken = LevelManager.Instance.totalDamage;
        timeToClear = LevelManager.Instance.timeSpentInLevel;
        StartCoroutine("SendData");
    }

    IEnumerator SendData()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1878775528", comboScoreTotal.ToString());
        form.AddField("entry.2014317904", damageTaken.ToString());
        form.AddField("entry.275986314", timeToClear.ToString());        
        byte[] rawData = form.data;
        WWW www = new WWW(url, rawData);
        yield return www;
    }
}
