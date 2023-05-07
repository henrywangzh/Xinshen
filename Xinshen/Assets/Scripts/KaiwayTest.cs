using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBendingBoss : Enemy
{
    [SerializeField] Transform predictionIndicator;

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
        playerPositions = new Vector3[10];

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
                    FaceTarget(Vector3.Distance(target.position, trfm.position)/.4f);
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
                    Instantiate(pillarProjectile, trfm.position + trfm.forward * 2, trfm.rotation).GetComponent<ThrownEarthPillar>().Init(i * 15, target, this);
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

        if (calculateTimer > 0) { calculateTimer--; }
        else
        {
            calculateTimer = 5;
            CalculatePredictedPos();
            predictionIndicator.position = GetPredictedPos(2);
        }
    }

    void DoLeapAttack()
    {
        SetAnimationLock(Mathf.RoundToInt(leapDuration * 50 + 25));

        FaceTarget(leapDuration);

        //leapDuration *= Vector3.Distance(target.position, trfm.position) / 10;

        rb.velocity += Vector3.up * -Physics.gravity.y / 2 * leapDuration;

        float distance = Vector3.Distance(GetPredictedPos(leapDuration), trfm.position);
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

    protected void FaceTarget(float predictionTime = 0)
    {
        trfm.forward = GetPredictedPos(predictionTime) - trfm.position;
    }

    [SerializeField] Vector3[] playerPositions;
    Vector3 predictedOffset;
    int addPos, calculateTimer;
    public Vector3 GetPredictedPos(float seconds)
    {
        int newestPos = addPos - 3;
        if (newestPos < 0) { newestPos += 10; }
        predictedOffset = (target.position - playerPositions[addPos]) * seconds * .5f;
        predictedOffset += (target.position - playerPositions[newestPos]) * 4 * seconds * .5f;
        return predictedOffset + target.position;
    }
    private void CalculatePredictedPos()
    {
        playerPositions[addPos] = target.position;
        addPos++;
        if (addPos > 9) { addPos = 0; }
    }

    public override void Die()
    {
        Debug.Log("GG NO RE");
        Destroy(gameObject);
    }
}
