using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] int trackingRange, lungingRange, lungeCooldown;
    [SerializeField] EnemyAttack attackScript;
    Transform trfm;
    Rigidbody rb;

    Vector3 vect3;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        target = PredictionManager.playerTrfm;
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

            if (lungeCooldown > 0)
            {
                lungeCooldown--;
            }
            else
            {
                rb.velocity += trfm.forward * speed;

                if (Vector3.SqrMagnitude(target.position - trfm.position) < lungingRange * lungingRange)
                {
                    rb.velocity += Vector3.up * 6 + trfm.forward * 4;
                    attackScript.EnableHitbox(25);
                    lungeCooldown = 125 + Random.Range(0, 100);
                }
            }
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
