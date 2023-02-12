using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEvade : MonoBehaviour
{
    FlowScriptController controller;
    Rigidbody rb;
    Vector3 destination;
    
    [SerializeField] float dashSpeed = 5;
    [SerializeField] float totalDashTime = 0.5f;

    float currentDashTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FlowScriptController>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(rb)
        {
            float xinput = Input.GetAxis("Horizontal");
            float yinput = Input.GetAxis("Vertical");

            Vector3 inputDir = new Vector3(xinput, 0, yinput).normalized;

            if (inputDir.magnitude > 0)
            {
                rb.velocity = inputDir * dashSpeed;
                transform.forward = inputDir;
            }
            else
            {
                rb.velocity = transform.forward * dashSpeed;
            }
        }

        // Now we've totally evaded
    }
    private void OnDisable()
    {
        // reset dash timer when done
        currentDashTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        currentDashTime += Time.deltaTime;
        if(currentDashTime >= totalDashTime)
        {
            controller.switchState.Invoke("move");
        }
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
