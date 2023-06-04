using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{
    [SerializeField] int slashCooldown, evadeCooldown;
    [SerializeField] int trackingRange, attackRange, evadeRange;
    [SerializeField] float strafeSpeed;
    [SerializeField] Transform target, teleportPtclTrfm;
    [SerializeField] ParticleSystem teleportPtcls, slashTelegraph, slashAttackPtcls;
    [SerializeField] EnemyAttack attackHitbox;

    Rigidbody rb;
    Transform trfm;
    int slashTimer;
    int slashCharges;

    Vector3 vect3;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        trfm = transform;
        teleportPtclTrfm.parent = null;
        target = PredictionManager.playerTrfm;

        strafeDirection = Random.Range(0, 2) * 2 - 1;
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
    }

    int strafeDirection;
    void Strafe()
    {
        if (slashTimer > 0) { return; }

        if (Vector3.SqrMagnitude(target.position - trfm.position) < evadeRange * evadeRange * 4)
        {
            trfm.forward = target.position - trfm.position;
            rb.velocity += trfm.forward * -strafeSpeed + trfm.right * strafeSpeed * strafeDirection;
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
                evadeCooldown = 250;

                vect3 = target.position - trfm.position;
                vect3.y = 0;
                vect3 = vect3.normalized;

                if (Physics.Raycast(trfm.position, trfm.forward, out hit, evadeRange * 2, terrainLayerMask))
                {
                    Debug.Log("hit distance: " + hit.distance + "hit object: " + hit.collider.gameObject);
                    Teleport(trfm.position + vect3 * hit.distance);
                }
                else
                {
                    Teleport(trfm.position + vect3 * evadeRange * 2);
                }

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
            if (slashTimer == 12)
            {
                slashTelegraph.Stop();
            }

            if (slashTimer == 7)
            {
                slashAttackPtcls.Play();
            }

            if (slashTimer == 3)
            {
                if (Physics.Raycast(trfm.position, trfm.forward, out hit, 11, terrainLayerMask))
                {
                    Debug.Log("hit distance: " + hit.distance + "hit object: " + hit.collider.gameObject);
                    trfm.position += trfm.forward * hit.distance;
                }
                else
                {
                    trfm.position += trfm.forward * 11;
                }
                
                attackHitbox.EnableHitbox();
            }

            if (slashTimer == 1)
            {
                attackHitbox.DisableHitbox();
            }

            slashTimer--;
        }
        else if (Vector3.SqrMagnitude(target.position - trfm.position) < attackRange * attackRange)
        {
            if (slashCooldown > 0)
            {
                Strafe();
            }
            else
            {
                Slash();
            }
        }
    }

    RaycastHit hit;
    void Slash()
    {
        if (slashCharges > 0)
        {
            slashCooldown = 25;
            slashCharges--;
        }
        else
        {
            slashCooldown = 125 + Random.Range(0, 50);

            if (GetHP() > 50) { slashCharges = Random.Range(2, 4); }
            else { slashCharges = Random.Range(4, 6); }
        }
        
        vect3 = PredictionManager.GetPredictedPos(.45f, false) - trfm.position;
        vect3.y = 0;

        trfm.forward = vect3;
        slashTelegraph.Play();
        slashTimer = 30;
    }

    void Teleport(Vector3 position)
    {
        teleportPtclTrfm.position = trfm.position;
        teleportPtcls.Play();
        trfm.position = position;
    }

    public override void Die()
    {
        slashTelegraph.transform.parent = null;
        slashTelegraph.Play();
        Destroy(teleportPtclTrfm.gameObject);
        Destroy(slashTelegraph);
        Destroy(gameObject);
    }
}
