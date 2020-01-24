using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    PlayerScriptFinder finder;
    float lastDirection;
    bool canTurnSprite;
    public GameObject spriteHolder;

    // Start is called before the first frame update
    void Start()
    {
        canTurnSprite = true;
        lastDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastDirection != finder.movement.lastDirection)
        {
            if (canTurnSprite)
            {
                lastDirection = finder.movement.lastDirection;
                TurnPlayerSprite();
            }

        }
    }

    void TurnPlayerSprite()
    {
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * -1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
