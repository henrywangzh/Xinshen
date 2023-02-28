using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterStar : MonoBehaviour
{
    [SerializeField] [Tooltip("Lifespan in seconds")] float lifetime = 6f;
    [SerializeField] [Tooltip("Turning speed in terms of PI, scaled with deltaTime")] float turnSpeed = 2f;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float flightSpeed = 3f;
    [SerializeField] GameObject star;
    Rigidbody rb;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Commence());
    }

    public void AssignTarget(Transform targ)
    {
        target = targ;
    }

    IEnumerator Commence()
    {
        Vector3 vertRef = transform.up;
        Vector3 horiRef = transform.right;

        if (Random.Range(0, 1f) > 0.5f)
        {
            horiRef *= -1;
        }

        if (Random.Range(0, 1f) > 0.5f)
        {
            // vertRef *= -1;
        }

        Vector3 direction = Vector3.RotateTowards(vertRef, horiRef, Mathf.PI / 2 * Random.Range(0.5f, 1f), 0);
        float duration = lifetime;
        while (duration > 0)
        {
            rb.velocity = transform.forward * flightSpeed * (duration / lifetime);
            transform.forward = Vector3.RotateTowards(transform.forward, transform.forward + direction, turnSpeed * Mathf.PI * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.1f);
            duration -= 0.1f;
            if (duration % spawnInterval <= Time.deltaTime)
            {
                GameObject obj = Instantiate(star, transform.position, Quaternion.identity);
                obj.GetComponent<CometShard>().AssignTargetTrfm(target);
                GameObject obj2 = Instantiate(star, transform.position, transform.rotation);
                obj2.GetComponent<CometShard>().AssignTargetTrfm(target);
            }
        }
        yield return new WaitForSeconds(3f);
        GameObject obj5 = Instantiate(star, transform.position, Random.rotation);
        obj5.GetComponent<CometShard>().AssignTargetTrfm(target);
        GameObject obj6 = Instantiate(star, transform.position, Random.rotation);
        obj6.GetComponent<CometShard>().AssignTargetTrfm(target);
        GameObject obj3 = Instantiate(star, transform.position, Random.rotation);
        obj3.GetComponent<CometShard>().AssignTargetTrfm(target);
        GameObject obj4 = Instantiate(star, transform.position, Random.rotation);
        obj4.GetComponent<CometShard>().AssignTargetTrfm(target);
        Destroy(gameObject);
    }
}
