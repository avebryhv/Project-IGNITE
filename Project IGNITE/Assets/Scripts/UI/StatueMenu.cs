using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueMenu : MonoBehaviour
{
    public Canvas menu;
    UnlocksUI unlocksUI;

    // Start is called before the first frame update
    void Start()
    {
        unlocksUI = FindObjectOfType<UnlocksUI>();
        unlocksUI.CloseMenu();
        menu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        menu.enabled = true;
        unlocksUI.gameObject.SetActive(true);
        unlocksUI.ShowMenu();
    }

    public void CloseMenu()
    {
        menu.enabled = false;
        GameManager.Instance.SetGamePaused(false);
        Time.timeScale = 1;
        GameManager.Instance.finder.input.allowPlayerInput = true;
    }
}
