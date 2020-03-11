using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public Canvas screenCanvas;
    public Selectable firstButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScreen()
    {
        screenCanvas.enabled = true;
        firstButton.Select();
    }

    public void Continue()
    {

    }

    public void Restart()
    {

    }

    public void Quit()
    {

    }
}
