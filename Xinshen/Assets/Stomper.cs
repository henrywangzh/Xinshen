using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : Enemy
{
    [SerializeField] bool isStomper;
    [SerializeField] Transform target;
    [SerializeField] int trackingRange, jumpCooldown;
    int smashTimer;
    Transform trfm;
    Rigidbody rb;

    Vector3 vect3;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        trfm = transform;
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
                if (isStomper) { jumpCooldown = 40 + Random.Range(0, 30); }
                else { jumpCooldown = 70 + Random.Range(0, 40); }
                rb.velocity += trfm.forward * Random.Range(4,7) + Vector3.up * 10;

                if (isStomper) { smashTimer = 35; }
            }

            if (isStomper && smashTimer > 0)
            {
                if (smashTimer == 1)
                {
                    rb.velocity += trfm.up * -15;
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
