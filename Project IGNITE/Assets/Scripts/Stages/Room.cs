using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool enemyLocked;
    public List<EnemyBaseBehaviour> roomEnemies;
    public bool roomEnabled;

    public GameObject entryDoor;
    public GameObject exitDoor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roomEnabled && enemyLocked)
        {
            for (int i = 0; i < roomEnemies.Count; i++)
            {
                if (roomEnemies[i] == null)
                {
                    roomEnemies.RemoveAt(i);
                }
                
            }

            if (roomEnemies.Count <= 0)
            {
                UnlockRoom();
                roomEnabled = false;
            }
        }
    }

    void LockRoom()
    {
        entryDoor.SetActive(true);
        exitDoor.SetActive(true);
    }

    void UnlockRoom()
    {
        entryDoor.SetActive(false);
        exitDoor.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            roomEnabled = true;
            for (int i = 0; i < roomEnemies.Count; i++)
            {
                roomEnemies[i].Activate();
            }

            if (enemyLocked)
            {
                LockRoom();
            }
        }
        
    }
}
