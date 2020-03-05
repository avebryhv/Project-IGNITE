using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : EnemyBaseBehaviour
{
    public float gunCooldown;
    float gunTimer;
    public float gunRange;
    public GameObject gunPoint;
    public GameObject gunArm;
    public ParticleSystem thrusterParticles;



    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        gunTimer = gunCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        Actions();
    }

    public override void Actions()
    {
        base.Actions();
        if (currentMode == Mode.Attack)
        {
            if (!movement.inKnockback && !movement.inHitStun && !movement.hitThisFrame)
            {
                float xDifference = player.transform.position.x - transform.position.x;
                float yDifference = player.transform.position.y - transform.position.y;
                if (movement.lastDirection != Mathf.Sign(xDifference))
                {
                    movement.lastDirection = Mathf.Sign(xDifference);
                    sprite.TurnSprite();
                }

                if (Mathf.Abs(xDifference) >= gunRange && CheckOnScreen())
                {
                    gunTimer -= Time.deltaTime;
                    if (gunTimer <= 0)
                    {
                        sprite.currentAttackAnimName = "shoot";
                        gun.TriggerShot(0.25f);
                        gunTimer = gunCooldown;
                    }
                }

                if (yDifference > -3)
                {
                    velocity.y = movement.moveSpeed; // Move up if player gets too close from below
                }
                else if (yDifference < -4) //Move down if not close enough
                {
                    velocity.y = -movement.moveSpeed;
                }
                else
                {
                    velocity.y = 0;
                }
                

                if (Mathf.Abs(xDifference) <= 3)
                {
                    velocity.x = -movement.moveSpeed * movement.lastDirection;
                }
                else if (Mathf.Abs(xDifference) >= 4)
                {
                    velocity.x = movement.moveSpeed * movement.lastDirection;
                }
                else
                {
                    velocity.x = 0;
                }


                //Rotate Gun
                Vector2 dir = new Vector2(xDifference * movement.lastDirection, yDifference * movement.lastDirection);
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                gunArm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                //Play Particles
                if (!thrusterParticles.isPlaying)
                {
                    thrusterParticles.Play();
                }


            }
            else
            {
                if (thrusterParticles.isPlaying)
                {
                    thrusterParticles.Stop();
                }
            }
        }
        else
        {
            velocity.x = 0;
        }
    }
}
