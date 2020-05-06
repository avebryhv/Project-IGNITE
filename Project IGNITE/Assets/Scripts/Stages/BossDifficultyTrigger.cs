using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDifficultyTrigger : MonoBehaviour
{
    public BossDifficultySelect selectScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            selectScreen.Show();
            Destroy(gameObject);
        }
    }

}
