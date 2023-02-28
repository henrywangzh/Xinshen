using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaifuBehaviour : MonoBehaviour
{
    [SerializeField] [Tooltip("Beyond this distance, the enemy will not evade backwards")] float strafeRadius = 5f;
    [SerializeField] [Tooltip("Time interval for which the boss will do something")] float decisionPeriod = 3f;
    [SerializeField] float dashSpeed = 3f;
    [SerializeField] Transform player;
    [SerializeField] GameObject star;
    [SerializeField] GameObject shard;
    [SerializeField] [Tooltip("Element 0 - evade, element 1 - splitter star, element 2 - basic attack")] List<int> delays;
    List<int> counter;
    float startTime;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTime = Time.timeSinceLevelLoad;
        counter = new List<int>(delays);
        Debug.Log(counter[0]);
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z), 0.2f);
        if ((Time.timeSinceLevelLoad - startTime) % decisionPeriod <= Time.deltaTime)  // Make a decision every few seconds
        {
            // Debug.Log("Making decision");
            
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
                    SpawnStars();
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

    public void SpawnStars()
    {
        GameObject obj = Instantiate(star, transform.position + transform.up, transform.rotation);
        obj.GetComponent<SplitterStar>().AssignTarget(player);
    }

    public void BasicAttack()
    {
        GameObject obj = Instantiate(shard, transform.position, transform.rotation);
        obj.GetComponent<CometShard>().AssignTargetTrfm(player);
    }

    IEnumerator Attack()
    {
        for (int i = 0; i < 3; i++)
        {
            BasicAttack();
            yield return new WaitForSeconds(.2f);
        }
    }

    IEnumerator Evade()
    {
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
    }
    
}
