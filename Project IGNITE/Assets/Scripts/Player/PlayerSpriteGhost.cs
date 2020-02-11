using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteGhost : MonoBehaviour
{
    SpriteRenderer[] spriteRendererList;
    public GameObject spriteHolder;
    public float killTime;
    float counter;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRendererList = GetComponentsInChildren<SpriteRenderer>();
        if (FindObjectOfType<PlayerMovement>().lastDirection == -1)
        {
            TurnPlayerSprite();
        }
        ChangeSpriteColour(new Color(1, 1, 1, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= killTime)
        {
            Destroy(gameObject);
        }
    }

    void TurnPlayerSprite()
    {
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * -1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }

    public void ChangeSpriteColour(Color col)
    {
        foreach (SpriteRenderer spr in spriteRendererList)
        {
            spr.color = col;
        }
    }
}
