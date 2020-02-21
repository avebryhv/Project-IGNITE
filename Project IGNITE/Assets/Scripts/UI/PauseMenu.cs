using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    bool unPausedThisFrame;
    bool submenuOpen;
    public Canvas pauseMenuCanvas;
    public Selectable resumeButton;
    public EventSystem eventSystem;

    public GameObject baseMenuPanel;
    public GameObject optionsPanel;
    public Selectable optionsFirstSelection;

    public GameObject trainingPanel;
    public Selectable trainingFirstSelection;

    public GameObject moveListPanel;
    public Selectable moveListFirstSelection;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.enabled = false;
        baseMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        trainingPanel.SetActive(false);
        moveListPanel.SetActive(false);
        GameManager.Instance.SetGamePaused(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenuCanvas.enabled && submenuOpen)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                HideSubmenus();
            }
        }
    }

    private void LateUpdate()
    {
        if (unPausedThisFrame)
        {
            unPausedThisFrame = false;
            GameManager.Instance.finder.input.allowPlayerInput = true;
        }
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

    public void ShowPauseMenu()
    {
        isPaused = true;
        GameManager.Instance.SetGamePaused(true);
        pauseMenuCanvas.enabled = true;
        baseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance.finder.input.allowPlayerInput = false;
        resumeButton.Select();
    }

    public void HidePauseMenu()
    {
        isPaused = false;
        unPausedThisFrame = true;
        GameManager.Instance.SetGamePaused(false);
        pauseMenuCanvas.enabled = false;
        optionsPanel.SetActive(false);
        trainingPanel.SetActive(false);
        moveListPanel.SetActive(false);
        submenuOpen = false;
        Time.timeScale = 1;
        eventSystem.SetSelectedGameObject(null);
    }

    public void ShowOptions()
    {
        baseMenuPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        optionsPanel.SetActive(true);
        submenuOpen = true;
        optionsFirstSelection.Select();
    }

    public void CloseOptions()
    {
        baseMenuPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        optionsPanel.SetActive(false);
        submenuOpen = false;
        resumeButton.Select();
    }

    public void ShowTraining()
    {
        baseMenuPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        trainingPanel.SetActive(true);
        submenuOpen = true;
        trainingFirstSelection.Select();
    }

    public void CloseTraining()
    {
        baseMenuPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        trainingPanel.SetActive(false);
        submenuOpen = false;
        resumeButton.Select();
    }

    public void ShowMoveList()
    {
        baseMenuPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        moveListPanel.SetActive(true);
        submenuOpen = true;
        if (moveListFirstSelection != null)
        {
            moveListFirstSelection.Select();
        }
    }

    public void CloseMoveList()
    {
        baseMenuPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        moveListPanel.SetActive(false);
        submenuOpen = false;
        resumeButton.Select();
    }

    public void HideSubmenus()
    {
        CloseMoveList();
        CloseOptions();
        CloseTraining();
    }

    public void ToggleLockOnToggle()
    {
        FindObjectOfType<PlayerMovement>().toggleLockOn = !FindObjectOfType<PlayerMovement>().toggleLockOn;
    }

    public void ToggleUnlimitedDT()
    {
        FindObjectOfType<PlayerStats>().unlimitedDT = !FindObjectOfType<PlayerStats>().unlimitedDT;
    }

    public void CloseGame()
    {

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
