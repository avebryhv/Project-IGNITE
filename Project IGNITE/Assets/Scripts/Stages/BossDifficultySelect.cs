using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossDifficultySelect : MonoBehaviour
{
    public Canvas displayCanvas;
    public Selectable firstSelection;
    EventSystem eventSystem;
    public RivalBehaviour bossBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        displayCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        displayCanvas.enabled = true;
        firstSelection.Select();
        Time.timeScale = 0;
        GameManager.Instance.finder.input.allowPlayerInput = false;

    }

    public void Hide()
    {
        displayCanvas.enabled = false;
        eventSystem.SetSelectedGameObject(null);
        Time.timeScale = 1;
        GameManager.Instance.finder.input.allowPlayerInput = true;
    }

    public void SetEasy()
    {
        bossBehaviour.SetDifficulty(RivalBehaviour.Difficulty.Easy);
        Hide();
    }

    public void SetMedium()
    {
        bossBehaviour.SetDifficulty(RivalBehaviour.Difficulty.Medium);
        Hide();
    }

    public void SetHard()
    {
        bossBehaviour.SetDifficulty(RivalBehaviour.Difficulty.Hard);
        Hide();
    }


}
