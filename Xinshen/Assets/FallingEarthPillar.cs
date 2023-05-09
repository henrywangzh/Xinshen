using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEarthPillar : EarthPillar
{
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            PlayerHP.TakeDamage(damage);
        }
    }
}
