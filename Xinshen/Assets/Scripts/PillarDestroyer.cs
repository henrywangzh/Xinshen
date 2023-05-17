using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<EarthPillar>())
        {
            Destroy(col.gameObject);
        }
    }
}
