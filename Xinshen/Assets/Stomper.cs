using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : Enemy
{
    [SerializeField] bool isStomper;
    [SerializeField] Transform target;
    [SerializeField] int trackingRange, jumpCooldown;
    [SerializeField] float minHorzJump, maxHorzJump, vertJump;
    [SerializeField] EnemyAttack attackScript;
    int smashTimer;
    Transform trfm;
    Rigidbody rb;

    Vector3 vect3;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        trfm = transform;
        target = PredictionManager.playerTrfm;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.SqrMagnitude(target.position - trfm.position) < trackingRange * trackingRange)
        {
            vect3 = target.position - trfm.position;
            vect3.y = 0;
            trfm.forward = vect3;

            if (jumpCooldown > 0)
            {
                jumpCooldown--;
            }
            else
            {
                jumpCooldown = 100 + Random.Range(0, 60);
                rb.velocity += trfm.forward * Random.Range(minHorzJump,maxHorzJump) + Vector3.up * vertJump;

                if (isStomper)
                {
                    smashTimer = 36;
                }
                else
                {
                    attackScript.EnableHitbox(45);
                }
            }

            if (isStomper && smashTimer > 0)
            {
                if (smashTimer == 1)
                {
                    attackScript.EnableHitbox(14);
                    rb.velocity += trfm.up * -10 + trfm.forward * maxHorzJump * 1.3f;
                }
                smashTimer--;
            }
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
