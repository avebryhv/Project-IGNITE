﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    int current;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current = (int)(1f / Time.unscaledDeltaTime);
        counter.text = "FPS: " + current;
    }
}
