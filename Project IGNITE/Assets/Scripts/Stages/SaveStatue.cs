using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SaveStatue : MonoBehaviour
{
    public bool playerInFront;
    public StatueMenu menu;
    public Image interactImage;
    float timeSinceHit;
    bool canCheckPointSave;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        menu = FindObjectOfType<StatueMenu>();
        canCheckPointSave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInFront)
        {
            interactImage.enabled = true;
            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                OpenSaveStatue();
            }
            
        }
        else
        {
            interactImage.enabled = false;

        }

        if (!canCheckPointSave)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit >= 10)
            {
                canCheckPointSave = true;
                timeSinceHit = 0;
            }
        }

    }

    private void LateUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            playerInFront = true;
            if (canCheckPointSave)
            {
                CheckPointSave();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            playerInFront = false;
        }
    }

    public void OpenSaveStatue()
    {
        GameManager.Instance.SetGamePaused(true);
        
        GameManager.Instance.finder.input.allowPlayerInput = false;
        anim.Play("panelDown", 0, 0);
        Invoke("OpenMenu", 0.5f);
    }

    public void CheckPointSave()
    {
        FindObjectOfType<PlayerUnlocks>().SaveUnlocksWithCheckpoint();
        canCheckPointSave = false;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menu.OpenMenu(this);
    }

    public void CloseMenu()
    {        
        anim.Play("panelUp", 0, 0);
        Invoke("ResumePlayer", 0.5f);
    }

    public void ResumePlayer()
    {
        GameManager.Instance.finder.input.allowPlayerInput = true;
    }
}
