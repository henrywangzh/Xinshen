using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : Enemy
{
    [SerializeField] GameObject shatteredVersion;
    [SerializeField] GameObject parent;

    public override void Die()
    {
        GameObject shatteredObject = Instantiate(shatteredVersion, transform.position, Quaternion.identity);

        Rigidbody[] rigidbodies = shatteredObject.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rigidbodies)
        {
                rb.AddExplosionForce(Random.Range(500, 1000), transform.position, 10);
        }

        Destroy(parent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
