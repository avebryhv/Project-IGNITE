using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMelee : MonoBehaviour
{
    public enum phase { None, Startup, Active, Endlag };
    public phase currentState;
    public bool inAttack;
    public AttackObject currentAttack;
    public GameObject currentHitbox;

    public EnemyBaseMovement movement;
    public ParticleSystem eyeFlashParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !inAttack)
        {
            AttackStartup();
        }
    }

    public void TriggerAttack()
    {
        if (!inAttack)
        {
            AttackStartup();
        }
    }

    void AttackStartup()
    {
        currentState = phase.Startup;
        //finder.state.ResetStateForAttack();
        inAttack = true;
        Invoke("EyeFlash", (currentAttack.startUpTime - 0.2f));
        Invoke("CreateHitbox", currentAttack.startUpTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    void CreateHitbox()
    {
        currentState = phase.Active;
        currentHitbox = Instantiate(currentAttack.hitboxObject, transform.position, transform.rotation, transform);
        currentHitbox.transform.localScale = new Vector3(currentHitbox.transform.localScale.x * movement.lastDirection, currentHitbox.transform.localScale.y, currentHitbox.transform.localScale.z);
        currentHitbox.GetComponent<EnemyMeleeHitbox>().SetDirection(movement.lastDirection);
        Invoke("StartEndLag", currentAttack.hitboxLingerTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    void StartEndLag()
    {
        currentState = phase.Endlag;
        Invoke("EndAttack", currentAttack.endingTime / GameManager.Instance.ReturnPlayerSpeed());
    }

    public void EndAttack()
    {
        currentState = phase.None;
        inAttack = false;
        //finder.movement.EndAirStall();
        //comboTimerPaused = false;
        //timeSinceLastLightAttackEnded = 0;
        //finder.movement.StopSpecialAttackMovement();
    }

    public void CancelAttacks()
    {
        inAttack = false;
        CancelInvoke();
        currentState = phase.None;        
        if (currentHitbox != null)
        {
            currentHitbox.GetComponent<EnemyMeleeHitbox>().DestroyHitbox();
        }
    }

    void EyeFlash()
    {
        eyeFlashParticle.Play();
    }
}
