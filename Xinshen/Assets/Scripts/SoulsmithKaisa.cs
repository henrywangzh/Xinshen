using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsmithKaisa : MonoBehaviour
{
    [SerializeField] Transform targetTrfm;
    [SerializeField] GameObject bullet;
    [SerializeField] int firingDuration, firingDelay;
    int firingTimer;
    // Start is called before the first frame update
    void Start()
    {
        //GlobalVariableManager.SetLockedTarget(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            firingTimer = firingDuration;
        }
    }

    private void FixedUpdate()
    {
        if (firingTimer > 0)
        {
            firingTimer--;

            if (firingTimer % firingDelay == 0)
            {
                Instantiate(bullet, transform.position, transform.rotation).GetComponent<KaisaMissile>().AssignTargetTrfm(targetTrfm);
            }
        }
    }
}
