using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEarthPillar : EarthPillar
{
    [SerializeField] int damage;
    [SerializeField] float fallSpeed;
    int delay;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (trfm.position.y > 7) { trfm.position -= Vector3.up * fallSpeed; }
        else
        {
            if (delay == 0)
            {
                Inertenize(GetComponent<CapsuleCollider>());
                CameraController.SetTrauma(15);
            }
            delay++;
        }

        if (delay > 200)
        {
            trfm.position -= Vector3.up * fallSpeed * .2f;

            if (delay > 350)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8 && !inert)
        {
            PlayerHP.TakeDamage(damage);
        }
    }
}
