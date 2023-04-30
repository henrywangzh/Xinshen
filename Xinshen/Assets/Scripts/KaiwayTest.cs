using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaiwayTest : Enemy
{
    [SerializeField] Transform target;

    [SerializeField] float leapDuration;
    [SerializeField] int range, fissureMinRange, leapChainChance;

    [SerializeField] GameObject fissureProjectile;
    [SerializeField] GameObject pillarProjectile;
    [SerializeField] int[] cooldownRange;

    int attackAnimationTimer;
    int leapCooldown, fissureCooldown, pillarCooldown, pillarCharges;
    float initialY;

    Transform trfm;
    Rigidbody rb;
    new void Start()
    {
        base.Start();

        trfm = transform;
        rb = GetComponent<Rigidbody>();

        pillarCharges = 500;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!IsStunned())
        {
            if (leapCooldown > 0) { leapCooldown--; } else
            {
                DoLeapAttack();
            }
            if (fissureCooldown > 0) { fissureCooldown--; } else
            {
                if (TargetInRange(range) && !TargetInRange(fissureMinRange))
                {
                    FaceTarget();
                    Instantiate(fissureProjectile, trfm.position, trfm.rotation);
                    SetAnimationLock(25);
                    fissureCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
                }
            }
            if (pillarCooldown > 0) { pillarCooldown--; } else
            {
                int numPillars = pillarCharges / 100;
                SetAnimationLock(15 * numPillars);

                for (int i = 0; i < numPillars; i++)
                {
                    Instantiate(pillarProjectile, trfm.position + trfm.forward * 2, trfm.rotation).GetComponent<ThrownEarthPillar>().Init(i * 15, target);
                    trfm.Rotate(Vector3.up * 360/numPillars);
                }

                pillarCharges = pillarCharges % 100;
                pillarCooldown = Random.Range(200, 600);
            }
            if (pillarCharges < 700)
            {
                pillarCharges++;
            }
        }
    }

    void DoLeapAttack()
    {
        SetAnimationLock(Mathf.RoundToInt(leapDuration * 50 + 25));

        FaceTarget();

        //leapDuration *= Vector3.Distance(target.position, trfm.position) / 10;

        rb.velocity += Vector3.up * -Physics.gravity.y / 2 * leapDuration;

        float distance = Vector3.Distance(target.position, trfm.position);
        if (distance > range) { distance = range; }
        rb.velocity += trfm.forward * distance / leapDuration;

        if (Random.Range(0,100) < leapChainChance)
        {
            leapCooldown = 25;
        }
        else
        {
            leapCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
            Stun(100);
        }
        initialY = transform.position.y;
    }

    bool IsAirborne() //TODO
    {
        return false;
    }

    bool TargetInRange(int pRange)
    {
        return (target.position - trfm.position).sqrMagnitude < pRange * pRange;
    }

    bool IsAnimationLocked()
    {
        return attackAnimationTimer > 0;
    }
    
    void SetAnimationLock(int duration)
    {
        if (attackAnimationTimer < duration) { attackAnimationTimer = duration; }
    }

    protected void FaceTarget()
    {
        trfm.forward = target.position - trfm.position;
    }

    public override void Die()
    {
        Debug.Log("GG NO RE");
        Destroy(gameObject);
    }
}
