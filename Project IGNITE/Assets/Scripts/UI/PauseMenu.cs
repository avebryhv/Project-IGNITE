using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public Canvas pauseMenuCanvas;
    public Selectable resumeButton;
    public EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.enabled = false;
        GameManager.Instance.SetGamePaused(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseButtonPressed()
    {
        if (isPaused)
        {
            HidePauseMenu();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    void ShowPauseMenu()
    {
        isPaused = true;
        GameManager.Instance.SetGamePaused(true);
        pauseMenuCanvas.enabled = true;
        Time.timeScale = 0;
        resumeButton.Select();
    }

    void HidePauseMenu()
    {
        isPaused = false;
        GameManager.Instance.SetGamePaused(false);
        pauseMenuCanvas.enabled = false;
        Time.timeScale = 1;
        eventSystem.SetSelectedGameObject(null);
    }
}
