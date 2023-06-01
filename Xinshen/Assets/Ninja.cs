using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{
    [SerializeField] int slashCooldown, evadeCooldown;
    [SerializeField] int trackingRange, attackRange, evadeRange;
    [SerializeField] float strafeSpeed;
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem teleportPtcls, slashTelegraph;

    Rigidbody rb;
    Transform trfm;
    int slashTimer;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.SqrMagnitude(target.position - trfm.position) > trackingRange * trackingRange)
        {
            return;
        }

        HandleSlashAttack();
        HandleEvading();
        HandleStrafing();
    }

    void HandleStrafing()
    {
        if (slashTimer > 0) { return; }

        if (Vector3.SqrMagnitude(target.position - trfm.position) < evadeRange * evadeRange * 4)
        {
            trfm.forward = target.position;
            rb.velocity += trfm.forward * -strafeSpeed;
        }
    }

    void HandleEvading()
    {
        if (evadeCooldown > 0)
        {
            evadeCooldown--;
        } else
        {
            if (Vector3.SqrMagnitude(target.position - trfm.position) < evadeRange * evadeRange)
            {
                evadeCooldown = 150 + Random.Range(-25, 25);
                trfm.forward = target.position - trfm.position;
                Teleport(trfm.forward * evadeRange * 2);
                Slash();
            }
        }
    }

    void HandleSlashAttack()
    {
        if (slashCooldown > 0)
        {
            slashCooldown--;
        }

        if (slashTimer > 0)
        {
            if (slashTimer == 1)
            {
                slashTelegraph.Stop();
                trfm.position += trfm.forward * 10;
            }

            slashTimer--;
        }
    }

    void Slash()
    {
        slashCooldown = 150 + Random.Range(-25, 25);
        trfm.forward = PredictionManager.GetPredictedPos(1);
        slashTelegraph.Play();
        slashTimer = 50;
    }

    void Teleport(Vector3 position)
    {
        teleportPtcls.Play();
        trfm.position = position;
    }

    public override void Die()
    {
        slashTelegraph.transform.parent = null;
        slashTelegraph.Play();
        Destroy(slashTelegraph, 2);
        Destroy(gameObject);
    }
}
