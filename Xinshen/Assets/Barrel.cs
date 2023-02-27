using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Enemy
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }
}
