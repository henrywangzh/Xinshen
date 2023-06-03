using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Enemy
{
    [SerializeField] Transform target, spinPtclTrfm;
    [SerializeField] float speed;
    [SerializeField] int trackingRange, attackRange, spinCooldown, spinTimer;
    [SerializeField] EnemyAttack attackScript;
    [SerializeField] ParticleSystem spinPtcls;
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
                
            if (spinTimer > 0)
            {
                if (spinTimer < 151)
                {
                    spinPtclTrfm.localEulerAngles += Vector3.up * 10;

                    rb.velocity += trfm.forward * speed;
                    if (spinTimer % 10 == 0)
                    {
                        attackScript.ToggleHitbox();
                    }

                    if (spinTimer == 1)
                    {
                        spinPtcls.Stop();
                        attackScript.DisableHitbox();
                    }
                }

                spinTimer--;
            }

            if (spinCooldown > 0)
            {
                spinCooldown--;
            }
            else
            {
                rb.velocity += trfm.forward * speed;

                if (Vector3.SqrMagnitude(target.position - trfm.position) < attackRange * attackRange)
                {
                    spinPtcls.Play();
                    spinTimer = 200;
                    spinCooldown = 250 + Random.Range(0, 100);
                }
            }
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
