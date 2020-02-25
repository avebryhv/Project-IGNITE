using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMessages : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    bool showingMessage;

    float lingerTime;
    float lingerCounter;

    // Start is called before the first frame update
    void Start()
    {
        messageText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showingMessage)
        {
            lingerCounter += Time.deltaTime;
            if (lingerCounter >= lingerTime)
            {
                messageText.enabled = false;
                showingMessage = false;
            }
        }
    }

    public void CreateMessage(string text, Color col, float duration)
    {
        messageText.enabled = true;
        showingMessage = true;
        messageText.text = text;
        messageText.color = col;
        lingerCounter = 0;
        lingerTime = duration;
    }
}
