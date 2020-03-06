using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<EnemyBaseBehaviour> roomEnemies;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            for (int i = 0; i < roomEnemies.Count; i++)
            {
                roomEnemies[i].Activate();
            }
        }
        
    }
}
