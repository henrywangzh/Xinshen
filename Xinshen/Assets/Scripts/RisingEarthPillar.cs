using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingEarthPillar : EarthPillar
{
    [SerializeField] int duration, damage;
    [SerializeField] float riseRate;
    [SerializeField] int manualDelay;
    [SerializeField] MeshCollider meshCol;
    // Start is called before the first frame update
    void Start()
    {
        //trfm.Rotate(Vector3.right * Random.Range(-85, -34));
        //trfm.Rotate(Vector3.right * Random.Range(0, 25));

        //trfm.Rotate(Vector3.up * Random.Range(-25, 26));

        //trfm.forward = EarthBendingBoss.targetTrfm.position - trfm.position;
        //trfm.Rotate(trfm.forward * 45);
        duration += Random.Range(-3, 7);
        //trfm.position += trfm.forward * riseRate * (duration + 2);
        trfm.localScale += Vector3.right * Random.Range(-.4f, .4f);
        trfm.localScale += Vector3.up * Random.Range(-.4f, .4f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        manualDelay++;
        duration--;

        if (duration > 0)
        {
            trfm.position -= trfm.forward * riseRate;
            if (duration == 1) { Inertenize(meshCol); }
        }
        if (duration < -250)
        {
            trfm.position += trfm.forward * riseRate * .5f;
            if (duration < -350)
            {
                Destroy(gameObject);
            }
        }
        //trfm.position += trfm.forward * .1f;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8 && !inert)
        {
            PlayerHP.TakeDamage(damage);
        }
    }
}
