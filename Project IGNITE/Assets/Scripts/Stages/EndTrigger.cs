using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public UsabilityData dataSending;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            GameManager.Instance.finder.input.allowPlayerInput = false;
            GameManager.Instance.SetGamePaused(true);
            FindObjectOfType<PlayerMovement>().freezeMovement = true;
            anim.Play("doorOpen", 0, 0);
            Invoke("EndLevel", 1.2f);
        }
    }

    void EndLevel()
    {
        LevelManager.Instance.EndLevel();
        if (dataSending != null)
        {
            dataSending.StartSending();
        }
    }
}
