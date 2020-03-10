using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    PlayerScriptFinder finder;
    public GameObject arrow;
    Vector2 arrowDir;
    public LayerMask layerMask;
    public float hookToDistanceX;
    Vector2 hookToPoint;
    public bool inGrapple;
    bool endingGrapple;
    GameObject grappleTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inGrapple)
        {
            grappleTarget.transform.position = Vector3.Lerp(grappleTarget.transform.position, hookToPoint, 0.1f * GameManager.Instance.ReturnPlayerSpeed());
            if (Mathf.Abs(Vector2.Distance(grappleTarget.transform.position, hookToPoint)) < 0.1 && !endingGrapple)
            {
                endingGrapple = true;
                Invoke("EndGrapple", 0.2f / GameManager.Instance.ReturnPlayerSpeed());
            }
        }
    }

    public void CStickInput(Vector2 input)
    {
        if (input.magnitude == 0)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.right = input;
            arrowDir = input;
            Debug.DrawRay(transform.position, arrow.transform.right * 10, Color.cyan);
        }
    }

    public void GrappleButtonPressed()
    {
        if (!finder.melee.inAttack && !inGrapple)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, arrowDir, 10.0f, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "EnemyHurtbox")
                {
                    Debug.Log("Hook Hit");
                    Debug.Log(Mathf.Sign(arrowDir.x));
                    hookToPoint = new Vector2(transform.position.x + Mathf.Sign(arrowDir.x) * hookToDistanceX, transform.position.y);
                    Debug.Log(hookToPoint);
                    grappleTarget = hit.collider.transform.parent.gameObject;
                    StartGrapple();                    
                }
            }
        }
        

    }

    public void StartGrapple()
    {
        inGrapple = true;
        finder.melee.inAttack = true;
        finder.melee.currentState = MeleeAttacker.phase.Active;
        if (!finder.controller.collisions.below)
        {
            finder.movement.SetAirStall();
        }
        grappleTarget.GetComponentInParent<EnemyBaseMovement>().StartInGrapple();
        FindObjectOfType<ComboUI>().AddComboScore(5, "GrappleGunStart");
        endingGrapple = false;
    }

    public void EndGrapple()
    {
        inGrapple = false;
        finder.melee.inAttack = false;
        finder.melee.currentState = MeleeAttacker.phase.None;
        finder.movement.EndAirStall();
        grappleTarget.GetComponentInParent<EnemyBaseMovement>().EndInGrapple();
        FindObjectOfType<ComboUI>().AddComboScore(5, "GrappleGunEnd");
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
