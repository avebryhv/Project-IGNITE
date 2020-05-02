using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maxHealthUpItem : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        switch (index)
        {
            case 1:
                if (FindObjectOfType<PlayerUnlocks>().foundHealth1)
                {
                    Destroy(gameObject);
                }
                break;
            case 2:
                if (FindObjectOfType<PlayerUnlocks>().foundHealth2)
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            FindObjectOfType<PlayerUnlocks>().FindHealthUp(index);
            Destroy(gameObject);
        }
    }
}
