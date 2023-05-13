using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownEarthPillar : EarthPillar
{
    [SerializeField] float riseRate, flightSpeed;
    [SerializeField] int damage, launchDelay, delay;
    [SerializeField] EarthBendingBoss bossScript;
    int collisionDelay;
    Rigidbody rb;
    bool inFlight;
    [SerializeField] Collider hitbox;
    [SerializeField] Transform target;
    Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(int pDelay, Transform pTarget, EarthBendingBoss pBossScript)
    {
        target = pTarget;
        delay = pDelay;
        bossScript = pBossScript;
        trfm.forward = target.position - trfm.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delay > 0)
        {
            delay--;
        }
        else if (launchDelay > 0)
        {
            trfm.position += Vector3.up * riseRate;
            trfm.forward = target.position - trfm.position;
            launchDelay--;
        }
        else
        {
            collisionDelay++;
            Launch();
            delay = 999;
        }

        if (inFlight)
        {
            trfm.forward = lastPos - trfm.position;
            lastPos = trfm.position;
        }
    }

    void Launch()
    {
        hitbox.enabled = true;
        rb.useGravity = true;

        float distance = Vector3.Distance(target.position, trfm.position);
        float flightTime = distance / flightSpeed / 50f;
        Debug.Log("flight time: " + flightTime);

        //rb.velocity += Vector3.up * -Physics.gravity.y * 1000 * flightTime;

        rb.velocity += (target.position - trfm.position).normalized * flightSpeed * 52;
        rb.velocity += Vector3.up * -Physics.gravity.y * .5f * flightTime;

        Debug.Log(rb.velocity);

        lastPos = trfm.position;
        inFlight = true;
    }
    protected void FaceTarget()
    {
        trfm.forward = target.position - trfm.position;
    }

    private Vector3 GetDistancedTargetPosition()
    {
        return bossScript.GetPredictedPos(Vector3.Distance(target.position, trfm.position));
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            PlayerHP.TakeDamage(damage);
        }

        if (collisionDelay > 24 && col.gameObject.layer == 6)
        {
            Destroy(rb);
            Destroy(GetComponent<ThrownEarthPillar>());
        }
    }
}
