using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool inIntro;
    public Animator introAnim;
    public bool readyToPressStart;
    public GameObject buttonsPanel;
    public Selectable firstSelectable;
    public GameObject startText;
    public GameObject quitPanel;
    public Selectable quitSelectable;
    EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        inIntro = false;
        readyToPressStart = true;
        buttonsPanel.SetActive(false);
        startText.SetActive(true);
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.usingController)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                StartPressed();
            }
        }
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartPressed();
        }
        
    }

    void StartPressed()
    {
        if (readyToPressStart)
        {
            buttonsPanel.SetActive(true);
            firstSelectable.Select();
            startText.SetActive(false);
            readyToPressStart = false;
        }
    }

    public void SkipIntro()
    {
        introAnim.Play("spaceShot", 0, 0);
        inIntro = true;
    }

    public void TrainingRoom()
    {
        LevelManager.Instance.LoadLevelFromStart("TestingRoom");
    }

    public void TutorialRoom()
    {
        LevelManager.Instance.LoadLevelFromStart("TutorialLevel");
    }

    public void ShowQuitMenu()
    {
        quitPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        quitSelectable.Select();
    }

    public void CancelQuit()
    {
        quitPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        firstSelectable.Select();
    }

    public void QuitGame()
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


