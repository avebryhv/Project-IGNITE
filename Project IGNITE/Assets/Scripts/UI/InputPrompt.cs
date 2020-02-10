using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour
{
    public Image promptImage;
    public Sprite testSprite;
    // Start is called before the first frame update
    void Start()
    {
        HidePrompt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPrompt()
    {
        promptImage.enabled = true;
        promptImage.sprite = testSprite;
    }

    public void HidePrompt()
    {
        promptImage.enabled = false;
    }
}
