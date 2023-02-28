using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometShard : MonoBehaviour
{
    [SerializeField] Transform targetTrfm;
    [SerializeField] float travelSpd, turnSpd;
    [SerializeField] int damage;
    [SerializeField] GameObject destroyFX;

    Transform trfm;

    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;

        // trfm.forward = targetTrfm.position - trfm.position;
        // trfm.Rotate(new Vector3(Random.Range(160, 201), Random.Range(-20, 21), 0));
    }

    public void AssignTargetTrfm(Transform p_TargetTrfm)
    {
        targetTrfm = p_TargetTrfm;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.forward += ((targetTrfm.position - trfm.position) - trfm.forward) * turnSpd;
        trfm.position += trfm.forward * travelSpd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerHP.TakeDamage(damage);
            DestroySelf();
        }
        if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(Instantiate(destroyFX, trfm.position, trfm.rotation), 1);
        Destroy(gameObject);
    }
}
