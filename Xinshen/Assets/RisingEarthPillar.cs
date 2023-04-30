using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingEarthPillar : EarthPillar
{
    [SerializeField] int duration;
    [SerializeField] float riseRate;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //trfm.Rotate(Vector3.right * Random.Range(-85, -34));
        trfm.Rotate(Vector3.right * Random.Range(0, 25));
        trfm.Rotate(Vector3.up * Random.Range(-25, 26));
        duration += Random.Range(-5, 6);
        //trfm.position += trfm.forward * riseRate * (duration + 2);
        trfm.localScale += Vector3.right * Random.Range(-.4f, .4f);
        trfm.localScale += Vector3.up * Random.Range(-.4f, .4f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (duration > 0)
        {
            trfm.position -= trfm.forward * riseRate;
            duration--;
        }
    }
}
