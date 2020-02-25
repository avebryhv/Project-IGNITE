using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBladeHitbox : MonoBehaviour
{
    List<GameObject> hitList; //Stores enemies that have already been hit, to prevent duplicate collisions
    public GameObject hitEffect;

    public int damage;
    public Vector2 knockbackDirection;
    public float knockbackStrength;
    public float comboWeight;

    public float hitRate;
    float hitTimer;


    // Start is called before the first frame update
    void Start()
    {
        hitList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer >= hitRate)
        {
            hitList.Clear();
            hitTimer = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "EnemyHurtbox")
        {
            if (!hitList.Contains(other.gameObject)) //Check if enemy has already been hit
            {
                //Debug.Log("Hit Enemy Hurtbox");
                hitList.Add(other.gameObject);
                other.GetComponentInParent<EnemyBaseHealth>().TakeDamage(damage, knockbackDirection * knockbackStrength, MeleeHitbox.type.Light);
                FindObjectOfType<ComboUI>().AddComboScore(comboWeight, name);
                //GameManager.Instance.DoHitLag();
                Vector2 pos = other.transform.position;
                pos += new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-1, 2));
                Instantiate(hitEffect, pos, transform.rotation);
                PlayRandomHitSound();
            }

        }
    }

    void PlayRandomHitSound()
    {
        int toPlay = Random.Range(1, 10);
        string path = "SFX/Player/Sword Hit Sounds/0" + toPlay;
        AudioClip aud = Resources.Load<AudioClip>(path);
        AudioManager.Instance.PlaySFX(aud, 0.3f);
    }
}
