using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    EnemyBaseMovement baseMovement;
    Animator anim;
    SpriteRenderer[] spriteRendererList;
    public GameObject spriteHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        baseMovement = GetComponentInParent<EnemyBaseMovement>();
        anim = GetComponent<Animator>();
        spriteRendererList = GetComponentsInChildren<SpriteRenderer>();
        ChangeSpriteColour(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        if (baseMovement.inKnockback)
        {
            anim.Play("knockback", 0, 0);
        }
        else
        {
            anim.Play("idle", 0, 0);
        }
    }

    public void ChangeSpriteColour(Color col)
    {
        foreach (SpriteRenderer spr in spriteRendererList)
        {
            spr.color = col;
        }
    }

    public void SetDirection(float dir)
    {
        spriteHolder.transform.localScale = new Vector3(spriteHolder.transform.localScale.x * dir, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
    }
}
