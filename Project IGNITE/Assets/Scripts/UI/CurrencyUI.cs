using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI newText;
    Color newTextDefaultCol;
    
        
    // Start is called before the first frame update
    void Start()
    {
        newTextDefaultCol = newText.color;
    }

    // Update is called once per frame
    void Update()
    {
        Color col = newText.color;
        col.a -= Time.deltaTime / 2;
        newText.color = col;
    }    

    public void UpdateCurrencyUI(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void SaveCurrency()
    {

    }

    public void ShowNewCurrency(int amount)
    {
        newText.text = "+" + amount.ToString();
        newText.color = newTextDefaultCol;
    }
}
