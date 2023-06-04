using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Animator anim;

    int attackAnimationTimer, actionQueTimer, actionID, leapTimer;
    [SerializeField] int leapCooldown, fissureCooldown, pillarCooldown, columnCooldown, pillarCharges;
    float initialY;
    private bool landed = true;

    const int LEAP = 0, FISSURE = 1, PILLAR_THROW = 2, COLUMN = 3;

    public static Transform targetTrfm;
    Transform trfm;
    Rigidbody rb;

    bool phaseTwoActivated;
    int lastHP;

    private void Awake()
    {
        targetTrfm = target;
    }

    new void Start()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //pillarCharges = 500;
        leapCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        fissureCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        columnCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);

        //onAtkInterrupt.AddListener(OnAttackInterrupt);
        //onStun.AddListener(OnStunned);

        lastHP = GetHP();
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
            if (phaseTwoActivated)
            {
                if (columnCooldown > 0) { columnCooldown--; }
                else
                {
                    // anim.Play("dropPillar");
                    Instantiate(fallingPillar, PredictionManager.GetPredictedPos(1) + Vector3.up * 30, fallingPillar.transform.rotation);
                    Instantiate(fallingPillarTelegraph, PredictionManager.GetPredictedPos(1), fallingPillarTelegraph.transform.rotation);
                    columnCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
                }
            }
            if (leapCooldown > 0) { leapCooldown--; } else
            {
                QueueAttack(LEAP);
            }
            if (fissureCooldown > 0) { fissureCooldown--; } else
            {
                if (TargetInRange(range) && !TargetInRange(fissureMinRange))
                {
                    FaceTarget(Vector3.Distance(target.position, trfm.position) / 20f);
                    QueueAttack(FISSURE);
                }
            }
            if (pillarCooldown > 0) { pillarCooldown--; } else
            {
                QueueAttack(PILLAR_THROW);
            }
            if (pillarCharges < 120)
            {
                pillarCharges++;
            }
        }

        if (actionQueTimer > 0)
        {
            if (actionID == FISSURE)
            {
                FaceTarget(Vector3.Distance(target.position, trfm.position) / 20f);
            }
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
                    anim.Play("dash");
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

        if (!groundTrigger.IsOnGround())
        {
            landed = false;
        }
        if (groundTrigger.IsOnGround() && landed ==false)
        {
            landed  = true;
            rb.velocity = Vector3.zero;
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

        if (lastHP != GetHP())
        {
            lastHP = GetHP();

            if (!phaseTwoActivated && lastHP <= 200)
            {
                cooldownRange[0] = 50;
                cooldownRange[1] = 80;
                leapChainChance = 75;
                phaseTwoActivated = true;
            }
        }
    }

    void QueueAttack(int ID)
    {
        actionID = ID;
        actionQueTimer = 40;

        if (ID == LEAP){leapTelegraph.Play();}
        if (ID == FISSURE)
        {
            anim.Play("earthbend");
            fissureTelegraph.Play();
        }
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

        float distance = Vector3.Distance(PredictionManager.GetPredictedPos(leapDuration), trfm.position);
        if (distance > range) { distance = range; }
        if (distance < 10) { distance = 10; }
        rb.velocity += trfm.forward * distance / leapDuration;
        
        SetLeapCooldown();

        initialY = transform.position.y;
    }

    void SetLeapCooldown()
    {
        if (Random.Range(0, 100) < leapChainChance)
        {
            leapCooldown = 10;
        }
        else
        {
            leapCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
            Stun(100);
        }
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
        trfm.forward = (PredictionManager.GetPredictedPos(predictionTime, false) - trfm.position);
    }

    public override void Die()
    {
        Debug.Log("GG NO RE");
        Destroy(gameObject);
    }

    void OnAttackInterrupt()
    {
        Debug.Log("atk interrupted");
        if (actionID == FISSURE)
        {
            fissureCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        }
        else if (actionID == LEAP)
        {
            SetLeapCooldown();
        }
        else if (actionID == PILLAR_THROW)
        {
            pillarCooldown = Random.Range(cooldownRange[0], cooldownRange[1]);
        }

        actionQueTimer = 0;
    }
    void OnStunned()
    {
        Debug.Log("stunned");
        OnAttackInterrupt();
        Stun(50);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<EarthPillar>())
        {
            Destroy(col.gameObject);
        }
    }
}
