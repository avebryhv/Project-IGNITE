using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool inIntro;
    public Animator introAnim;
    public bool readyToPressStart;

    // Start is called before the first frame update
    void Start()
    {
        inIntro = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            if (!inIntro)
            {
                SkipIntro();
            }

            if (readyToPressStart)
            {
                TrainingRoom();
            }
            
        }
    }

    public void SkipIntro()
    {
        introAnim.Play("spaceShot", 0, 0);
        inIntro = true;
    }

    public void TrainingRoom()
    {
        SceneManager.LoadScene("TestingRoom");
    }


}


