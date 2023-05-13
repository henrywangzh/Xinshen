using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPillar : MonoBehaviour
{
    protected Transform trfm;
    protected bool inert;
    // Start is called before the first frame update
    protected void Awake()
    {
        trfm = transform;
        //trfm.localEulerAngles = new Vector3(90, 0, 0);
    }

    protected void Inertenize(Collider col) //a real word that means 'to become inert'
    {
        inert = true;
        //col.isTrigger = false;
        gameObject.layer = 0;
        col.gameObject.layer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
