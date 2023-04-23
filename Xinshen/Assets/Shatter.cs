using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : Enemy
{
    [SerializeField] GameObject shatteredVersion;
    [SerializeField] GameObject parent;

    public override void Die()
    {
        Instantiate(shatteredVersion, transform.position, Quaternion.identity);
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
