using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPillar : MonoBehaviour
{
    protected Transform trfm;
    // Start is called before the first frame update
    protected void Awake()
    {
        trfm = transform;
        //trfm.localEulerAngles = new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
