using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalFloorBeam : MonoBehaviour
{
    public SpriteRenderer warningSprite;
    public GameObject beamObject;
    public Animator anim;

    public float warningTime;
    bool inWarning;
    float warningCounter;
    public int damage;
    public Vector2 knockbackDirection;
    public float knockbackStrength;

    public float lingerTime;
    float lingerCounter;

    bool hasHitPlayer;
    public bool playsSound;

    // Start is called before the first frame update
    void Start()
    {
        hasHitPlayer = false;
        warningCounter = warningTime;
        lingerCounter = lingerTime;
        inWarning = true;
        warningSprite.enabled = true;
        beamObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inWarning)
        {
            warningCounter -= Time.deltaTime;
            if (warningCounter <= 0)
            {
                Shoot();
                inWarning = false;
            }
        }
        else
        {
            lingerCounter -= Time.deltaTime;
            if (lingerCounter <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }

    void Shoot()
    {
        warningSprite.enabled = false;
        beamObject.SetActive(true);
        anim.Play("shoot", 0, 0);
        if (playsSound)
        {
            AudioManager.Instance.PlaySFX("SFX/Enemies/Rival/floorLaserBeam", 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasHitPlayer && other.tag == "PlayerHurtbox")
        {
            FindObjectOfType<PlayerHealth>().OnHit(damage, knockbackDirection, knockbackStrength, transform.position);
            GameManager.Instance.DoHitLag();
            hasHitPlayer = true;
        }
        
    }
}
