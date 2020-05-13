using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    LineRenderer line;
    public int damage;
    int baseDamage;
    public float lingerTime;
    float destroyTimer;
    public Vector2 knockbackDirection;
    public float knockbackStrength;
    float direction;
    public float comboWeight; //How much landing this move will contribute to the combo meter
    public string name; //Name of attack
    public enum type { Light, Heavy, Special};
    public type attackType;
    List<GameObject> hitList; //Stores enemies that have already been hit, to prevent duplicate collisions
    public float lineFadeDelay;

    public GameObject hitEffect;

    //Hit Sounds
    //public List<AudioClip> hitSoundList;

    // Start is called before the first frame update
    void Start()
    {
        baseDamage = damage;
        hitList = new List<GameObject>();
        line = GetComponentInChildren<LineRenderer>();
        if (line != null)
        {
            SetInitialLine();
        }
        PlaySwingSound();        
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime * GameManager.Instance.ReturnPlayerSpeed();
        if (line != null && destroyTimer >= lineFadeDelay)
        {
            FadeLine();
        }
        
        if (destroyTimer >= lingerTime)
        {
            DestroyHitbox();
        }
    }

    public void DestroyHitbox()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyHurtbox")
        {
            if (!hitList.Contains(other.gameObject)) //Check if enemy has already been hit
            {
                //Debug.Log("Hit Enemy Hurtbox");
                hitList.Add(other.gameObject);
                damage = RecalculateDamage();
                other.GetComponentInParent<EnemyBaseHealth>().TakeDamage(damage, knockbackDirection * knockbackStrength, attackType);
                FindObjectOfType<ComboUI>().AddComboScore(comboWeight, name);
                FindObjectOfType<DronesBehaviour>().ReduceCooldown(1);
                //GameManager.Instance.DoHitLag(0.01f);
                GameManager.Instance.TriggerHitLagAlt(0.1f);
                Vector2 pos = other.transform.position;
                pos += new Vector2(Random.Range(-0.1f,0.1f), Random.Range(-0.5f, 0.5f));
                Instantiate(hitEffect, pos, transform.rotation, other.transform);
                PlayRandomHitSound();
                GameManager.Instance.TriggerSmallRumble(0.05f);
            }
            
        }

        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }
    }

    int RecalculateDamage()
    {
        ComboUI combo = FindObjectOfType<ComboUI>();
        float percent = combo.comboBuildup / combo.maxComboBuildup;
        float modifier = 1f + (0.3f * percent);
        int newDamage = Mathf.RoundToInt((float)baseDamage * modifier);
        return newDamage;
    }

    public void SetDirection(float dir)
    {
        direction = dir;
        knockbackDirection.x *= dir;
    }

    void FadeLine()
    {
        Gradient gr = line.colorGradient;
        GradientAlphaKey[] alpha = gr.alphaKeys;
        alpha[0].time += (Time.deltaTime / (lingerTime - lineFadeDelay));
        alpha[1].time += (Time.deltaTime / (lingerTime - lineFadeDelay));
        alpha[0].time = Mathf.Clamp(alpha[0].time, 0, 1);
        alpha[1].time = Mathf.Clamp(alpha[1].time, 0, 1);
        gr.SetKeys(gr.colorKeys, alpha);
        line.colorGradient = gr;
    }

    void SetInitialLine()
    {
        Gradient gr = line.colorGradient;
        GradientAlphaKey[] alpha = gr.alphaKeys;
        alpha[1].time = 0.0f;
        alpha[0].time = 0.01f;
        alpha[0].time = Mathf.Clamp(alpha[0].time, 0, 1);
        alpha[1].time = Mathf.Clamp(alpha[1].time, 0, 1);
        gr.SetKeys(gr.colorKeys, alpha);
        line.colorGradient = gr;
    }

    void PlaySwingSound()
    {
        //int toPlay = Random.Range(1, 4);
        //string path = "SFX/Player/Sword Swing/swing0" + toPlay;
        AudioClip aud = Resources.Load<AudioClip>("SFX/Player/Sword Swing/swing01");
        AudioManager.Instance.PlaySFX(aud, 1f);
    }

    void PlayRandomHitSound()
    {
        int toPlay = Random.Range(1, 10);
        string path = "SFX/Player/Sword Hit Sounds/0" + toPlay;
        AudioClip aud = Resources.Load<AudioClip>(path);
        AudioManager.Instance.PlaySFX(aud, 0.3f);
    }
}
