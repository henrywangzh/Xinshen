using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBendingBoss : Enemy
{
    [SerializeField] BoxCollider pillarDestroyer;

    [SerializeField] Material defaultMaterial, redMaterial;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] ParticleSystem leapTelegraph, fissureTelegraph;
    [SerializeField] GroundTrigger groundTrigger;
    [SerializeField] EnemyAttack rammingHitbox;

    [SerializeField] Transform target;

    [SerializeField] float leapDuration;
    [SerializeField] int range, fissureMinRange, leapChainChance;

    [SerializeField] GameObject fissureProjectile, pillarProjectile, fallingPillar, fallingPillarTelegraph;
    [SerializeField] int[] cooldownRange;

    int attackAnimationTimer, actionQueTimer, actionID, leapTimer;
    [SerializeField] int leapCooldown, fissureCooldown, pillarCooldown, columnCooldown, pillarCharges;
    float initialY;

    const int LEAP = 0, FISSURE = 1, PILLAR_THROW = 2, COLUMN = 3;

    public static Transform targetTrfm;
    Transform trfm;
    Rigidbody rb;

    private void Awake()
    {
        targetTrfm = target;
    }

    new void Start()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody>();

        playerPositions = new Vector3[10];

        //pillarCharges = 500;
        leapCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        fissureCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        columnCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);


        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!IsStunned() && groundTrigger.IsOnGround() && actionQueTimer < 1)
        {
            if (columnCooldown > 0) { columnCooldown--; } else
            {
                Instantiate(fallingPillar, GetPredictedPos(1) + Vector3.up * 30, fallingPillar.transform.rotation);
                Instantiate(fallingPillarTelegraph, GetPredictedPos(1), fallingPillarTelegraph.transform.rotation);
                columnCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
            }
            if (leapCooldown > 0) { leapCooldown--; } else
            {
                QueAttack(LEAP);
            }
            if (fissureCooldown > 0) { fissureCooldown--; } else
            {
                if (TargetInRange(range) && !TargetInRange(fissureMinRange))
                {
                    QueAttack(FISSURE);
                }
            }
            if (pillarCooldown > 0) { pillarCooldown--; } else
            {
                QueAttack(PILLAR_THROW);
            }
            if (pillarCharges < 120)
            {
                pillarCharges++;
            }
        }

        if (actionQueTimer > 0)
        {
            if (actionQueTimer == 1)
            {
                if (actionID == FISSURE)
                {
                    FaceTarget(Vector3.Distance(target.position, trfm.position) / 20f);
                    Instantiate(fissureProjectile, trfm.position, trfm.rotation);
                    SetAnimationLock(25);
                    fissureCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
                }
                else if (actionID == LEAP)
                {
                    DoLeapAttack();
                }
                else if (actionID == PILLAR_THROW)
                {
                    int numPillars = pillarCharges / 20;
                    SetAnimationLock(15 * numPillars);

                    for (int i = 0; i < numPillars; i++)
                    {
                        Instantiate(pillarProjectile, trfm.position + trfm.forward * 2, trfm.rotation).GetComponent<ThrownEarthPillar>().Init(i * 15, target, this);
                        trfm.Rotate(Vector3.up * 360 / numPillars);
                    }

                    pillarCharges = pillarCharges % 20;
                    pillarCooldown = Random.Range(cooldownRange[0], cooldownRange[1]) * 2;
                }
                else if (actionID == COLUMN)
                {
                    
                }
            }
            actionQueTimer--;
        }

        if (calculateTimer > 0) { calculateTimer--; }
        else
        {
            calculateTimer = 5;
            CalculatePredictedPos();
        }

        if (leapTimer > 0)
        {
            if (leapTimer == 1)
            {
                meshRenderer.material = defaultMaterial;
                pillarDestroyer.enabled = false;
            }
            leapTimer--;
        }
    }

    void QueAttack(int ID)
    {
        actionID = ID;
        actionQueTimer = 40;

        if (ID == LEAP) leapTelegraph.Play();
        if (ID == FISSURE) fissureTelegraph.Play();
    }

    void DoLeapAttack()
    {
        rammingHitbox.EnableHitbox(Mathf.RoundToInt(leapDuration * 45));
        leapTimer = 46;
        meshRenderer.material = redMaterial;
        //pillarDestroyer.enabled = true;

        SetAnimationLock(Mathf.RoundToInt(leapDuration * 50 + 25));

        FaceTarget(leapDuration);

        //leapDuration *= Vector3.Distance(target.position, trfm.position) / 10;

        rb.velocity += Vector3.up * -Physics.gravity.y / 2 * leapDuration;

        float distance = Vector3.Distance(GetPredictedPos(leapDuration), trfm.position);
        if (distance > range) { distance = range; }
        if (distance < 10) { distance = 10; }
        rb.velocity += trfm.forward * distance / leapDuration;

        if (Random.Range(0,100) < leapChainChance)
        {
            leapCooldown = 10;
        }
        else
        {
            leapCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
            Stun(100);
        }
        initialY = transform.position.y;
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
        trfm.forward = (GetPredictedPos(predictionTime) - trfm.position);
    }

    [SerializeField] Vector3[] playerPositions;
    Vector3 predictedOffset;
    int addPos, calculateTimer;
    public Vector3 GetPredictedPos(float seconds, bool verticalTargeting = true)
    {
        int newestPos = addPos - 3;
        if (newestPos < 0) { newestPos += 10; }
        predictedOffset = (target.position - playerPositions[addPos]) * seconds * .5f;
        predictedOffset += (target.position - playerPositions[newestPos]) * 4 * seconds * .5f;

        if (verticalTargeting) { return predictedOffset + target.position; }
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<EarthPillar>())
        {
            Destroy(col.gameObject);
        }
    }
}
