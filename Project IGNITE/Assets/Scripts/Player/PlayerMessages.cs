using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMessages : MonoBehaviour
{
    public TextMeshProUGUI minorText;
    bool showingMinorMessage;

    public TextMeshProUGUI majorText;
    bool showingMajorMessage;

    float minorLingerTime;
    float minorLingerCounter;

    float majorLingerTime;
    float majorLingerCounter;

    // Start is called before the first frame update
    void Start()
    {
        minorText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showingMinorMessage)
        {
            minorLingerCounter += Time.deltaTime;
            if (minorLingerCounter >= minorLingerTime)
            {
                minorText.enabled = false;
                showingMinorMessage = false;
            }
        }

        if (showingMajorMessage)
        {
            majorLingerCounter += Time.deltaTime;
            if (majorLingerCounter >= majorLingerTime)
            {
                majorText.enabled = false;
                showingMajorMessage = false;
            }
        }
    }

    public void CreateMinorMessage(string text, Color col, float duration)
    {
        minorText.enabled = true;
        showingMinorMessage = true;
        minorText.text = text;
        minorText.color = col;
        minorLingerCounter = 0;
        minorLingerTime = duration;
    }

    public void CreateMajorMessage(string text, Color col, float duration)
    {
        majorText.enabled = true;
        showingMajorMessage = true;
        majorText.text = text;
        majorText.color = col;
        majorLingerCounter = 0;
        majorLingerTime = duration;
    }
}
