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

    public GameObject baseMenuPanel;
    public GameObject optionsPanel;
    public Selectable optionsFirstSelection;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.enabled = false;
        baseMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
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

    public void Resume()
    {
        HidePauseMenu();
    }

    void ShowPauseMenu()
    {
        isPaused = true;
        GameManager.Instance.SetGamePaused(true);
        pauseMenuCanvas.enabled = true;
        baseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        resumeButton.Select();
    }

    void HidePauseMenu()
    {
        isPaused = false;
        GameManager.Instance.SetGamePaused(false);
        pauseMenuCanvas.enabled = false;
        optionsPanel.SetActive(false);
        Time.timeScale = 1;
        eventSystem.SetSelectedGameObject(null);
    }

    public void ShowOptions()
    {
        baseMenuPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        optionsPanel.SetActive(true);
        optionsFirstSelection.Select();
    }

    public void CloseOptions()
    {
        baseMenuPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        optionsPanel.SetActive(false);
        resumeButton.Select();
    }

    public void ToggleLockOnToggle()
    {
        FindObjectOfType<PlayerMovement>().toggleLockOn = !FindObjectOfType<PlayerMovement>().toggleLockOn;
    }
}
