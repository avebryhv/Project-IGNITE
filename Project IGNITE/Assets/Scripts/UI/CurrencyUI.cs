using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    

        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    public void UpdateCurrencyUI(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void SaveCurrency()
    {

    }
}
