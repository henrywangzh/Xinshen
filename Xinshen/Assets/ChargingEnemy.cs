using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : Enemy
{
    [SerializeField] Transform targetTrfm;
    [SerializeField] ParticleSystem telegraphFX, bloodFX;
    [SerializeField] float lockRange, speed, turnRate;
    [SerializeField] int[] cooldownRange;
    [SerializeField] int attackCooldown, attackTimer;
    Rigidbody rb;

    int phase;
    const int APPROACHING = 0, IN_RANGE = 1, IDLE = 2;
    bool inLockRange;

    Vector3 movementVector;
    Vector3 facingVector;
    Transform trfm;
    // Start is called before the first frame update
    new void Start()
    {
        rb = GetComponent<Rigidbody>();
        trfm = transform;

        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsStunned())
        {
            if (phase == IDLE)
            {
                if (attackCooldown > 0)
                {
                    if (attackCooldown == 1)
                    {
                        phase = APPROACHING;
                        telegraphFX.Play();
                    }
                    attackCooldown--;
                }
            }
            else if (phase == APPROACHING || phase == IN_RANGE)
            {
                movementVector = trfm.forward * speed;
                movementVector.y = rb.velocity.y;
                rb.velocity = movementVector;

                facingVector = ((targetTrfm.position - trfm.position) - trfm.forward) * turnRate;
                facingVector.y = 0;
                trfm.forward += facingVector;

                if ((trfm.position - targetTrfm.position).sqrMagnitude < lockRange * lockRange)
                {
                    if (phase == APPROACHING)
                    {
                        phase = IN_RANGE;
                    }
                }
                else if (phase == IN_RANGE)
                {
                    attackCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
                    phase = IDLE;
                }
            }
        }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
