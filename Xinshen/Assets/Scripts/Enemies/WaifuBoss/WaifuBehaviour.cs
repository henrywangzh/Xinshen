using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaifuBehaviour : Enemy
{
    [SerializeField] [Tooltip("Beyond this distance, the enemy will not evade backwards")] float strafeRadius = 5f;
    [SerializeField] [Tooltip("Time interval for which the boss will do something")] float decisionPeriod = 3f;
    [SerializeField] float dashSpeed = 3f;
    [SerializeField] Transform player;
    [SerializeField] GameObject star;
    [SerializeField] GameObject shard;
    [SerializeField] GameObject bomb;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem ps;
    [SerializeField] [Tooltip("Element 0 - evade, element 1 - splitter star, element 2 - basic attack, element 3 - rush attack")] List<int> delays;
    List<int> counter;
    float startTime;
    bool attacking = false;
    Rigidbody rb;
    Collider col;
    Animator anim;
    ParticleSystem.MainModule psmain;
    bool stun = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        psmain = ps.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startTime = Time.timeSinceLevelLoad;
        counter = new List<int>(delays);
        // Debug.Log(counter[0]);
        col = GetComponent<CapsuleCollider>();
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z), 0.1f);
        if ((Time.timeSinceLevelLoad - startTime) % decisionPeriod <= Time.deltaTime)  // Make a decision every few seconds
        {
            // Debug.Log("Making decision");
            if (attacking) return;
            if (IsStunned())
            {
                if (!stun)
                {
                    // Play animation
                    stun = true;
                }
                Debug.Log("Stunned");
                return;
            } else if (stun)
            {
                stun = false;
            }

            for (int i = 0; i < counter.Count; i++)
            {
                if (counter[i] > 0)
                    counter[i]--;
            }

            // Given a strafe radius, attacks player outside the radius, and evades inside the radius
            if (Vector3.Distance(transform.position, player.position) < strafeRadius)
            {
                if (counter[0] == 0) {
                    counter[0] = delays[0];
                    StartCoroutine(Evade());
                    return;
                }
            } else
            {
                if (counter[1] == 0)
                {
                    counter[1] = delays[1];
                    StartCoroutine(SplitterStars());
                    return;
                }

                if (counter[3] == 0)
                {
                    counter[3] = delays[3];
                    StartCoroutine(RushAssault());
                    return;
                }
            }
            if (counter[2] == 0)
            {
                counter[2] = delays[2];
                StartCoroutine(Attack());
                return;
            }
        }
    }

    // Note: All actions should be done within 2 seconds, or whatever the decision period is

    public void PlayParticles(Color color)
    {
        psmain.startColor = color;
        ps.Emit(10);
    }

    public void SpawnStars()
    {
        GameObject obj = Instantiate(star, transform.position + transform.up * 1.5f, transform.rotation);
        obj.GetComponent<SplitterStar>().AssignTarget(player);
    }

    public void BasicAttack()
    {
        GameObject obj = Instantiate(shard, transform.position + transform.up, transform.rotation);
        obj.GetComponent<CometShard>().AssignTargetTrfm(player);
    }

    IEnumerator SplitterStars()
    {
        Color orange = new Color(255, 128, 0, 1);
        PlayParticles(orange);
        yield return new WaitForSeconds(.5f);
        PlayParticles(Color.red);
        yield return new WaitForSeconds(.5f);
        anim.Play("WaifuStar");
        yield return new WaitForSeconds(0.8f);
        SpawnStars();
    }

    IEnumerator Attack()
    {
        attacking = true;
        PlayParticles(Color.cyan);
        yield return new WaitForSeconds(.5f);
        anim.Play("WaifuAttack");
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < 3; i++)
        {
            BasicAttack();
            yield return new WaitForSeconds(.2f);
        }
        attacking = false;
    }

    IEnumerator Evade()
    {
        attacking = true;
        // point that lies on the strafe radius
        // Vector3 pointOnCircle = player.position + (transform.position - player.position).normalized * strafeRadius;
        Vector3 backVector = (transform.position - player.position).normalized;
        backVector = (backVector - new Vector3(0, backVector.y, 0)).normalized;
        Vector3 sideVector = new Vector3(backVector.z, 0, backVector.x * -1);

        /*
        float distFromPlayer = Vector3.Distance(transform.position, player.position);
        float maxDistBack = strafeRadius / 2;
        float distBack = maxDistBack;
        if (strafeRadius - distFromPlayer < maxDistBack)
        {
            distBack = strafeRadius - distFromPlayer;
        }
        float distSide = strafeRadius - distBack;
        */
        anim.Play("WaifuEvade");
        float duration = 1f;
        // The dodge should curve, from fully perpendicular to fully tangential from the perimeter around the player
        if (Random.Range(0f, 1f) > 0.5f)  // Choose a random direction to dodge
        {
            sideVector *= -1;
        }
        while (duration > 0)
        {
            rb.velocity = (backVector * Mathf.Pow(duration, 2) + sideVector * (1 - Mathf.Pow(duration, 2))).normalized * dashSpeed;
            yield return new WaitForSeconds(0.1f);
            duration -= 0.1f;
        }
        BasicAttack();
        attacking = false;
    }

    // Dash through the player, leaving bombs in the trail (Does not account for falling off edges)
    IEnumerator RushAssault()
    {
        Color orange = new Color(255, 128, 0, 1);
        attacking = true;
        PlayParticles(Color.red);
        yield return new WaitForSeconds(.3f);
        PlayParticles(orange);
        yield return new WaitForSeconds(.3f);
        PlayParticles(Color.yellow);
        yield return new WaitForSeconds(.3f);
        for (int i = 0; i < 3; i++)
        {
            Vector3 direction = (player.position - transform.position);
            direction = (direction - new Vector3(0, direction.y, 0)).normalized;
            Vector3 destPoint = transform.position + direction * strafeRadius * 2;
            rb.velocity = direction * dashSpeed * 3;
            rb.useGravity = false;
            col.enabled = false;
            trail.emitting = true;
            anim.Play("WaifuRush");
            float duration = 2f;
            float horiOffset = 0.25f;
            while (duration > 0 && Vector3.Distance(transform.position, destPoint) > 0.4f)
            {
                rb.velocity = direction * dashSpeed * 3;
                yield return new WaitForSeconds(0.025f);
                duration -= 0.025f;
                if (duration % 0.1f <= Time.deltaTime)
                {
                    Instantiate(bomb, transform.position + transform.up * 1.5f + transform.right * horiOffset, Quaternion.identity);
                    horiOffset *= -1;
                }
            }
            Instantiate(bomb, transform.position + transform.up * 1.5f, Quaternion.identity);
            trail.emitting = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            col.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
        attacking = false;
    }

    public override void Die()
    {
        Debug.Log("GG NO RE");
        Destroy(gameObject);
    }
}
