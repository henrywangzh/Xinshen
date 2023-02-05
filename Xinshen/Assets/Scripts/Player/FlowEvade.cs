using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEvade : MonoBehaviour
{
    FlowScriptController controller;
    Rigidbody rb;
    Vector3 destination;
    
    [SerializeField] float dashSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FlowScriptController>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.velocity = transform.forward * dashSpeed;

        // Now we've totally evaded
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.switchState.Invoke("move");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // controller.switchState.Invoke("jump");
        }
    }
}
