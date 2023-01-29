using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody rb;
    private float xmove;
    private float zmove;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        xmove = Input.GetAxisRaw("Horizontal");
        zmove = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector3(xmove, rb.velocity.y, zmove);
    }
}
