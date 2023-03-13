using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTunnel : MonoBehaviour
{
    [SerializeField] float windStrength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Rigidbody rb = other.gameObject.transform.root.GetComponentInChildren<Rigidbody>();
            if(rb)
            {
                rb.AddForce(new Vector3(0, windStrength, 0));
            }
        }
    }
}
